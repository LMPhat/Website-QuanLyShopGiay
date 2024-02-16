using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;
namespace ShopGiay.Controllers
{
    public class KhachHangController : Controller
    {
        //
        // GET: /KhachHang/

        Sa_1DataContext dl = new Sa_1DataContext();
        public ActionResult MenuLoaiSP()
        {
            List<LOAISP> ds = dl.LOAISPs.ToList();
            return PartialView(ds);
        }

        public ActionResult MenuLoaiSP_Doc()
        {
            List<LOAISP> ds = dl.LOAISPs.ToList();
            return PartialView(ds);
        }

        public ActionResult MenuMucGia()//lọc theo giá
        {
            return PartialView();
        }
        public ActionResult HTSP_TheoGia(string id) //Hiển thị sản phẩm theo giá
        {
            if (id == "duoi500k")
            {
                List<SANPHAM> ds = dl.SANPHAMs.Where(t => t.GIABAN < 500000.0).ToList();
                return View("SanPham", ds);
            }
            else if (id == "tu500den700")
            {
                List<SANPHAM> ds = dl.SANPHAMs.Where(t => t.GIABAN >= 500000.0 && t.GIABAN < 700000.0).ToList();
                return View("SanPham", ds);
            }
            else
            {
                List<SANPHAM> ds = dl.SANPHAMs.Where(t => t.GIABAN > 700000.0).ToList();
                return View("SanPham", ds);
            }
        }
        public ActionResult SanPham()
        {
            List<SANPHAM> ds = dl.SANPHAMs.ToList();
            return View(ds);
        }

        public ActionResult HTSP_TheoLoai(string id)
        {
            List<SANPHAM> ds = dl.SANPHAMs.Where(t => t.MALOAI == id).ToList();
            return View("SanPham", ds);
        }

        [HttpPost]
        public ActionResult XLTimKiem(FormCollection fc)
        {
            string tensp = fc["search"];
            List<SANPHAM> ds1 = dl.SANPHAMs.Where(t => t.TENSP.Contains(tensp)).ToList();
            if (ds1.Count > 0)
            {
                return View("SanPham", ds1);
            }
            return View("SanPham", ds1);
        }
        //==============================================================================================================================//
        public ActionResult Details(string id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            else
            {
                SANPHAM sp = dl.SANPHAMs.Where(t => t.MASP == id).FirstOrDefault();
                ViewBag.Loai = dl.LOAISPs.Where(t => t.MALOAI == sp.MALOAI).FirstOrDefault();
                ViewBag.SanPham = sp;
                return View();
            }

        }
        [HttpPost]
        public ActionResult Details(AddCart crt, string id)
        {
            if (crt.MaKH == null)
            {
                Session["TempCart"] = crt;
                return RedirectToAction("Login", "TaiKhoan");
            }
            else
                if (ModelState.IsValid)
                {
                    HOADON cart = new HOADON();
                    List<Cart> product = new List<Cart>();
                    int flag = 0;
                    List<HOADON> hd = dl.HOADONs.Where(p => p.MAKH == crt.MaKH).ToList();
                    foreach (var item in hd)
                    {
                        if (dl.VANCHUYENs.ToList().Exists(p => p.SOHD == item.SOHD) != true)
                        {
                            flag = 1;
                            cart = item;
                            break;
                        }
                    }
                    if (flag == 1)
                    {
                        CHITIETHOADON chitiet = new CHITIETHOADON();
                        chitiet.SOHD = cart.SOHD;
                        chitiet.MASP = crt.MaSP;
                        chitiet.SOLUONG = crt.SoLuong;
                        chitiet.SIZE = crt.Size;
                        CHITIETHOADON chitiet2 = dl.CHITIETHOADONs.Where(p => p.SIZE == chitiet.SIZE && p.SOHD == chitiet.SOHD && p.MASP == chitiet.MASP).FirstOrDefault();
                        if (chitiet2 == null)
                        {
                            dl.CHITIETHOADONs.InsertOnSubmit(chitiet);
                            dl.SubmitChanges();
                        }
                        else
                        {
                            //SANPHAM SP = dl.SANPHAMs.Where(p => p.MASP == chitiet2.MASP).FirstOrDefault();
                            //chitiet2.SOLUONG += chitiet.SOLUONG;
                            //if (chitiet2.SOLUONG > SP.SOLUONG)
                            //    chitiet2.SOLUONG = SP.SOLUONG;
                            //dl.SubmitChanges();
                        }
                        return RedirectToAction("Cart", "KhachHang");
                    }
                    else
                    {
                        string idorder = "";
                        if (flag == 0)
                        {

                            if (dl.HOADONs.Count() == 0)
                                idorder = "ORD" + String.Format("{0:D6}", 1);
                            else
                            {
                                int max = dl.HOADONs.ToList().Max(i => Int32.Parse(i.SOHD.Remove(0, 2)));
                                idorder = "HD" + String.Format("{0:D3}", max + 1);
                            }
                        }
                        HOADON hdNEW = new HOADON();
                        hdNEW.SOHD = idorder;
                        hdNEW.TONGTIENHD = 0;
                        hdNEW.MAKH = crt.MaKH;
                        hdNEW.MANV = "NV001";
                        hdNEW.NGAYLAPHD = DateTime.Now;
                        dl.HOADONs.InsertOnSubmit(hdNEW);
                        dl.SubmitChanges();

                        CHITIETHOADON chitiet = new CHITIETHOADON();
                        chitiet.MASP = crt.MaSP;
                        chitiet.SOHD = hdNEW.SOHD;
                        chitiet.SOLUONG = crt.SoLuong;
                        chitiet.SIZE = crt.Size;
                        dl.CHITIETHOADONs.InsertOnSubmit(chitiet);
                        dl.SubmitChanges();
                        return RedirectToAction("Cart", "KhachHang");
                    }
                }
                else
                {
                    SANPHAM sp = dl.SANPHAMs.Where(t => t.MASP == id).FirstOrDefault();
                    ViewBag.Loai = dl.LOAISPs.Where(t => t.MALOAI == sp.MALOAI).FirstOrDefault();
                    ViewBag.SanPham = sp;
                    return View(crt);
                }

        }
        public ActionResult Cart()
        {

            if (Session["kh"] != null)
            {
                KHACHHANG kh = Session["kh"] as KHACHHANG;
                string giohang = "";
                List<HOADON> hd = dl.HOADONs.Where(p => p.MAKH == kh.MAKH).ToList();
                List<Cart> product = new List<Cart>();
                List<SANPHAM> listsp = new List<SANPHAM>();
                foreach (var item in hd)
                {
                    if (dl.VANCHUYENs.ToList().Exists(p => p.SOHD == item.SOHD) != true)
                    {

                        List<CHITIETHOADON> sanpham = dl.CHITIETHOADONs.Where(p => p.SOHD == item.SOHD).ToList();
                        foreach (var i in sanpham)
                        {
                            SANPHAM sp = dl.SANPHAMs.Where(p => p.MASP == i.MASP).FirstOrDefault();
                            Cart a = new Cart(i.SOHD, i.MASP, sp.TENSP, sp.HINHANH, sp.GIABAN.Value, i.SOLUONG.Value, i.SIZE);
                            listsp.Add(sp);
                            product.Add(a);
                        }
                        ViewBag.Bill = item;

                    }
                }
                ViewBag.Cart = product;
                ViewBag.ListPro = listsp;

                int max = dl.VANCHUYENs.ToList().OrderBy(p => p.MAVC).Max(i => Int32.Parse(i.MAVC.Remove(0, 2)));
                ViewBag.MaVC = "VC" + String.Format("{0:D3}", max + 1);
                return View();
            }
            else
            {
                ViewBag.Cart = null;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Delete(string MASP, string MAHD, int SIZE)
        {
            CHITIETHOADON ct = dl.CHITIETHOADONs.Where(p => p.MASP == MASP && p.SOHD == MAHD && p.SIZE == SIZE).FirstOrDefault();
            if (ct != null)
            {
                dl.CHITIETHOADONs.DeleteOnSubmit(ct);
                dl.SubmitChanges();
                return RedirectToAction("Cart", "KhachHang");
            }
            return RedirectToAction("Cart", "KhachHang");
        }
        [HttpPost]
        public ActionResult Change(string MASP, string MAHD, int OLDSIZE, string SOLUONG, int NEWSIZE)
        {

            int sl = 0;
            if (SOLUONG != "")
            {
                sl = Int16.Parse(SOLUONG);
            }

            CHITIETHOADON ct = dl.CHITIETHOADONs.Where(p => p.MASP == MASP && p.SOHD == MAHD && p.SIZE == OLDSIZE).FirstOrDefault();
            if (ct != null)
            {

                if (OLDSIZE != NEWSIZE)
                {
                    CHITIETHOADON ctsamesize = dl.CHITIETHOADONs.Where(p => p.MASP == MASP && p.SOHD == MAHD && p.SIZE == NEWSIZE).FirstOrDefault();
                    if (ctsamesize != null)
                    {
                        //SANPHAM SP = dl.SANPHAMs.Where(p => p.MASP == ctsamesize.MASP).FirstOrDefault();
                        //ctsamesize.SOLUONG += sl;
                        //if (ctsamesize.SOLUONG > SP.SOLUONG)
                        //    ctsamesize.SOLUONG = SP.SOLUONG;
                        //dl.CHITIETHOADONs.DeleteOnSubmit(ct);
                        //dl.SubmitChanges();
                    }
                    else
                    {
                        CHITIETHOADON temp = new CHITIETHOADON();
                        temp.SOHD = MAHD;
                        temp.SIZE = NEWSIZE;
                        temp.SOLUONG = sl;
                        temp.MASP = MASP;
                        dl.CHITIETHOADONs.DeleteOnSubmit(ct);
                        dl.CHITIETHOADONs.InsertOnSubmit(temp);
                        dl.SubmitChanges();
                    }
                }
                else if (sl == 0)
                {
                    dl.CHITIETHOADONs.DeleteOnSubmit(ct);
                    dl.SubmitChanges();
                }
                else
                {
                    ct.SOLUONG = sl;
                    dl.SubmitChanges();
                }
                return RedirectToAction("Cart", "KhachHang");
            }
            return RedirectToAction("Cart", "KhachHang");
        }
        [HttpPost]
        public ActionResult Cart(Confirm cn)
        {
            int slsp = dl.CHITIETHOADONs.Count(p => p.SOHD == cn.MAHD);
            if (ModelState.IsValid == true && slsp > 0)
            {
                VANCHUYEN VC = new VANCHUYEN();
                VC.MAVC = cn.MAVC;
                VC.SOHD = cn.MAHD;
                VC.NGAYGIAO = cn.NGAYGIAO;
                dl.VANCHUYENs.InsertOnSubmit(VC);
                HOADON hd = dl.HOADONs.Where(p => p.MAKH == cn.MAKH && p.SOHD == cn.MAHD).FirstOrDefault();
                hd.TONGTIENHD = (float)cn.TongTien;
                foreach (var item in dl.CHITIETHOADONs.Where(P => P.SOHD == cn.MAHD))
                {
                    //SANPHAM sp = dl.SANPHAMs.Where(p => p.MASP == item.MASP).FirstOrDefault();
                    //sp.SOLUONG -= item.SOLUONG;
                    //dl.SubmitChanges();
                }
                Session["annouce"] = null;
                return RedirectToAction("SanPham", "KhachHang");
            }
            else
            {
                Session["annouce"] = null;
                var annouce = "";
                if (ModelState.IsValid == false)
                    annouce += "Vui lòng chọn phương thức thanh toán!";
                if (slsp == 0)
                    annouce = "Vui lòng chọn sản phẩm trước khi thanh toán!";
                Session["annouce"] = annouce;
                return RedirectToAction("Cart", "KhachHang");
            }
        }
    }
}
