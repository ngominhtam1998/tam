using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModels
{
    public class DonDatHang
    {
        public int MaHD { get; set; }
        [Display(Name ="Mã khách hàng")]
        public int MaKH { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string TenKH { get; set; }
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Số điện thoại")]
        public int SDT { get; set; }
        [Display(Name = "Ngày đặt hàng")]
        public string NgayDatHang { get; set; }
    }
}
