using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecuDev.Models
{
    public class Location
    {
        public int InstallationID { get; set; }
        public string LocationName { get; set; }
        public string CorpsName { get; set; }
        public string GateName { get; set; }
        public string InstallationDate { get; set; }
        public string InstallationType { get; set; }
        public string SoftwareName { get; set; }
        public string Version { get; set; }
        public string Notes { get; set; }
    }
}