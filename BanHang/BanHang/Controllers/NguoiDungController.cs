﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bán_hàng.Areas.Admin.Models;
using BanHang.Models;
using BanHang.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BanHang.Controllers
{
    public class NguoiDungController : Controller
    {
        public string ChuoiKetNoi = @"Data Source=HIEU-PC\SQLEXPRESS;Initial Catalog=BanHang;
        Integrated Security=True";
        private List<SanPham> DataSanPham = new List<SanPham>();
        public IActionResult Index()
        {

            var sql = "select * from SanPham";
            ViewBag.TongSoTrang = Math.Ceiling(1.0 * Sql.GetDataTable(sql).Rows.Count / 4);
            return View(DataSanPham);
        }
        // login nguoi dung
        public IActionResult ViewLogin()
        {
            return View();
        }

        //dang ki nguoi dung
        public async Task<IActionResult> Login(string Username, string Password)
        {
            if ((Username != null) && (Password != null))
            {
                string sql = $"select * from Account where UserName = '{Username}' ";
                if (Sql.GetDataTable(sql).Rows.Count > 0)
                {
                    String Name = $"{Sql.GetDataTable(sql).Rows[0]["UserName"]}";
                    String Pass = $"{Sql.GetDataTable(sql).Rows[0]["Password"]}";
                    if (Password == Pass)
                    {

                        var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, Username),
                        new Claim("Password", Password),
                        new Claim(ClaimTypes.Role, "User")

                    };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Thongbao = "Sai thông tin đăng nhập!";
                        return View("ViewLogin");
                    }

                }
                else
                {
                    ViewBag.Thongbao = "Sai thông tin đăng nhập!";
                    return View("ViewLogin");
                }
              
            }

            return View("ViewLogin");
        }
        public IActionResult ViewDangKy()
        {
            return View();
        }
      

        // TaiThem
        [HttpPost]
        public IActionResult LoadMore(int page)
        {
            var sql = "select * from SanPham";
            if (Sql.GetDataTable(sql).Rows.Count > 0)
            {
                if ((page * 4) <= Sql.GetDataTable(sql).Rows.Count)
                {
                    for (int i = (page - 1) * 4; i < page * 4; i++)
                    {
                        DataSanPham.Add(new SanPham
                        {
                            Masp = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["Masp"] }"),
                            Tensp = $"{ Sql.GetDataTable(sql).Rows[i]["Tensp"] }",
                            Dongia = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["Dongia"] }"),
                            Hinh = $"{Sql.GetDataTable(sql).Rows[i]["Hinh"] }",
                            Soluong = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["SoLuong"] }")
                        });
                    }
                }
                else
                {
                    for (int i = (page - 1) * 4; i < Sql.GetDataTable(sql).Rows.Count; i++)
                    {
                        DataSanPham.Add(new SanPham
                        {
                            Masp = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["Masp"] }"),
                            Tensp = $"{ Sql.GetDataTable(sql).Rows[i]["Tensp"] }",
                            Dongia = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["Dongia"] }"),
                            Hinh = $"{Sql.GetDataTable(sql).Rows[i]["Hinh"] }",
                            Soluong = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["SoLuong"] }")
                        });
                    }
                }

            }

            return PartialView("PartialSanPhamSeach", DataSanPham);
        }
        public IActionResult Items(int ID)
        {
            //Kiểm tra đã khởi tạo session ?
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("SessionShoping")))
            {
                //Get chuỗi session -> Parse thành danh sách 
                try
                {
                    List<SessionShoping> lstSessionShopings = JsonConvert.DeserializeObject<List<SessionShoping>>(HttpContext.Session.GetString("SessionShoping"));
                    //kiểm tra Id sp đã có trong danh sách ? 
                    if (lstSessionShopings.FindIndex(x => x.Masp == ID) != -1)
                    // có rồi -> +1 số lượng
                    {
                        lstSessionShopings[lstSessionShopings.FindIndex(x => x.Masp == ID)].Soluong += 1;
                    }
                    //chưa có -> thêm sản phẩm
                    else
                    {
                        string sql = $"select Tensp, Dongia, Hinh from SanPham where masp = {ID}";
                        string tensp = $"{Sql.GetDataTable(sql).Rows[0]["Tensp"]}";
                        string Hinh = $"{Sql.GetDataTable(sql).Rows[0]["Hinh"]}";
                        int Dongia = int.Parse($"{Sql.GetDataTable(sql).Rows[0]["Dongia"]}");
                        lstSessionShopings.Add(new SessionShoping(ID, tensp, 1, Dongia, Hinh));
                    }

                    // update session sau khi chỉnh sửa
                    HttpContext.Session.SetString("SessionShoping", JsonConvert.SerializeObject(lstSessionShopings));
                }
                catch (Exception)
                {
                    return Content("Lỗi");
                }

            }
            else
            {
                try
                {
                    string sql = $"select Tensp, Dongia, Hinh from SanPham where masp = {ID}";
                    string tensp = $"{Sql.GetDataTable(sql).Rows[0]["Tensp"]}";
                    string Hinh = $"{Sql.GetDataTable(sql).Rows[0]["Hinh"]}";
                    int Dongia = int.Parse($"{Sql.GetDataTable(sql).Rows[0]["Dongia"]}");
                    List<SessionShoping> lstSessionShopings = new List<SessionShoping>();
                    lstSessionShopings.Add(new SessionShoping(ID, tensp, 1, Dongia, Hinh));

                    //Parse danh sách trên thành chuỗi -> lưu vào session
                    HttpContext.Session.SetString("SessionShoping", JsonConvert.SerializeObject(lstSessionShopings));
                }
                catch (Exception)
                {
                    return Content("Lỗi");
                }
            }

            return RedirectToAction("Index");
        }

        //View Gio Hang
        public IActionResult Shopping(int? ID)
        {

            if (HttpContext.Session.GetString("SessionShoping") != null)
            {
                List<SessionShoping> lstSessionShopings = JsonConvert.DeserializeObject<List<SessionShoping>>(HttpContext.Session.GetString("SessionShoping"));
                if (ID.HasValue)
                {
                    var delete = lstSessionShopings.SingleOrDefault(p => p.Masp == ID);
                    lstSessionShopings.Remove(delete);
                    HttpContext.Session.SetString("SessionShoping", JsonConvert.SerializeObject(lstSessionShopings));
                }
                return View(lstSessionShopings);
            }
            List<SessionShoping> lstSessionShoping = new List<SessionShoping>();
            return View(lstSessionShoping);
        }
        ///////////////////mua////////////
        public IActionResult ViewBuy()
        {
            int TongTien = 0;
            List<SessionShoping> lstSessionShopings = JsonConvert.DeserializeObject<List<SessionShoping>>(HttpContext.Session.GetString("SessionShoping"));
            foreach (SessionShoping item in lstSessionShopings)
            {
                TongTien += item.Dongia * item.Soluong;
            }
            ViewBag.TongTien = TongTien;
            TempData["TongTien"] = ViewBag.TongTien;
            return View();
        }
        //////Them khach hang///
        public IActionResult AddKH(KhachHang KH)
        {
            if (ModelState.IsValid)
            {
                string _Sql = $"insert into KhachHang(TenKH,DiaChi,SDT,Email) values('{KH.TenKH}','{KH.DiaChi}','{KH.SDT}','{KH.Email}')";
                SqlConnection Connect = new SqlConnection(ChuoiKetNoi);
                SqlCommand cmd = new SqlCommand(_Sql, Connect);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                //ADD HoaDon
                var Sql1 = $"select MaKH from KhachHang where TenKH = '{KH.TenKH}'";
                var MaKH = int.Parse($"{ Sql.GetDataTable(Sql1).Rows[0]["MaKH"] }");
                var NgayDatHang = DateTime.Now.ToString();
                string Sql2 = $"insert into HoaDon(MaKH,TenKH,DiaChi,SDT,NgayDatHang) values('{MaKH}','{KH.TenKH}','{KH.DiaChi}','{KH.SDT}','{NgayDatHang}')";
                SqlCommand cmd2 = new SqlCommand(Sql2, Connect);
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();

                //ADD HoaDonCT va trừ soluong tồn kho
                string Sql3 = $"select MaHD from HoaDon where MaKH = {MaKH}";
                var MaHD = int.Parse($"{ Sql.GetDataTable(Sql3).Rows[0]["MaHD"] }");
                
                List<SessionShoping> lstSessionShopings = JsonConvert.DeserializeObject<List<SessionShoping>>(HttpContext.Session.GetString("SessionShoping"));
                foreach (SessionShoping item in lstSessionShopings)
                {
                    string Sql4 = $"select soluong from sanpham where masp = {item.Masp}";
                    var soluongtonkho = int.Parse($"{Sql.GetDataTable(Sql4).Rows[0]["Soluong"]}");
                    var updatesoluong = soluongtonkho - item.Soluong;
                    string SqlUpdateSoluong = $"update sanpham set soluong = {updatesoluong} where masp = {item.Masp}";
                    SqlCommand cmd3 = new SqlCommand(SqlUpdateSoluong, Connect);
                    cmd3.Connection.Open();
                    cmd3.ExecuteNonQuery();
                    cmd3.Connection.Close();
                    var Sql5 = $"insert into HoaDonCT(MaSP,MaHD,SoLuong,DonGia) values('{item.Masp}','{MaHD}','{item.Soluong}','{item.Dongia}')";
                    SqlCommand cmd5 = new SqlCommand(Sql5, Connect);
                    cmd5.Connection.Open();
                    cmd5.ExecuteNonQuery();
                    cmd5.Connection.Close();
                }
                ViewBag.Success = "Dat hang thanh cong!";
                ViewBag.Return = "True";
                ViewBag.TongTien = TempData["TongTien"];
            }

            return View("ViewBuy");
        }
        // SEACH//
        [HttpPost]
        public IActionResult Search(string Keyword)
        {
            if (!string.IsNullOrEmpty(Keyword))
            {
                string sql = $"select * from  SanPham where Tensp like N'%{Keyword}%'";

                foreach (DataRow hhrow in Sql.GetDataTable(sql).Rows)
                {
                    DataSanPham.Add(new SanPham
                    {

                        Tensp = $"{ hhrow["Tensp"] }",
                        Dongia = int.Parse($"{ hhrow["Dongia"] }"),
                        Soluong = int.Parse($"{ hhrow["Soluong"] }"),
                        Hinh = $"{ hhrow["Hinh"] }",
                    });

                }
                return PartialView("PartialSanPhamSeach", DataSanPham);
            }
            return Content("false");
        }
    }
}

