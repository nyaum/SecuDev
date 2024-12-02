using FrameWork.DB;
using SecuDev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Models;

namespace SecuDev.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            SqlParamCollection param = new SqlParamCollection();

            DataSet ds = (new Common()).MdlList(param, "usp_GetLatestUpdatesByLocation");

            List<Location> list = new List<Location>();
            
            for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                Location l = new Location();

                l.LocationName = ds.Tables[0].Rows[i]["LocationName"].ToString();
                l.CorpsName = ds.Tables[0].Rows[i]["CorpsName"].ToString();
                l.GateName = ds.Tables[0].Rows[i]["GateName"].ToString();
                l.InstallationDate = ds.Tables[0].Rows[i]["InstallationDate"].ToString();
                l.InstallationType = ds.Tables[0].Rows[i]["InstallationType"].ToString();
                l.SoftwareName = ds.Tables[0].Rows[i]["SoftwareName"].ToString();
                l.Version = ds.Tables[0].Rows[i]["Version"].ToString();
                l.Notes = ds.Tables[0].Rows[i]["Notes"].ToString();

                list.Add(l);
            }

            ViewBag.list = list;

            return View();
        }

        public ActionResult Setup()
        {


            return View();
        }

    }
}