using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Taekwondo.Models;
using System.Data;

namespace Taekwondo.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        void ConnectionString()
        {
            con.ConnectionString = "data source=DESKTOP-3LCV8U4; database=Taekwondo DB; integrated security = SSPI; ";
        }
        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            DataSet ds = new DataSet();
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Login where username='"+ acc.Name+"' and password = '"+acc.Password+"'";
            dr = com.ExecuteReader();
            //SqlDataAdapter da = new SqlDataAdapter(com);
            //da.Fill(ds);
            //con.Close();
            if (dr.HasRows)
            {
                con.Close();
                return Redirect("~/Home/Index");
            }
            else
            {
                con.Close();
                return View("Login");
            }


        }
    }
}