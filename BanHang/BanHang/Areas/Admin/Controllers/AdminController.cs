﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bán_hàng.Areas.Admin.Models;
using BanHang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Bán_hàng.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        public string ChuoiKetNoi = @"Data Source=HIEU-PC\SQLEXPRESS;Initial Catalog=BanHang;
        Integrated Security=True";
        private List<SanPham> DataSanPham = new List<SanPham>();

        [AllowAnonymous]
        public IActionResult ViewLogin()
        {
            return View();
        }

        [AllowAnonymous, HttpPost]
        public async Task<ReponsResult> Login(string a, string b)
        {
            if ((a != null) && (b != null))
            {
                SqlConnection Connection = new SqlConnection(ChuoiKetNoi);
                string sql = $"select * from Admin where AdminName = '{a}' ";
                SqlDataAdapter da = new SqlDataAdapter(sql, Connection);
                DataTable dtAdmin = new DataTable();
                da.Fill(dtAdmin);

                //xu ly
                if (dtAdmin.Rows.Count > 0)
                {
                    String Name = $"{dtAdmin.Rows[0]["AdminName"]}";
                    String Pass = $"{dtAdmin.Rows[0]["Password"]}";
                    if (b == Pass)
                    {

                        var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, a),
                        new Claim("Password", b),
                        new Claim(ClaimTypes.Role, "Admin"),

                    };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);

                        return new ReponsResult(true, "Ok", "Ngon lanh");
                    }
                    else
                    {
                        return new ReponsResult(false, "Fail", "Sai Thông tin đăng nhập!");
                    }

                }


                //foreach (DataRow hhrow in dtAdmin.Rows)
                //{
                //    String Name = $"{ hhrow["AdminName"] }";
                //    String Pass = $"{ hhrow["Password"] }";
                //    if (b == Pass)
                //    {
                //        return new ReponsResult(true, "Ok", "Ngon lanh");
                //    }
                //    else
                //    {
                //        return new ReponsResult(false, "Fail", "Sai Thông tin đăng nhập!");
                //    }
                //}
            }

            return new ReponsResult(false, "Fail", "Sai Thông tin đăng nhập!");
        }
        public IActionResult Index()
        {
            var sql = "select * from SanPham";

            foreach (DataRow hhrow in Sql.GetDataTable(sql).Rows)
            {
                DataSanPham.Add(new SanPham
                {
                    Masp = int.Parse($"{ hhrow["Masp"] }"),
                    Tensp = $"{ hhrow["Tensp"] }",
                    Dongia = int.Parse($"{ hhrow["Dongia"] }"),
                    Soluong = int.Parse($"{ hhrow["Soluong"] }"),
                    Hinh = $"{ hhrow["Hinh"] }",
                });
            }
            return View(DataSanPham);



        }
        // Thêm sản phẩm
        public IActionResult ViewCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SanPham _sanpham, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                if (Hinh != null)
                {
                    var saveImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ADMIN", "Images", Hinh.FileName);
                    using (var file = new FileStream(saveImage, FileMode.Create))
                    {
                        Hinh.CopyTo(file);
                    }
                    _sanpham.Hinh = Hinh.FileName;
                    var sql = $"insert into SanPham(Tensp,Dongia,Soluong,Hinh) values('{_sanpham.Tensp}','{_sanpham.Dongia}','{_sanpham.Soluong}','{_sanpham.Hinh}')";
                    SqlConnection Connection = new SqlConnection(ChuoiKetNoi);
                    SqlCommand cmd = new SqlCommand(sql, Connection);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
            return View("ViewCreate");
        }

        // sua  san pham
        public IActionResult ViewEdit(int MaSp)
        {
            var sql = "select * from SanPham where Masp = " + MaSp.ToString();

            List<SanPham> _DataSanPham = new List<SanPham>();

            foreach (DataRow hhrow in Sql.GetDataTable(sql).Rows)
            {
                _DataSanPham.Add(new SanPham
                {
                    Masp = int.Parse($"{ hhrow["Masp"] }"),
                    Tensp = $"{ hhrow["Tensp"] }",
                    Dongia = int.Parse($"{ hhrow["Dongia"] }"),
                    Soluong = int.Parse($"{ hhrow["Soluong"] }"),
                    Hinh = $"{ hhrow["Hinh"] }",
                });
            }

            //var sp = DataSanPham.SingleOrDefault(p => p.Masp == MaSp);
            if (_DataSanPham != null)
            {
                return View(_DataSanPham[0]);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(SanPham _sanpham)
        {

            //var sp = DataSanPham.SingleOrDefault(p => p.Masp == _sanpham.Masp);
            //if (sp != null)
            //{
            try
            {
                var sql = $"update SanPham set Tensp = '{_sanpham.Tensp}', Dongia = '{_sanpham.Dongia}',Soluong = '{_sanpham.Soluong}',Hinh = '{_sanpham.Hinh}' where Masp = {_sanpham.Masp}";
                var Con = new SqlConnection(ChuoiKetNoi);
                var cmd = new SqlCommand(sql, Con);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                //------------------------//
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("ViewEdit", _sanpham.Masp);

            }


        }

        // xoa san pham

        public IActionResult Delete(int MaSp)
        {
            var sql = $"Delete from SanPham where Masp = {MaSp}";
            var Con = new SqlConnection(ChuoiKetNoi);
            var cmd = new SqlCommand(sql, Con);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return RedirectToAction("Index");
        }

        //Don dat hang
        public IActionResult ViewDonDatHang()
        {
            List<DonDatHang> donDatHang = new List<DonDatHang>();
            var sql = "select * from HoaDon";

            foreach (DataRow hhrow in Sql.GetDataTable(sql).Rows)
            {
                donDatHang.Add(new DonDatHang
                {
                    MaHD = int.Parse($"{ hhrow["MaHD"] }"),
                    MaKH = int.Parse($"{ hhrow["MaKH"] }"),
                    TenKH = $"{ hhrow["TenKH"] }",
                    DiaChi = $"{ hhrow["DiaChi"] }",
                    SDT = int.Parse($"{ hhrow["SDT"] }"),
                    NgayDatHang = $"{hhrow["NgayDatHang"] }",
                });
            }
            return View(donDatHang);
        }
        //Doanh thu
        public IActionResult ViewDoanhThu()
        {
            var doanhthu = new List<DoanhThu>();
            string sql = "select masp, sum(soluong) as soluong from doanhthu group by masp";
            foreach (DataRow hhrow in Sql.GetDataTable(sql).Rows)
            {
                string sql2 = $"select tensp, dongia from sanpham where masp = '{hhrow["Masp"]}'";
                string tensp = $"{Sql.GetDataTable(sql2).Rows[0]["Tensp"]}";
                int dongia = int.Parse($"{Sql.GetDataTable(sql2).Rows[0]["dongia"]}");
                doanhthu.Add(new DoanhThu
                {
                    Tensp = tensp,
                    Dongia = dongia,
                    Soluong = int.Parse($"{hhrow["soluong"]}"),
                    TongTien = dongia * int.Parse($"{hhrow["soluong"]}")

                });
            }
            return View(doanhthu);
        }

        public IActionResult DoanhThu(int ID)
        {
            //ADD Doanh Thu
            var _Sql = $"select * from HoaDonCT where MaHD = {ID}";
            foreach (DataRow hhrow in Sql.GetDataTable(_Sql).Rows)
            {
                if (Sql.GetDataTable(_Sql).Rows.Count > 0)
                {
                    var Masp = int.Parse($"{ hhrow["MaSP"] }");
                    var Dongia = int.Parse($"{ hhrow["DonGia"] }");
                    var Soluong = int.Parse($"{ hhrow["SoLuong"] }");
                    var Sql2 = $"insert into DoanhThu(MaSP,SoLuong,DonGia) values('{Masp}','{Soluong}','{Dongia}')";
                    SqlConnection Connect2 = new SqlConnection(ChuoiKetNoi);
                    SqlCommand cmd2 = new SqlCommand(Sql2, Connect2);
                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();
                }
            }
            // Xoa HDCT
            var Sql4 = $"delete from HoaDonCT where MaHD = {ID}";
            SqlConnection Connect4 = new SqlConnection(ChuoiKetNoi);
            SqlCommand cmd4 = new SqlCommand(Sql4, Connect4);
            cmd4.Connection.Open();
            cmd4.ExecuteNonQuery();
            cmd4.Connection.Close();
            //Xoa Hoa Don 
            var Sql3 = $"delete from HoaDon where MaHD = {ID}";
            SqlConnection Connect3 = new SqlConnection(ChuoiKetNoi);
            SqlCommand cmd3 = new SqlCommand(Sql3, Connect3);
            cmd3.Connection.Open();
            cmd3.ExecuteNonQuery();
            cmd3.Connection.Close();
            return RedirectToAction("ViewDonDatHang");

        }
    }
}