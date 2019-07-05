using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePageAppDomain
{
    public class Owner
    {
        public string Cl_No { get; set; }
        public string Birth { get; set; }
        public string Sex { get; set; }

        public List<OwnNames> FileOwnNames { get; set; }
    }
}
