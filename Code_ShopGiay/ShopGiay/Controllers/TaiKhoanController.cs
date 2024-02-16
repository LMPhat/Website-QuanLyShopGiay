using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;

namespace ShopGiay.Controllers
{
    public class TaiKhoanController : Controller
    {
        //
        // GET: /TaiKhoan/
        Sa_1DataContext dl = new Sa_1DataContext();
        public ActionResult KhoiTao()
        {
            KHACHHANG kh = (KHACHHANG)Session["kh"];
            if (kh != null)
                ViewBag.chao = "Hello, " + kh.HOTEN;
            return PartialView();
        }
        public ActionResult KhoiTao_AD()
        {
            NHANVIEN ad = (NHANVIEN)Session["ad"];
            if (ad != null)
                ViewBag.chao = "Hello, " + ad.HOTEN;
            return PartialView();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            KHACHHANG kh = dl.KHACHHANGs.FirstOrDefault(t => (t.TAIKHOAN == fc["username"] && t.MATKHAU == fc["password"]));
            if (kh != null)
            {
                Session["kh"] = kh;
                return RedirectToAction("SanPham", "KhachHang");
            }
            else
            {
                NHANVIEN ad = dl.NHANVIENs.FirstOrDefault(t => t.TAIKHOAN == fc["username"] && t.MATKHAU == fc["password"]);
                if (ad != null)
                {
                    Session["ad"] = ad;
                    return RedirectToAction("SanPham", "QuanLy");
                }
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(KHACHHANG d, FormCollection fc)
        {
            int t = dl.KHACHHANGs.Count();
            if (t >= 0 && t < 9)
            {
                d.MAKH = "KH00" + (dl.SANPHAMs.Count() + 1).ToString();
            }
            else
            {
                d.MAKH = "KH0" + (dl.SANPHAMs.Count() + 1).ToString();
            }
            d.HOTEN = fc["name"];
            d.SDT = fc["phone"];
            d.DIACHI = "Chưa xác định";
            d.NGAYS = DateTime.Parse(fc["date"]);
            d.TAIKHOAN = fc["username"];
            d.MATKHAU = fc["password"];
            dl.KHACHHANGs.InsertOnSubmit(d);
            dl.SubmitChanges();
            return RedirectToAction("Login", "TaiKhoan");
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "TaiKhoan");
        }
    }
}
