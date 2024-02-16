using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopGiay.Models
{
    public class Cart
    {
        public string MAHD { get; set; }
        public string MASP { get; set; }
        public string tenSP { get; set; }
        public string hinhanh { get; set; }
        public double gia { get; set; }
        public int soluong { get; set; }
        public int size { get; set;}
        public Cart()
        {
            
        }
        public Cart(string MAHD,string MASP,string tenSP, string hinhanh,double gia,int soluong,int size)
        {
            this.MAHD = MAHD;
            this.MASP = MASP;
            this.tenSP = tenSP;
            this.hinhanh = hinhanh;
            this.gia = gia;
            this.soluong = soluong;
            this.size = size;
        }
    }
}