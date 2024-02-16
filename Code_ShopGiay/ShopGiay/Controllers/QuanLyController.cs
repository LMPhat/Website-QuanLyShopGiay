using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;

namespace ShopGiay.Controllers
{
    public class QuanLyController : Controller
    {
        //
        // GET: /QuanLy/
        Sa_1DataContext dl = new Sa_1DataContext();

        public ActionResult MenuLoaiSP()
        {
            List<LOAISP> ds = dl.LOAISPs.ToList();
            return PartialView(ds);
        }

        public ActionResult TheLoai()
        {
            List<LOAISP> ds = dl.LOAISPs.ToList();
            return View(ds);
        }

        [HttpPost]
        public ActionResult ThemTL(FormCollection fc)
        {
            //ViewBag.ErrorMessage = null;
            string tenloai = fc["tenLoai"].Trim().ToString();
            List<LOAISP> dsl = dl.LOAISPs.ToList();
            LOAISP l = new LOAISP();
            l = dsl.FirstOrDefault(f => string.Equals(f.TENLOAI, tenloai, StringComparison.OrdinalIgnoreCase));
            if(l == null)
            {
                LOAISP loai = new LOAISP();
                int t = dl.LOAISPs.Count();
                loai.MALOAI = "L0" + (t + 1).ToString();
                loai.TENLOAI = tenloai;

                dl.LOAISPs.InsertOnSubmit(loai);
                dl.SubmitChanges();
                return RedirectToAction("TheLoai", loai);
            }
            else
            {
                // Thông báo cho người dùng
                ViewBag.ErrorMessage = "Tên loại đã tồn tại.";

                return View("TheLoai", dsl); // Trả về View với thông báo lỗi
            }
        }

        [HttpPost]
        public ActionResult SuaTL(LOAISP t)
        {
            List<LOAISP> dsl = dl.LOAISPs.ToList();
            LOAISP l = dl.LOAISPs.FirstOrDefault(s => s.MALOAI != t.MALOAI && s.TENLOAI == t.TENLOAI);
            if (l == null)
            {
                LOAISP ft = dl.LOAISPs.FirstOrDefault(s => s.MALOAI == t.MALOAI);

                ft.TENLOAI = t.TENLOAI;

                UpdateModel(ft);
                dl.SubmitChanges();

                return RedirectToAction("TheLoai");
            }
            else
            {
                // Thông báo cho người dùng
                ViewBag.ErrorMessage = "Tên loại vừa sửa đã tồn tại.";

                return View("TheLoai", dsl); // Trả về View với thông báo lỗi
            }
        }

        [HttpPost]
        public ActionResult XoaTL(LOAISP l)
        {
            List<LOAISP> dsl = dl.LOAISPs.ToList();
            LOAISP ft = dl.LOAISPs.FirstOrDefault(t => t.MALOAI == l.MALOAI);
            SANPHAM sp = dl.SANPHAMs.FirstOrDefault(t => t.MALOAI == ft.MALOAI);
            if(sp == null)
            {
                dl.LOAISPs.DeleteOnSubmit(ft);
                dl.SubmitChanges();

                return RedirectToAction("TheLoai");
            }
            else
            {
                // Thông báo cho người dùng
                ViewBag.ErrorMessage = "Xóa loại không thành công.";

                return View("TheLoai", dsl); // Trả về View với thông báo lỗi
            }
        }

        public ActionResult NhaCungCap()
        {
            List<NHACUNGCAP> ds = dl.NHACUNGCAPs.ToList();
            return View(ds);
        }

        [HttpPost]
        public ActionResult ThemNCC(FormCollection fc)
        {
            string tenNCC = fc["tenNCC"].Trim().ToString();
            string diaChi = fc["diaChi"].Trim().ToString();
            string sdt = fc["sdt"].Trim().ToString();
            
            List<NHACUNGCAP> dsl = dl.NHACUNGCAPs.ToList();
            NHACUNGCAP l = new NHACUNGCAP();
            l = dsl.FirstOrDefault(f => string.Equals(f.TENNCC, tenNCC, StringComparison.OrdinalIgnoreCase) &&
                                        string.Equals(f.DC_NCC, diaChi, StringComparison.OrdinalIgnoreCase) &&
                                        f.SDT == sdt);
            if (l == null)
            {
                NHACUNGCAP loai = new NHACUNGCAP();
                int t = dl.NHACUNGCAPs.Count();
                loai.MANCC = "NCC0" + (t + 1).ToString();
                loai.TENNCC = tenNCC;
                loai.DC_NCC = diaChi;
                loai.SDT = sdt;

                dl.NHACUNGCAPs.InsertOnSubmit(loai);
                dl.SubmitChanges();
                return RedirectToAction("NhaCungCap", loai);
            }
            else
            {
                // Thông báo cho người dùng
                ViewBag.ErrorMessage = "Thông tin nhà cung cấp đã tồn tại.";

                return View("NhaCungCap", dsl); // Trả về View với thông báo lỗi
            }
        }

        [HttpPost]
        public ActionResult SuaNCC(NHACUNGCAP t)
        {
            List<NHACUNGCAP> dsncc = dl.NHACUNGCAPs.ToList();
            NHACUNGCAP l = dl.NHACUNGCAPs.FirstOrDefault(s => (s.MANCC != t.MANCC && s.DC_NCC != t.DC_NCC && s.TENNCC == t.TENNCC) || s.SDT == t.SDT);
            if (l == null)
            {
                NHACUNGCAP ft = dl.NHACUNGCAPs.FirstOrDefault(s => s.MANCC == t.MANCC);
                ft.TENNCC = t.TENNCC;
                ft.DC_NCC = t.DC_NCC;
                ft.SDT = t.SDT;

                UpdateModel(ft);
                dl.SubmitChanges();

                return RedirectToAction("NhaCungCap");
            }
            else
            {
                // Thông báo cho người dùng
                ViewBag.ErrorMessage = "Thông tin nhà cung cáp vừa sửa đã tồn tại.";

                return View("NhaCungCap", dsncc); // Trả về View với thông báo lỗi
            }
        }

        [HttpPost]
        public ActionResult XoaNCC(NHACUNGCAP n)
        {
            List<NHACUNGCAP> dsl = dl.NHACUNGCAPs.ToList();
            NHACUNGCAP ft = dl.NHACUNGCAPs.FirstOrDefault(t => t.MANCC == n.MANCC);
            PHIEUNHAP sp = dl.PHIEUNHAPs.FirstOrDefault(t => t.MANCC == ft.MANCC);
            if (sp == null)
            {
                dl.NHACUNGCAPs.DeleteOnSubmit(ft);
                dl.SubmitChanges();

                return RedirectToAction("NhaCungCap");
            }
            else
            {
                // Thông báo cho người dùng
                ViewBag.ErrorMessage = "Xóa nhà cung cấp không thành công.";

                return View("NhaCungCap", dsl); // Trả về View với thông báo lỗi
            }
        }

        public ActionResult SanPham()
        {
            List<SANPHAM> ds = dl.SANPHAMs.ToList();
            ViewBag.ctkt = dl.CHITIETKICHTHUOCs.ToList();
            ViewBag.maloai = dl.LOAISPs.ToList();
            return View(ds);
        }

        public ActionResult ThemSanPham(string str)
        {
            ViewBag.maloai = new SelectList(dl.LOAISPs.ToList(), "MALOAI", "TENLOAI");
            //ViewBag.makho = new SelectList(dl.KHOs.ToList(), "MAKHO", "TENKHO");
            ViewBag.makt = dl.KICHTHUOCs.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult ThemSanPham(SANPHAM d, CHITIETKICHTHUOC kt, HttpPostedFileBase fileUpload, FormCollection fc)
        {
            int t = dl.SANPHAMs.Count();
            KICHTHUOC s = dl.KICHTHUOCs.FirstOrDefault(k => k.MAKICHTHUOC == fc["MaKT"]);
            if (t >= 0 && t < 9)
            {
                d.MASP = "SP00" + (dl.SANPHAMs.Count() + 1).ToString();
            }
            else
            {
                d.MASP = "SP0" + (dl.SANPHAMs.Count() + 1).ToString();
            }
            //d.SOLUONG = int.Parse(fc["SoLuong"]);
            fileUpload.SaveAs(Server.MapPath("/ImagesSP/" + fileUpload.FileName));
            d.HINHANH = fileUpload.FileName;
            //d.SIZE = s.KICHTHUOC1.ToString();
            dl.SANPHAMs.InsertOnSubmit(d);//thêm vào bảng Sản phẩm
            dl.SubmitChanges();
            //kt.MAKICHTHUOC = fc["MaKT"];
            //kt.MASP = d.MASP;
            //dl.CHITIETKICHTHUOCs.InsertOnSubmit(kt); //thêm vào bảng chi tiết kích thước
            //dl.SubmitChanges();
            //c.MASP = d.MASP;
            //CapNhatGia(c, d); //thêm vào bảng Cập nhật giá
            return RedirectToAction("SanPham", d);
        }

        public ActionResult XoaSP(string id)
        {
            var sp = dl.SANPHAMs.FirstOrDefault(t => t.MASP == id);
            return View(sp);
        }
        [HttpPost]
        public ActionResult XoaSP(string id, FormCollection coll)
        {
            //Tìm 1 thể loại trong sql
            SANPHAM ft = dl.SANPHAMs.FirstOrDefault(t => t.MASP == id); //tìm được thể loại cũ
            //CAPNHATGIA cng = dl.CAPNHATGIAs.FirstOrDefault(t => t.MASP == id);
            CHITIETKICHTHUOC kt = dl.CHITIETKICHTHUOCs.FirstOrDefault(t => t.MASP == id);
            //if (cng != null)
            //{
            //    dl.CAPNHATGIAs.DeleteOnSubmit(cng);
            //    dl.SubmitChanges();
            //}
            dl.CHITIETKICHTHUOCs.DeleteOnSubmit(kt);
            dl.SubmitChanges();
            dl.SANPHAMs.DeleteOnSubmit(ft);
            dl.SubmitChanges(); //được thêm vào sql
            return RedirectToAction("SanPham");
        }

        //public ActionResult SuaSP(string id)
        //{
        //    SANPHAM t = dl.SANPHAMs.FirstOrDefault(s => s.MASP == id);
        //    return View(t);
        //}
        [HttpPost]
        public ActionResult SuaSP(SANPHAM t)
        {
            List<SANPHAM> dsl = dl.SANPHAMs.ToList();
            SANPHAM l = dl.SANPHAMs.FirstOrDefault(s => s.MASP != t.MASP && s.TENSP == t.TENSP && s.MALOAI == t.MALOAI);
            if (l == null)
            {
                SANPHAM ft = dl.SANPHAMs.FirstOrDefault(s => s.MASP == t.MASP);
                ft.TENSP = t.TENSP;
                ft.MALOAI = t.MALOAI;

                UpdateModel(ft);
                dl.SubmitChanges();

                return RedirectToAction("SanPham");
            }
            else
            {
                // Thông báo cho người dùng
                ViewBag.ErrorMessage = "Thông tin sửa đã tồn tại.";
                ViewBag.maloai = dl.LOAISPs.ToList();

                return View("SanPham", dsl); // Trả về View với thông báo lỗi
            }
        }
        //public void CapNhatGia(CAPNHATGIA c, SANPHAM t)
        //{
        //    c.GIABAN = t.GIABAN;
        //    c.NGAYCN = DateTime.Now;
        //    dl.CAPNHATGIAs.InsertOnSubmit(c);
        //    dl.SubmitChanges();
        //}

        [HttpPost]
        public ActionResult AddProduct(CHITIETPN c, FormCollection fc)
        {
            string MASP = fc["MASP"];
            string MAKICHTHUOC = fc["MAKICHTHUOC"];
            string SLNHAP = fc["SLNHAP"];
            string DGNHAP = fc["DGNHAP"];

            // Lấy sản phẩm từ CSDL dựa trên id hoặc theo cách bạn lấy sản phẩm
            SANPHAM product = dl.SANPHAMs.FirstOrDefault(t => t.MASP == MASP);
            KICHTHUOC kt = dl.KICHTHUOCs.FirstOrDefault(k => k.MAKICHTHUOC == fc["MAKICHTHUOC"]);
            //int kth = (int)kt.KICHTHUOC1;
            //int soluong = int.Parse(c.SLNHAP);
            
            CTPN item = new CTPN(product.MASP, product.TENSP, product.LOAISP.TENLOAI, float.Parse(DGNHAP), int.Parse(SLNHAP), (int)kt.KICHTHUOC1);

            // Kiểm tra xem Session["SelectedProducts"] đã tồn tại và khởi tạo nó nếu cần
            List<CTPN> selectedProducts = Session["SelectedProducts"] as List<CTPN>;

            bool exists = false;
            foreach (CTPN i in selectedProducts)
            {
                if (MASP == i.MASP && (int)kt.KICHTHUOC1 == i.size)
                {
                    i.giaNhap = item.giaNhap; // Cộng dồn giá nhập
                    i.soluong += item.soluong; // Cộng dồn số lượng
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                selectedProducts.Add(item); // Thêm mới vào danh sách
            }
            
            Session["SelectedProducts"] = selectedProducts;

            return RedirectToAction("ThemPhieuNhap"); // Chuyển hướng trở lại trang danh sách sản phẩm
        }

        [HttpGet]
        public ActionResult DeleteProduct(string id, string kt)
        {
            List<CTPN> selectedProducts = Session["SelectedProducts"] as List<CTPN>;

            int kth = int.Parse(kt);
            // Tìm và xóa sản phẩm dựa trên id (MASP)
            CTPN productToDelete = selectedProducts.FirstOrDefault(p => p.MASP == id && p.size == kth);
            if (productToDelete != null)
            {
                selectedProducts.Remove(productToDelete);
                Session["SelectedProducts"] = selectedProducts;
            }

            return RedirectToAction("ThemPhieuNhap"); 
        }

        public ActionResult PhieuNhap()
        {
            List<PHIEUNHAP> ds = dl.PHIEUNHAPs.ToList();
            ViewBag.mapn = dl.CHITIETPNs.ToList();
            return View(ds);
        }

        public ActionResult ThemPhieuNhap()
        {
            // Kiểm tra xem Session["SelectedProducts"] đã được khởi tạo chưa
            if (Session["SelectedProducts"] == null)
            {
                // Nếu chưa được khởi tạo, bạn có thể tạo một danh sách trống
                Session["SelectedProducts"] = new List<CTPN>();
            }

            ViewBag.mancc =dl.NHACUNGCAPs.ToList();
            ViewBag.makt = dl.KICHTHUOCs.ToList();
            ViewBag.masp = dl.SANPHAMs.OrderByDescending(s => s.MASP).ToList();

            return View();
        }
        [HttpGet]
        public ActionResult ThemPhieuNhap_CT(string mancc)
        {
            PHIEUNHAP pn = new PHIEUNHAP();
            NHACUNGCAP ncc = dl.NHACUNGCAPs.FirstOrDefault(n => n.MANCC == mancc);
            List<CTPN> selectedProducts = Session["SelectedProducts"] as List<CTPN>;
            NHANVIEN nv = Session["ad"] as NHANVIEN;

            int t = dl.PHIEUNHAPs.Count();
            if (t >= 0 && t < 9)
            {
                pn.MAPN = "PN00" + (dl.PHIEUNHAPs.Count() + 1).ToString();
            }
            else
            {
                pn.MAPN = "PN0" + (dl.PHIEUNHAPs.Count() + 1).ToString();
            }
            //Thêm phiếu nhập
            pn.NGAYLAP = DateTime.Now;
            pn.MANV = nv.MANV;
            pn.MANCC = ncc.MANCC;
            dl.PHIEUNHAPs.InsertOnSubmit(pn);
            dl.SubmitChanges();

            //Thêm chi tiết phiếu nhập
            foreach (CTPN item in selectedProducts)
            {
                CHITIETPN ct = new CHITIETPN();
                ct.MAPN = pn.MAPN;
                ct.MASP = item.MASP;
                ct.KICHTHUOC = item.size;
                ct.SLNHAP = item.soluong;
                ct.DGNHAP = (float?)item.giaNhap;

                dl.CHITIETPNs.InsertOnSubmit(ct);
                dl.SubmitChanges();
            }

            //Thêm chi tiết kích thước
            foreach (CTPN item in selectedProducts)
            {
                KICHTHUOC s = dl.KICHTHUOCs.FirstOrDefault(k => k.KICHTHUOC1 == item.size);
                CHITIETKICHTHUOC kt = dl.CHITIETKICHTHUOCs.FirstOrDefault(d => d.MASP == item.MASP && d.MAKICHTHUOC == s.MAKICHTHUOC);
                if(kt != null)
                {
                    kt.SOLUONG += item.soluong;

                    UpdateModel(kt);
                    dl.SubmitChanges();
                }
                else
                {
                    CHITIETKICHTHUOC kth = new CHITIETKICHTHUOC();
                    kth.MASP = item.MASP;
                    kth.MAKICHTHUOC = s.MAKICHTHUOC;
                    kth.SOLUONG = item.soluong;

                    dl.CHITIETKICHTHUOCs.InsertOnSubmit(kth);
                    dl.SubmitChanges();
                }
                
            }

            return RedirectToAction("PhieuNhap", pn);
        }

        public ActionResult HTSP_TheoLoai(string id)
        {
            List<SANPHAM> ds = dl.SANPHAMs.Where(t => t.MALOAI == id).ToList();
            return View("SanPham", ds);
        }

        [HttpPost]
        public ActionResult XLTimKiem(FormCollection fc)
        {
            string str = fc["search"];
            List<SANPHAM> ds1 = dl.SANPHAMs.Where(t => t.TENSP.Contains(str)).ToList();
            List<SANPHAM> ds2 = dl.SANPHAMs.Where(t => t.MASP.Contains(str)).ToList();
            if (ds1.Count > 0)
            {
                return View("SanPham", ds1);
            }
            else
            {
                if (ds2.Count > 0)
                {
                    return View("SanPham", ds2);
                }
                return View("SanPham");
            }
        }

        public ActionResult NhanVien()
        {
            List<NHANVIEN> ds = dl.NHANVIENs.ToList();
            return View(ds);
        }

        [HttpPost]
        public ActionResult XLTimKiem_NV(FormCollection fc)
        {
            string str = fc["search"];
            List<NHANVIEN> ds1 = dl.NHANVIENs.Where(t => t.HOTEN.Contains(str)).ToList();
            List<NHANVIEN> ds2 = dl.NHANVIENs.Where(t => t.MANV.Contains(str)).ToList();
            if (ds1.Count > 0)
            {
                return View("NhanVien", ds1);
            }
            else
            {
                if (ds2.Count > 0)
                {
                    return View("NhanVien", ds2);
                }
                return View("NhanVien");
            }
        }
        public ActionResult ThemNV()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNV(NHANVIEN d, FormCollection fc)
        {
            int t = dl.NHANVIENs.Count();
            if (t >= 0 && t < 9)
            {
                d.MANV = "NV00" + (dl.NHANVIENs.Count() + 1).ToString();
            }
            else
            {
                d.MANV = "NV0" + (dl.NHANVIENs.Count() + 1).ToString();
            }
            d.NGAYS = DateTime.Parse(fc["date"]);
            d.CHUCVU = "Nhân Viên";
            dl.NHANVIENs.InsertOnSubmit(d);
            dl.SubmitChanges();
            return RedirectToAction("NhanVien", d);
        }

        public ActionResult SuaNV(string id)
        {
            NHANVIEN t = dl.NHANVIENs.FirstOrDefault(s => s.MANV == id);
            return View(t);
        }

        [HttpPost]

        public ActionResult SuaNV(NHANVIEN t)
        {
            NHANVIEN ft = dl.NHANVIENs.FirstOrDefault(s => s.MANV == t.MANV);
            ft.HOTEN = t.HOTEN;
            ft.DIACHI = t.DIACHI;
            ft.TRANGTHAI = t.TRANGTHAI;

            UpdateModel(ft);
            dl.SubmitChanges();

            return RedirectToAction("NhanVien");
        }
        public ActionResult KhachHang()
        {
            List<KHACHHANG> ds = dl.KHACHHANGs.ToList();
            return View(ds);
        }
        [HttpPost]
        public ActionResult XLTimKiem_KH(FormCollection fc)
        {
            string str = fc["search"];
            List<KHACHHANG> ds1 = dl.KHACHHANGs.Where(t => t.HOTEN.Contains(str)).ToList();
            List<KHACHHANG> ds2 = dl.KHACHHANGs.Where(t => t.MAKH.Contains(str)).ToList();
            if (ds1.Count > 0)
            {
                return View("KHACHHANG", ds1);
            }
            else
            {
                if (ds2.Count > 0)
                {
                    return View("KHACHHANG", ds2);
                }
                return View("KHACHHANG");
            }
        }
        public ActionResult HoaDon()
        {
            List<HOADON> ds = dl.HOADONs.ToList();
            return View(ds);
        }
        [HttpPost]
        public ActionResult XLTimKiem_HD(FormCollection fc)
        {
            string str = fc["search"];
            List<HOADON> ds1 = dl.HOADONs.Where(t => t.KHACHHANG.HOTEN.Contains(str)).ToList();
            if (ds1.Count > 0)
            {
                return View("HoaDon", ds1);
            }
            return View("HoaDon");
        }
        public ActionResult ChiTietHD_MaHoaDon(string id)
        {
            List<CHITIETHOADON> ds = dl.CHITIETHOADONs.Where(t => t.SOHD == id).ToList();
            return PartialView(ds);
        }

        public ActionResult ChiTietKT_MaSP(string id)
        {
            List<CHITIETKICHTHUOC> ds = dl.CHITIETKICHTHUOCs.Where(t => t.MASP == id).ToList();
            return PartialView(ds);
        }

        public ActionResult ChiTietPN_MaPN(string id)
        {
            List<CHITIETPN> ds = dl.CHITIETPNs.Where(t => t.MAPN == id).ToList();
            return PartialView(ds);
        }

        public ActionResult VanChuyen_MaHoaDon(string id)
        {
            List<VANCHUYEN> ds = dl.VANCHUYENs.Where(t => t.SOHD == id).ToList();
            return PartialView(ds);
        }


    }
}
