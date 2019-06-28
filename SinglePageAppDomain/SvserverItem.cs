using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePageAppDomain
{
    public class SvserverItem
    {
        public decimal ID { get; set; }
        public Nullable<byte> CODE { get; set; }
        public Nullable<byte> DIVISION { get; set; }
        public string DESCRIPTION { get; set; }
        public string NAME { get; set; }
        public string DIRECTORY { get; set; }
        public string CODE2 { get; set; }
        public Nullable<decimal> ORDINAL { get; set; }
        public string PATH {
            get
            {
                return NAME + '\\' + DIRECTORY;
            }
        }
    }
}
