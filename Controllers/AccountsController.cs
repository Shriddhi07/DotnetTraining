﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruYum_APP.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TruYum_APP.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLogin lg)
        {
            //string constr = ConfigurationManager.ConnectionStrings["TruYumconn"].ConnectionString;   
            string constr = ConfigurationManager.AppSettings["dbconn"].ToString();   
            using(SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.CommandText = $"select * from UserMaster where userid = '{lg.UId}' and password = '{lg. Pwd}'";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //TempData["msg"] = "Login Successfull!";
                    Session["userid"] = dt.Rows[0]["UserId"].ToString();
                    Session["fullname"] = dt.Rows[0]["FullName"].ToString();
                    Session["role"] = dt.Rows[0]["RoleId"].ToString();
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.Clear();
                    TempData["msg"] = "Incorrect UserId/Password";
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Accounts");
        }
    }
}