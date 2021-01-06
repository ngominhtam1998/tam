using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModelsAdmin
{
    public class DoanhThu
    {
        [Display(Name = "Tên sản phẩm")]
        public string Tensp { get; set; }
        [Display(Name = "Đơn giá")]
        public int Dongia { get; set; }
        [Display(Name = "Số lượng đã bán")]
        public int Soluong { get; set; }
        [Display(Name = "Doanh Thu")]
        public int TongTien { get; set; }
    }
}
