using FrameWork.DB;
using PagedList;
using SecuDev.Filter;
using SecuDev.Helper;
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
    [SessionFilter]
    public class SoftwareInstallationController : Controller
    {
        public ActionResult Index(FormCollection col, int? Page, int PageSize = 10)
        {
            int PageNo = Page ?? 1;

            SqlParamCollection param = new SqlParamCollection();

            DataSet ds = (new Common()).MdlList(param, "USP_GET_UPDATEBYLOCATION");

            List<Location> list = new List<Location>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                Location l = new Location();

                l.InstallationID = Int32.Parse(ds.Tables[0].Rows[i]["InstallationID"].ToString());
                l.LocationName = ds.Tables[0].Rows[i]["LocationName"].ToString();
                l.CorpsName = ds.Tables[0].Rows[i]["CorpsName"].ToString();
                l.GateName = ds.Tables[0].Rows[i]["GateName"].ToString();
                l.InstallationDate = Utility.DateTimeFormat(ds.Tables[0].Rows[i]["InstallationDate"].ToString(), 1);
                l.InstallationType = ds.Tables[0].Rows[i]["InstallationType"].ToString();
                l.SoftwareName = ds.Tables[0].Rows[i]["SoftwareName"].ToString();
                l.Version = ds.Tables[0].Rows[i]["Version"].ToString();
                l.Notes = ds.Tables[0].Rows[i]["Notes"].ToString();

                list.Add(l);
            }

            ViewBag.Count = list.Count;
            ViewBag.InstallSDate = col["InstallSDate"];
            ViewBag.InstallEDate = col["InstallEDate"];

            return View(list.ToPagedList(PageNo, PageSize));
        }
    }
}