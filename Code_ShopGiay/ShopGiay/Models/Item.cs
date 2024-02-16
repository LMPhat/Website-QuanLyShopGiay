using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopGiay.Models
{
    public class QL_Size
    {
        Sa_1DataContext dl = new Sa_1DataContext();
        public int size { get; set; }
        public int sl { get; set; }
        public QL_Size()
        { }
        public QL_Size(int size, int sl)
        {
            this.size = size;
            this.sl = sl;
        }
        
        public List<QL_Size> LayDanhSachKichThuocVaSoLuong(string maSanPham)
        {
            List<QL_Size> dsKichThuoc = new List<QL_Size>();
            var chiTietKichThuoc = (from c in dl.CHITIETKICHTHUOCs
                                    where c.MASP == maSanPham
                                    select c);
            foreach (var ctkt in chiTietKichThuoc)
            {
                KICHTHUOC kichThuoc = dl.KICHTHUOCs.FirstOrDefault(k => k.MAKICHTHUOC == ctkt.MAKICHTHUOC);

                if (kichThuoc != null)
                {
                    dsKichThuoc.Add(new QL_Size
                    {
                        size = kichThuoc.KICHTHUOC1 ?? 0,
                        sl = ctkt.SOLUONG ?? 0
                    });
                }
            }
            return dsKichThuoc;
        }
        

    }
    public class CTPN
    {
        public string MASP { get; set; }
        public string tenSP { get; set; }
        public string tenLoai { get; set; }
        public double giaNhap { get; set; }
        public int soluong { get; set; }
        public int size { get; set; }
        public List<QL_Size> DSSize { get; set; }

        public CTPN(string MASP, string tenSP, string tenLoai, double giaNhap, int soluong, int size)
        {
            this.MASP = MASP;
            this.tenSP = tenSP;
            this.tenLoai = tenLoai;
            this.giaNhap = giaNhap;
            this.soluong = soluong;
            this.size = size;
        }
        public CTPN(string MASP, string tenSP, string tenLoai, int soluong, int size)
        {
            this.MASP = MASP;
            this.tenSP = tenSP;
            this.tenLoai = tenLoai;
            this.soluong = soluong;
            this.size = size;
            
        }

        public CTPN(string MASP, string tenSP, string tenLoai)
        {
            this.MASP = MASP;
            this.tenSP = tenSP;
            this.tenLoai = tenLoai;
        }
    }
    public class Item1
    {
        Sa_1DataContext dl = new Sa_1DataContext();
        public string maSP { get; set; }
        public string tenSP { get; set; }
        public string anhBia { get; set; }
        public float donGia { get; set; }
        public int soLuong { get; set; }
        public int Size { get; set; }
        public List<QL_Size> DSSize { get; set; }

        public float thanhTien { get { return soLuong * donGia; } }

        
        public Item1(string ms)
        {
            maSP = ms;
            SANPHAM s = dl.SANPHAMs.FirstOrDefault(t => t.MASP == ms);
            tenSP = s.TENSP;
            anhBia = s.HINHANH;
            donGia = float.Parse(s.GIABAN.ToString());
            Size = 0;

            soLuong = 1;
        }
        public Item1(string ms, int sl, int size)
        {
            maSP = ms;
            SANPHAM s = dl.SANPHAMs.FirstOrDefault(t => t.MASP == ms);
            tenSP = s.TENSP;
            anhBia = s.HINHANH;
            donGia = float.Parse(s.GIABAN.ToString());
            Size = size;
            QL_Size ql = new QL_Size();
            DSSize = ql.LayDanhSachKichThuocVaSoLuong(ms);
            soLuong = sl;
        }

    }

    public class GioHang
    {
        public List<Item1> dssp;
        public GioHang()
        {
            dssp = new List<Item1>();
        }
        public void Them(Item1 x)
        {
            dssp.Add(x);
        }
        public int SLMatHang()
        {
            return dssp.Count();
        }
        public int TongSL()
        {
            return dssp.Sum(t => t.soLuong);
        }
        public float TongTien()
        {
            return dssp.Sum(t => t.thanhTien);
        }
        public int Xoa(string ma)
        {
            Item1 sp = dssp.Find(n => n.maSP == ma);
            if (sp != null)
            {
                dssp.Remove(sp);
                return 1;
            }
            return -1;

        }
        public int XoaAll()
        {
            if (dssp != null)
            {
                dssp.Clear();
                return 1;
            }
            return -1;

        }
    }
}