using SinglePageAppBusiness.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SinglePageApp.Controllers
{
    [RoutePrefix("api/binfile")]
    public class BinfileController : ApiController
    {
        IEmployeeBusiness db;

        public BinfileController(IEmployeeBusiness employeeBusiness)
        {
            db = employeeBusiness;
        }

        [HttpGet]
        public string Get(string fileName, int bytePos, int byteReq)
        {
            string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/FilesUpload"), fileName);
            return db.ReadBinFile(filePath, bytePos, byteReq);
        }

        [HttpPost]
        public IHttpActionResult Post(string msg)
        {
            string fileName = "test_content.bin";
            string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/FilesUpload"), fileName);
            if (db.WriteBinFile(msg, filePath))
                return Ok();
            return BadRequest("Some error has occurred");
        }
    }
}
