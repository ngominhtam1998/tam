using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bán_hàng.Areas.Admin.Models
{
    public class KhachHang
    {
        [Display(Name ="Nhập họ tên")]
        [Required(ErrorMessage ="Vui lòng Nhập tên")]
        public string TenKH { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng Nhập số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string SDT { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
