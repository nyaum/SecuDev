using FrameWork.DB;
using SecuDev.Helper;
using SecuDev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Models;
using PagedList;
using SecuDEV.Manager;

namespace SecuDev.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string alertType = "")
        {
            // 세션 초기화
            Session.Clear();

            ViewBag.alertType = alertType;

            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection col)
        {
            // 세션 초기화
            Session.Clear();

            string Result = "";

            string UserID = "";
            string Password = CryptoManager.EncryptBySHA256(col["Password"]);

            try
            {
                SqlParamCollection param = new SqlParamCollection();

                param.Add("@UserID", col["UserID"]);
                param.Add("@Password", CryptoManager.EncryptBySHA256(col["Password"]));

                DataSet ds = (new Common()).MdlList(param, "PROC_LOGIN");

                if (ds.Tables[0].Rows.Count == 1)
                {

                    Session["UserID"] = ds.Tables[0].Rows[0]["UserID"].ToString();
                    Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                    Session["AuthorityLevel"] = ds.Tables[0].Rows[0]["AuthorityLevel"].ToString();

                    Result = ds.Tables[0].Rows[0]["Result"].ToString();
                }
                else
                {
                    Result = "Invalid";
                }
            }
            catch (Exception ex)
            {
                Result = "ERR";
            }
            

            return Json(new { Result });
        }

    }
}