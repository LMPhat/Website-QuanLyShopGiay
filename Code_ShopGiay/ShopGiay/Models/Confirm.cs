using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ShopGiay.Models
{
    public class Confirm
    {

        public string MAHD { get; set; }
        public string MAKH { get; set; }
        public string MAVC { get; set; }
        public DateTime NGAYGIAO { get; set; }
        public double TongTien { get; set; }
        [Required(ErrorMessage="Vui lòng chọn phương thức thanh toán!")]
        public string Payment { get; set; }
    }
}