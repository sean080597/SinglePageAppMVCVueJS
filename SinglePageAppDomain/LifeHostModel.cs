using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePageAppDomain
{
    public class LifeHostModel
    {
        public List<MgcoverHost> Cov { get; set; }
        public string Reccode { get; set; }
        public string GroupNumber { get; set; }
        public string EfctDate { get; set; }
        public string Emp_cd { get; set; }
        public Owner Life { get; set; }
        public string Ocp_cd { get; set; }
        public string Death_rel { get; set; }
        public string D_joint { get; set; }
        public string Lneeds { get; set; }
        public string Shitei { get; set; }
        public string Totp { get; set; }
        public string Reqno { get; set; }
        public string DateNow
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd");
            }
        }
    }
}
