using System;

namespace BanHang.Models
{
    public class SessionShoping
    {
        public int Masp { get; set; }
        public string Tensp { get; set; }
        public int Soluong { get; set; }
        public string Hinh { get; set; }
        public int Dongia { get; set; }

      
        public SessionShoping()
        {
            this.Masp = 0;
            this.Tensp = "";
            this.Soluong = 0;
            this.Dongia = 0;
            this.Hinh = "";
        }

        public SessionShoping(int Masp, string Tensp, int Soluong, int Dongia, string Hinh)
        {
            this.Masp = Masp;
            this.Tensp = Tensp;
            this.Soluong = Soluong;
            this.Dongia = Dongia;
            this.Hinh = Hinh;
        }
    }
}
