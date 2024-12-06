using FrameWork.DB;
using SecuDev.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebAdmin.Models;

namespace SecuDev.Helper
{
    public class Utility
    {
        /// <summary>
        /// <para>Get Now DateTime </para>
        /// </summary>
        /// <returns></returns>
        public static string GetNowDate()
        {
            string Rtn = "";

            Rtn = DateTime.Now.ToString();

            return Rtn;
        }

        /// <summary>
        /// <para>DateTime Formatter</para>
        /// <para>format 1 : yyyy-MM-dd</para>
        /// <para>format 2 : yyyy-MM-dd HH:mm:ss</para>
        /// <para>format 3 : yyyy-MM-dd dddd HH:mm:ss</para>
        /// <para>format 4 : yyyy-MM-dd tt hh:mm:ss</para>
        /// <para>format 5 : yyyy</para>
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DateTimeFormat(string date, int format)
        {
            string Rtn = "";

            if (date != "" && date != null)
            {

                DateTime dt = Convert.ToDateTime(date);

                switch (format)
                {
                    case 1:
                        Rtn = String.Format("{0:yyyy-MM-dd}", dt);
                        break;
                    case 2:
                        Rtn = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dt);
                        break;
                    case 3:
                        Rtn = String.Format("{0:yyyy-MM-dd dddd HH:mm:ss}", dt);
                        break;
                    case 4:
                        Rtn = String.Format("{0:yyyy-MM-dd tt hh:mm:ss}", dt);
                        break;
                    case 5:
                        Rtn = String.Format("{0:yyyy}", dt);
                        break;
                    case 6:
                        Rtn = String.Format("{0:yyyy년 MM월 dd일}", dt);
                        break;
                }
            }

            return Rtn;

        }

        /// <summary>
        /// <para>Get Location List</para>
        /// </summary>
        /// <returns></returns>
        public static List<Location> GetLocationList()
        {
            SqlParamCollection param = new SqlParamCollection();

            param.Add("@Type", "Location");

            DataSet ds = (new Common()).MdlList(param, "PROC_LIST");

            List<Location> list = new List<Location>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Location l = new Location();

                l.LocationID = Int32.Parse(ds.Tables[0].Rows[i]["LocationID"].ToString());
                l.LocationName = ds.Tables[0].Rows[i]["LocationName"].ToString();
                l.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                l.Description = ds.Tables[0].Rows[i]["Description"].ToString();

                list.Add(l);
            }

            return list;
        }

        /// <summary>
        /// <para>Get Software List</para>
        /// </summary>
        /// <returns></returns>
        public static List<Software> GetSoftwareList()
        {
            SqlParamCollection param = new SqlParamCollection();

            param.Add("@Type", "Software");

            DataSet ds = (new Common()).MdlList(param, "PROC_LIST");

            List<Software> list = new List<Software>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Software s = new Software();

                s.SoftwareID = Int32.Parse(ds.Tables[0].Rows[i]["SoftwareID"].ToString());
                s.SoftwareName = ds.Tables[0].Rows[i]["SoftwareName"].ToString();

                list.Add(s);
            }

            return list;
        }
    }
}