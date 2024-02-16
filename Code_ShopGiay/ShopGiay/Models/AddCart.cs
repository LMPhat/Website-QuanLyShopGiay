using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
namespace ShopGiay.Models
{
    public class CheckBrandName : ValidationAttribute
    {
        public string IDProduct { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo ID = validationContext.ObjectType.GetProperty(IDProduct);
            var id = ID.GetValue(validationContext.ObjectInstance).ToString();
            Sa_1DataContext DB = new Sa_1DataContext();
            SANPHAM SP = DB.SANPHAMs.Where(P => P.MASP == id).FirstOrDefault();
            if (Int16.Parse(value.ToString()) <= 0)
                return new ValidationResult("Please choose quantity beyond 0!");
            else
                if (Int16.Parse(value.ToString()) > SP.GIABAN)
                    return new ValidationResult("Please choose quantity below the limit of " + SP.GIABAN.ToString() + " !");
                else
                    return ValidationResult.Success;
        }
    }
    public class AddCart
    {
        public string MaSP { get; set; }
        [Required(ErrorMessage = "Please choose quantity beyond 0!")]
        public int SoLuong { get; set; }
        public string MaKH { get; set; }
        [Required(ErrorMessage = "Please choose your size!")]

        public int Size { get; set; }
    }
}