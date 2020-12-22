﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bán_hàng.Areas.Admin.Models;
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
        [Route("/")]
        public IActionResult Index()
        {

            var sql = "select * from SanPham";

            //if (Sql.GetDataTable(sql).Rows.Count > 0)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        DataSanPham.Add(new SanPham
            //        {
            //            Tensp = $"{ Sql.GetDataTable(sql).Rows[i]["Tensp"] }",
            //            Dongia = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["Dongia"] }"),
            //            Hinh = $"{Sql.GetDataTable(sql).Rows[i]["Hinh"] }",
            //            Soluong = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["SoLuong"] }")
            //        });
            //    }

            //}
            ViewBag.TongSoTrang = Math.Ceiling(1.0*Sql.GetDataTable(sql).Rows.Count / 4);
            return View(DataSanPham);
        }

        // TaiThem
        [HttpPost]
        public IActionResult LoadMore(int page)
        {
            var sql = "select * from SanPham";
            try
            {
                if (Sql.GetDataTable(sql).Rows.Count > 0)
                {
                    for (int i = (page - 1) * 4; i < page * 4; i++)
                    {
                        DataSanPham.Add(new SanPham
                        {
                            Tensp = $"{ Sql.GetDataTable(sql).Rows[i]["Tensp"] }",
                            Dongia = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["Dongia"] }"),
                            Hinh = $"{Sql.GetDataTable(sql).Rows[i]["Hinh"] }",
                            Soluong = int.Parse($"{ Sql.GetDataTable(sql).Rows[i]["SoLuong"] }")
                        });
                    }

                }
            }
            catch (Exception)
            {
                DataSanPham.Clear();
                if (Sql.GetDataTable(sql).Rows.Count > 0)
                {
                    for (int i = (page - 1) * 4; i < 11; i++)
                    {
                        DataSanPham.Add(new SanPham
                        {
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
        //  gio hang 
        public IActionResult Items(int ID)
        {
            HttpContext.Session.SetInt32($"{ID}", ID);
            return RedirectToAction("Index");
        }
        //View Gio Hang
        public IActionResult Shopping()
        {
            List<string> _lstSessionId = HttpContext.Session.Keys.ToList();
            foreach (string _sId in _lstSessionId)
            {

                if (_sId != null)
                {
                    var sql2 = $"select * from SanPham where Masp = '{_sId}'";

                    if (Sql.GetDataTable(sql2).Rows.Count > 0)
                    {
                        DataSanPham.Add(new SanPham
                        {

                            Tensp = $"{ Sql.GetDataTable(sql2).Rows[0]["Tensp"] }",
                            Dongia = int.Parse($"{ Sql.GetDataTable(sql2).Rows[0]["Dongia"] }"),
                            Hinh = $"{Sql.GetDataTable(sql2).Rows[0]["Hinh"] }",
                        });
                    }
                }
            }
            return View(DataSanPham);
        }
        ///////////////////mua////////////
        public IActionResult ViewBuy()
        {
            int TongTien = 0;
            List<string> _lstSessionId = HttpContext.Session.Keys.ToList();
            foreach (string _sId in _lstSessionId)
            {
                int? ID = Convert.ToInt32($"{_sId}");
                if (ID != null)
                {
                    var sql2 = $"select DonGia from SanPham where Masp = {ID}";

                    if (Sql.GetDataTable(sql2).Rows.Count > 0)
                    {
                        TongTien = TongTien + int.Parse($"{ Sql.GetDataTable(sql2).Rows[0]["DonGia"] }");
                    }
                }
            }
            ViewBag.TongTien = TongTien;
            HttpContext.Session.SetInt32("TongTien", TongTien);
            return View();
        }
        //////Them khach hang///
        public IActionResult AddKH(KhachHang KH)
        {
            if (ModelState.IsValid)
            {
                string Sql = $"insert into KhachHang(TenKH,SDT,Email) values('{KH.TenKH}','{KH.SDT}','{KH.Email}')";
                SqlConnection Connect = new SqlConnection(ChuoiKetNoi);
                SqlCommand cmd = new SqlCommand(Sql, Connect);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                ViewBag.Success = "Dat hang thanh cong!";
                ViewBag.TongTien = HttpContext.Session.GetInt32("TongTien");
            }
            return View("ViewBuy");
        }
        // SEACH//
        [HttpPost]
        public IActionResult Search(string Keyword)
        {   //LinQ
            //var data = new List<SanPham>();
            //var sql = "select * from SanPham";

            //foreach (DataRow hhrow in Sql.GetDataTable(sql).Rows)
            //{
            //    DataSanPham.Add(new SanPham
            //    {
            //        Masp = int.Parse($"{ hhrow["Masp"] }"),
            //        Tensp = $"{ hhrow["Tensp"] }",
            //        Dongia = int.Parse($"{ hhrow["Dongia"] }"),
            //        Soluong = int.Parse($"{ hhrow["Soluong"] }"),
            //        Hinh = $"{ hhrow["Hinh"] }",
            //    });
            //}
            //if (!string.IsNullOrEmpty(Keyword))
            //{
            //    data = DataSanPham.Where(hh => hh.Tensp.Contains(Keyword)).ToList();
            //}

            //var dsHangHoa = data.Select(hh => new SanPham
            //{
            //    Tensp = hh.Tensp,
            //    Masp = hh.Masp,
            //    Dongia = hh.Dongia,
            //    Hinh = hh.Hinh
            //});

            //Query
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
            return RedirectToAction("Index");
        }
    }
}

