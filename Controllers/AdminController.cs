using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using TruYum_APP.Models;
using System.Runtime.Remoting.Messaging;

namespace TruYum_APP.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            string constr = ConfigurationManager.AppSettings["dbconn"].ToString();
            List<MenuItems> menuitems = new List<MenuItems>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;
                cmd.CommandText = $"select * from MenuItems";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        MenuItems menu = new MenuItems()
                        {
                            Id = int.Parse(dr["MenuId"].ToString()),
                            Name = dr["name"].ToString(),
                            Category = dr["category"].ToString(),
                            Price = double.Parse(dr["price"].ToString()),
                            IsActive = bool.Parse(dr["IsActive"].ToString()),
                            FreeDelivery = bool.Parse(dr["FreeDelivery"].ToString()),
                            CreatedOn = DateTime.Parse(dr["CreatedOn"].ToString())
                        };
                        menuitems.Add(menu);
                    }
                }
            }
            return View(menuitems);
        }
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                string constr = ConfigurationManager.AppSettings["dbconn"].ToString();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.CommandText = $"delete from MenuItems where Menuid = {id}";
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult Add()
        {
            ViewBag.categories = new List<SelectListItem>()
            {
                new SelectListItem{Text ="Desserst", Value="Dessert"},
                new SelectListItem{Text ="Main Course", Value="Main Course"},
                new SelectListItem{Text ="Starters", Value="Starters"},
                new SelectListItem{Text ="Beverages", Value="Beverages"},
            };
            return View();
        }
        [HttpPost]
        public ActionResult Add(MenuItems menu)
        {
            int isactive = menu.IsActive ? 1 : 0;
            int freedelivery = menu.FreeDelivery ? 1 : 0;
            string constr = ConfigurationManager.AppSettings["dbconn"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.CommandText = $"insert into MenuItems(name,Category,Price,IsActive,FreeDelivery,CreatedOn)" + $"values('{menu.Name}','{menu.Category}','{menu.Price}','{isactive}','{freedelivery}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    return RedirectToAction("Index", "Admin");
                }
                con.Close();
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            ViewBag.categories = new List<SelectListItem>()
            {
                new SelectListItem{Text ="Desserts", Value="Dessert"},
                new SelectListItem{Text ="Main Course", Value="Main Course"},
                new SelectListItem{Text ="Starters", Value="Starters"},
                new SelectListItem{Text ="Beverages", Value="Beverages"},
            };
            MenuItems menu = new MenuItems();
            if (id > 0)
            {
                string constr = ConfigurationManager.AppSettings["dbconn"].ToString();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.CommandText = $"select * from MenuItems where MenuId ={id} ";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        menu.Id = int.Parse(dt.Rows[0]["MenuId"].ToString());
                        menu.Name = dt.Rows[0]["Name"].ToString();
                        menu.Category = dt.Rows[0]["Category"].ToString();
                        menu.Price = double.Parse(dt.Rows[0]["Price"].ToString());
                        menu.IsActive = bool.Parse(dt.Rows[0]["IsActive"].ToString());
                        menu.FreeDelivery = bool.Parse(dt.Rows[0]["FreeDelivery"].ToString());
                    }
                }
            }
            return View(menu);
        }
        [HttpPost]
        public ActionResult Edit(MenuItems menu)
        {
            int isactive = menu.IsActive ? 1 : 0;
            int freedelivery = menu.FreeDelivery ? 1 : 0;
            string constr = ConfigurationManager.AppSettings["dbconn"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.CommandText = $"update menuitems set name='{ menu.Name}',category='{menu.Category}',price='{menu.Price}',IsActive='{isactive}',FreeDelivery='{freedelivery}',ModifiedOn='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where MenuId={menu.Id}";
                con.Open();
                int rowcount = cmd.ExecuteNonQuery();
                con.Close();
                if (rowcount>0)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }
    }

}