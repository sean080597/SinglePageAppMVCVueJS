using SinglePageAppBusiness.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SinglePageAppRepository;
using SinglePageAppDomain;

namespace SinglePageApp.Controllers
{
    [RoutePrefix("api/svserver")]
    public class SVServerController : ApiController
    {
        IEmployeeBusiness db;
        public SVServerController(IEmployeeBusiness employeeBusiness)
        {
            db = employeeBusiness;
        }

        public IEnumerable<SvserverItem> Get()
        {
            return db.GetAllSVSERVER();
        }

        [Route("settings")]
        public IEnumerable<SVSERVERSETTING> GetAllSettings()
        {
            return db.GetAllSVSERVERSetting();
        }

        [Route("getFiles")]
        public IEnumerable<MyFile> GetAllFiles(string filePath)
        {
            return db.GetAllFilesByPath(filePath);
        }

        [HttpPost]
        [Route("writeLogFile")]
        public void writeLogFile(string logPath, MyFile myFile, string datetime)
        {
            db.WriteLogFile(logPath, myFile, datetime);
        }

        [HttpPost]
        [Route("changeSettingValue")]
        public void changeSettingValue(string key, string value)
        {
            db.ChangeSettingValue(key, value);
        }
    }
}
