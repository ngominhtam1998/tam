using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanHang.Controllers
{
    [Authorize]
    public class KhachHangController : Controller
    {
        public string ChuoiKetNoi = @"Data Source=HIEU-PC\SQLEXPRESS;Initial Catalog=BanHang;
        Integrated Security=True";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [AllowAnonymous, HttpGet]
        public IActionResult ViewDangNhap()
        {
            
            return View();
        }

        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> DangNhap(string a, string b)
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

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("ViewDangNhap");
                    }

                }

            }
            return View("ViewDangNhap");
        }
    }
}
