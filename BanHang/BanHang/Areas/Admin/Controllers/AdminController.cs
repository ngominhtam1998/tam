using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bán_hàng.Areas.Admin.Models;
using BanHang.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bán_hàng.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AdminController : Controller
    {
        public string ChuoiKetNoi = @"Data Source=HIEU-PC\SQLEXPRESS;Initial Catalog=BanHang;
        Integrated Security=True";
        private List<SanPham> DataSanPham = new List<SanPham>();
        [Route("/")]
        public IActionResult ViewLogin()
        {
            return View();
        }

        [HttpGet]
        public ReponsResult Login(string a, string b)
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

                        HttpContext.Session.SetString("Password", Pass);
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
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Password") != null)
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
            return RedirectToAction("ViewLogin");

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
                    MaKH = int.Parse($"{ hhrow["MaKH"] }"),
                    TenKH = $"{ hhrow["TenKH"] }",
                    DiaChi = $"{ hhrow["DiaChi"] }",
                    SDT = int.Parse($"{ hhrow["SDT"] }"),
                    NgayDatHang = $"{hhrow["NgayDatHang"] }",
                });
            }
            return View(donDatHang);
        }
    }
}