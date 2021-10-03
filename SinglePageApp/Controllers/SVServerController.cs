using SinglePageAppBusiness.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SinglePageAppRepository;
using SinglePageAppDomain;
using System.IO;

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

        //get all files content
        [Route("getFilesContent")]
        public IEnumerable<FileContent> GetAllFilesContent(string filePath)
        {
            return db.getListFileContent(filePath);
        }

        [HttpPost]
        [Route("insertPRGR")]
        //insert PRGR
        public string InsertPRGRTable(FileContent content)
        {
            return db.insertPRGR(content);
        }

        [HttpGet]
        //get all files content
        [Route("readfile")]
        public string ReadBinFile(string filePath)
        {
            byte[] fileData = null;

            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                fileData = br.ReadBytes((int)br.BaseStream.Length);

                return System.Text.Encoding.GetEncoding(932).GetString(fileData); //cuz file's format is Shift-JIS
            }
        }
    }
}
