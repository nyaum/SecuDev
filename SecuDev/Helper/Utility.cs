using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            }

            return Rtn;

        }

    }
}