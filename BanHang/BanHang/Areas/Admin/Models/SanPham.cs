using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bán_hàng.Areas.Admin.Models
{
    public class SanPham
    {
        [Display(Name = "Mã")]
        public int Masp { get; set; }
        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage ="chua nhap ten san pham")]
        public string Tensp { get; set; }
        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "chua nhap so luong")]
        public int Soluong { get; set; }
        [Display(Name = "Hình")]
        public string Hinh { get; set; }
        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "chua nhap don gia")]
        public int Dongia { get; set; }
        [Display(Name = "Mô tả")]
        public string Mota { get; set; }
    }
}
