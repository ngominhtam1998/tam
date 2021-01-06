using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModelsAdmin
{
    public class LSGiaoDich
    {
        [Display(Name ="Tên khách hàng")]
        public string TenKH { get; set; }
        [Display(Name = "Ngày mua hàng")]
        public string NgayThanhToan { get; set; }
        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
        [Display(Name = "Tiền thanh toán")]
        public int TienThanhToan { get; set; }
    }
}
