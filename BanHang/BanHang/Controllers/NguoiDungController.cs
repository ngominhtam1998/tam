using System;
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
        //  gio hang 
        const int count = 1;
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
            if (!string.IsNullOrEmpty(Keyword))
            {
               var data = DataSanPham.Where(hh => hh.Tensp.Contains(Keyword));
            }

            var dsHangHoa = DataSanPham.Select(hh => new SanPham
            {
                Tensp = hh.Tensp,
                Masp = hh.Masp,
                Dongia = hh.Dongia,
                Hinh = hh.Hinh
            });

            return PartialView("PartialSanPhamSeach", dsHangHoa);
        }

    }
}

