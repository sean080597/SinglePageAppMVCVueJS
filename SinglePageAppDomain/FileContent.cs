using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePageAppDomain
{
    public class FileContent
    {
        public string Reccode { get; set; }
        public string Gpclass { get; set; }
        public string GroupNumber { get; set; }
        public string EfctDate { get; set; }
        public string Freq { get; set; }
        public string Mop { get; set; }
        public string Stddy { get; set; }
        public string Rcdate { get; set; }

        public string Desc { get; set; }
        public string Bcode { get; set; }
        public string BaseCode
        {
            get
            {
                return Bcode + ": " + Desc;
            }
        }

        public string Agc_no { get; set; }
        public string Joint { get; set; }
        public string Tot_life { get; set; }
        public string DateNow {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd");
            }
        }
        public string GroupDateTime { get; set; }
        public string File_ver { get; set; }
        public string Up_ver { get; set; }
        public string Mg_ver { get; set; }
        public string Senddate { get; set; }
        public string Sendtime { get; set; }
        public string BankcdShow { get; set; }
        public string BankPerson { get; set; }
        public Owner FileOwner { get; set; }
        public List<string> Agt_no { get; set; }
        public List<LifeHostModel> Life_hosts { get; set; }
        public string filePath { get; set; }
    }
}
