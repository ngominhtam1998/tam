using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.Models
{
    public class TaiKhoanKhachHang
    {
        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage ="Tên đăng nhập không được bỏ trống!")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Chưa nhập mật khẩu!")][DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
