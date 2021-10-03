using SinglePageAppBusiness.Interface;
using SinglePageAppDomain;
using SinglePageAppRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace SinglePageAppBusiness
{
    public class EmployeeBusiness : IEmployeeBusiness
    {
        EmployeeEntities db = new EmployeeEntities();

        public List<DepartmentDomainModel> GetAllDepartment()
        {
            List<DepartmentDomainModel> list = db.DEPARTMENTs.Select(t => new DepartmentDomainModel
            {
                DEPARTMENT_ID = t.DEPARTMENT_ID,
                DEPARTMENT_NAME = t.DEPARTMENT_NAME,
                DEPARTMENT_HEAD = t.DEPARTMENT_HEAD
            }).ToList();
            return list;
        }

        public DepartmentDomainModel GetSingleDepartment(int departID)
        {
            DepartmentDomainModel single = db.DEPARTMENTs.Where(t => t.DEPARTMENT_ID == departID).Select(t => new DepartmentDomainModel {
                DEPARTMENT_ID = t.DEPARTMENT_ID,
                DEPARTMENT_NAME = t.DEPARTMENT_NAME,
                DEPARTMENT_HEAD = t.DEPARTMENT_HEAD
            }).First();
            return single;
        }

        public void PostSingleDepartment(DEPARTMENT department)
        {
            department.DEPARTMENT_ID = (short)(db.DEPARTMENTs.Max(t => t.DEPARTMENT_ID) + 1);
            db.DEPARTMENTs.Add(department);
            db.SaveChanges();
        }

        public void PutSingleDepartment(DEPARTMENT department)
        {
            db.Entry(department).State = EntityState.Modified;
            db.SaveChanges();
        }

        public bool DeleteSingleDepartment(short departID)
        {
            DEPARTMENT dEPARTMENT = db.DEPARTMENTs.Find(departID);
            if (dEPARTMENT == null)
            {
                return false;
            }
            db.DEPARTMENTs.Remove(dEPARTMENT);
            db.SaveChanges();
            return true;
        }

        bool IEmployeeBusiness.DEPARTMENTExists(short departID)
        {
            return db.DEPARTMENTs.Count(e => e.DEPARTMENT_ID == departID) > 0;
        }

        //============================ EMPLOYEE METHODS ===============================================
        public List<EmployeeDomainModel> GetAllEmployee()
        {
            List<EmployeeDomainModel> list = db.EMPLOYEEs.Select(t => new EmployeeDomainModel
            {
                EMPLOYEE_ID = t.EMPLOYEE_ID,
                EMPLOYEE_NAME = t.EMPLOYEE_NAME,
                EMPLOYEE_SALARY = t.EMPLOYEE_SALARY,
                EMPLOYEE_DEPARTMENT = t.EMPLOYEE_DEPARTMENT
            }).ToList();
            return list;
        }

        public EmployeeDomainModel GetSingleEmployee(int empID)
        {
            EmployeeDomainModel single = db.EMPLOYEEs.Where(t => t.EMPLOYEE_ID == empID).Select(t => new EmployeeDomainModel
            {
                EMPLOYEE_ID = t.EMPLOYEE_ID,
                EMPLOYEE_NAME = t.EMPLOYEE_NAME,
                EMPLOYEE_SALARY = t.EMPLOYEE_SALARY,
                EMPLOYEE_DEPARTMENT = t.EMPLOYEE_DEPARTMENT
            }).First();
            return single;
        }

        public void PostSingleEmployee(EMPLOYEE employee)
        {
            employee.EMPLOYEE_ID = (short)(db.EMPLOYEEs.Max(t => t.EMPLOYEE_ID) + 1);
            db.EMPLOYEEs.Add(employee);
            db.SaveChanges();
        }

        public void PutSingleEmployee(EmployeeDomainModel employee)
        {
            //db.Entry(employee).State = EntityState.Modified;
            EMPLOYEE emp = db.EMPLOYEEs.SingleOrDefault(t => t.EMPLOYEE_ID == employee.EMPLOYEE_ID);
            emp.EMPLOYEE_NAME = employee.EMPLOYEE_NAME;
            emp.EMPLOYEE_SALARY = employee.EMPLOYEE_SALARY;
            emp.EMPLOYEE_DEPARTMENT = employee.EMPLOYEE_DEPARTMENT;
            db.SaveChanges();
        }

        public bool DeleteSingleEmployee(short empID)
        {
            EMPLOYEE employee = db.EMPLOYEEs.Find(empID);
            if (employee == null)
            {
                return false;
            }
            db.EMPLOYEEs.Remove(employee);
            db.SaveChanges();
            return true;
        }

        public bool EmployeeExists(short empID)
        {
            return db.EMPLOYEEs.Count(e => e.EMPLOYEE_ID == empID) > 0;
        }

        //=====================================================
        public List<EMPLOYEE> GetAllEmployeeWithDepartment()
        {
            return db.EMPLOYEEs.ToList();
        }

        public List<EMPLOYEE> GetAllEmployeeByDepartID(int departID)
        {
            return db.EMPLOYEEs.Where(t => t.DEPARTMENT.DEPARTMENT_ID == departID).ToList();
        }

        public EMPLOYEE GetSingleEmployeeWithDepartment(int empID)
        {
            return db.EMPLOYEEs.Find(empID);
        }

        //=======================================================================================
        public void DBDispose()
        {
            db.Dispose();
        }

        //=======================================================================================
        public bool WriteBinFile(string msg, string filePath)
        {
            bool isSuccess = false;

            if (!String.IsNullOrEmpty(msg))
            {
                //var byteArray = System.Text.Encoding.UTF8.GetBytes(msg);
                //System.IO.File.WriteAllBytes(filePath, byteArray);
                using (BinaryWriter binWriter = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                {
                    var byteArray = System.Text.Encoding.UTF8.GetBytes(msg);
                    binWriter.Write(byteArray);
                }
                isSuccess = true;
            }
            return isSuccess;
        }

        public string ReadBinFile(string filePath, int bytePos, int byteReq)
        {
            byte[] fileData = null;

            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                br.BaseStream.Seek(bytePos, SeekOrigin.Begin);
                fileData = br.ReadBytes(byteReq);
            }

            //return System.Text.Encoding.Default.GetString(fileData);
            //return System.Text.Encoding.UTF8.GetString(fileData);
            return System.Text.Encoding.GetEncoding(932).GetString(fileData); //cuz file's format is Shift-JIS
        }

        //==================================== SVSERVER =================================
        public List<SVSERVERSETTING> GetAllSVSERVERSetting()
        {
            return db.SVSERVERSETTINGS.ToList();
        }

        public List<SvserverItem> GetAllSVSERVER()
        {
            byte default_code = 0;
            byte default_division = 0;
            List<SVSERVERSETTING> settings = db.SVSERVERSETTINGS.ToList();
            foreach(SVSERVERSETTING item in settings)
            {
                if(item.SETTINGNAME == "DEFAULTCODE")
                {
                    default_code = Byte.Parse(item.SETTINGVALUE);
                    continue;
                }
                if (item.SETTINGNAME == "DEFAULTDIVISION")
                {
                    default_division = Byte.Parse(item.SETTINGVALUE);
                }
            }
            return db.SVSERVERs.Select(t => new SvserverItem
                {
                    ID = t.ID,
                    CODE = t.CODE,
                    CODE2 = t.CODE2,
                    DESCRIPTION = t.DESCRIPTION,
                    DIRECTORY = t.DIRECTORY,
                    DIVISION = t.DIVISION,
                    NAME = t.NAME,
                    ORDINAL = t.ORDINAL
                })
                .OrderBy(t => (t.CODE == default_code && t.DIVISION == default_division) ? 0 : 1)
                .ThenBy(t => t.ID).ToList();

        }

        public IEnumerable<MyFile> GetAllFilesByPath(string filePath)
        {
            List<MyFile> ls_files = new List<MyFile>();
            var files = Directory.GetFiles(filePath).Where(name => !name.ToLower().EndsWith(".log"));
            foreach (string file in files)
            {
                ls_files.Add(new MyFile {
                    FileName = Path.GetFileName(file),
                    FilePath = file,
                    FileDateModified = File.GetLastWriteTime(file).ToString()
                });
            }
            return ls_files;
        }

        public void WriteLogFile(string logPath, MyFile myFile, string datetime)
        {
            string logName = db.SVSERVERSETTINGS.Where(t => t.SETTINGNAME == "LOGFILE").First().SETTINGVALUE;
            logPath = Path.Combine(logPath, logName);
            using (StreamWriter writetext = new StreamWriter(logPath, true))
            {
                writetext.WriteLine(datetime + " --- " + myFile.FileName + " --- " + myFile.FilePath + " --- " + myFile.FileDateModified);
            }
        }

        public void ChangeSettingValue(string key, string value)
        {
            db.SVSERVERSETTINGS.Find(key).SETTINGVALUE = value;
            db.SaveChanges();
        }

        //Read files
        public string ReadPosBytes(BinaryReader br, int pos, int length, string type)
        {
            byte[] fileData = null;

            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            fileData = br.ReadBytes(length);

            switch (type)
            {
                case "short":
                    return BitConverter.ToUInt16(fileData, 0) + "";
                case "long":
                    return BitConverter.ToUInt32(fileData, 0) + "";
                default:
                    return System.Text.Encoding.GetEncoding(932).GetString(fileData); //cuz file's format is Shift-JIS
            }
        }

        public List<FileContent> getListFileContent(string filePath)
        {
            if (filePath == "\\") return null;

            string dirName = filePath.Substring(filePath.Length - 2);
            string desc = db.SVSERVERs.Where(t => t.DIRECTORY == dirName).First().DESCRIPTION;

            List<FileContent> ls_files = new List<FileContent>();
            IEnumerable<string> files = Directory.GetFiles(filePath).Where(name => !name.ToLower().EndsWith(".log"));

            foreach (string file in files)
            {
                using (BinaryReader br = new BinaryReader(File.Open(file, FileMode.Open)))
                {
                    //if (ReadPosBytes(br, 2, 8, null) == "00235128")
                    //{
                    //    var x = BitConverter.GetBytes(20000000);
                    //    var y = BitConverter.ToUInt32(x, 0);
                    //    var z = ReadPosBytes(br, 1067 + 1627, 4, "long");
                    //}

                    ls_files.Add(new FileContent
                    {
                        Reccode = ReadPosBytes(br, 0, 1, null),
                        Gpclass = ReadPosBytes(br, 1, 1, null),
                        GroupNumber = ReadPosBytes(br, 2, 8, null),
                        EfctDate = ReadPosBytes(br, 30, 6, null),
                        Freq = ReadPosBytes(br, 430, 2, null),
                        Mop = ReadPosBytes(br, 432, 1, null),
                        Stddy = ReadPosBytes(br, 437, 8, null),
                        Rcdate = ReadPosBytes(br, 445, 8, null),
                        Desc = desc,
                        Bcode = ReadPosBytes(br, 453, 2, null),
                        Agc_no = ReadPosBytes(br, 455, 5, null),
                        Joint = ReadPosBytes(br, 470, 2, "short"),
                        Tot_life = ReadPosBytes(br, 433, 4, "long"),
                        GroupDateTime = ReadPosBytes(br, 30, 6, null),
                        File_ver = ReadPosBytes(br, 903, 2, "short"),
                        Up_ver = ReadPosBytes(br, 905, 6, null),
                        Mg_ver = ReadPosBytes(br, 911, 6, null),
                        Senddate = ReadPosBytes(br, 917, 8, null),
                        Sendtime = ReadPosBytes(br, 925, 6, null),
                        BankcdShow = ReadPosBytes(br, 944, 4, null),
                        BankPerson = ReadPosBytes(br, 963, 12, null),
                        FileOwner = new Owner
                        {
                            Cl_No = ReadPosBytes(br, 36, 8, null),
                            Birth = ReadPosBytes(br, 144, 8, null),
                            Sex = ReadPosBytes(br, 152, 1, null),
                            Ad_code = ReadPosBytes(br, 169, 11, null),
                            Zip = ReadPosBytes(br, 180, 10, null),
                            FileOwnNames = new List<OwnNames>()
                            {
                                new OwnNames
                                {
                                    Fm = ReadPosBytes(br, 44, 30, null),
                                    Gv = ReadPosBytes(br, 74, 20, null)
                                },
                                new OwnNames
                                {
                                    Fm = ReadPosBytes(br, 94, 30, null),
                                    Gv = ReadPosBytes(br, 124, 20, null)
                                }
                            }
                        },
                        Agt_no = new List<string>()
                        {
                            ReadPosBytes(br, 460, 5, null),
                            ReadPosBytes(br, 465, 5, null)
                        },
                        filePath = file
                    });

                    int lhost_pos = (Int32.Parse(ReadPosBytes(br, 903, 2, "short")) == 9) ? 1067 : 1059;
                    int lhost_length = (int)br.BaseStream.Length - lhost_pos;
                    int lhost_lengthObj = (Int32.Parse(ReadPosBytes(br, 903, 2, "short")) == 9) ? 1729 : 1719;
                    int lhost_numberLifeHost = lhost_length / lhost_lengthObj;

                    ls_files[ls_files.Count - 1].Life_hosts = new List<LifeHostModel>();
                    for (int i = 0; i<lhost_numberLifeHost; i++)
                    {
                        int pos = lhost_pos + i * lhost_lengthObj;
                        ls_files[ls_files.Count - 1].Life_hosts.Add(
                            new LifeHostModel
                            {
                                Reccode = ReadPosBytes(br, pos, 1, null),
                                GroupNumber = ReadPosBytes(br, pos + 1, 8, null),
                                EfctDate = ReadPosBytes(br, pos + 9, 6, null),
                                Emp_cd = ReadPosBytes(br, pos + 15, 10, null),
                                Life = new Owner
                                {
                                    Cl_No = ReadPosBytes(br, pos + 25, 8, null),
                                    Birth = ReadPosBytes(br, pos + 133, 8, null),
                                    Sex = ReadPosBytes(br, pos + 141, 1, null),
                                    Ad_code = ReadPosBytes(br, pos + 158, 11, null),
                                    Zip = ReadPosBytes(br, pos + 169, 10, null),
                                    FileOwnNames = new List<OwnNames>
                                    {
                                        new OwnNames
                                        {
                                            Fm = ReadPosBytes(br, pos + 33, 30, null),
                                            Gv = ReadPosBytes(br, pos + 63, 20, null)
                                        },
                                        new OwnNames
                                        {
                                            Fm = ReadPosBytes(br, pos + 83, 30, null),
                                            Gv = ReadPosBytes(br, pos + 113, 20, null)
                                        }
                                    }
                                },
                                Ocp_cd = ReadPosBytes(br, pos + 419, 4, null),
                                Death_rel = ReadPosBytes(br, pos + 817, 2, null),
                                D_joint = ReadPosBytes(br, pos + 819, 2, "short"),
                                Lneeds = ReadPosBytes(br, pos + 1219, 1, null),
                                Shitei = ReadPosBytes(br, pos + 1220, 1, null),
                                Cov = new List<MgcoverHost>()
                                {
                                    new MgcoverHost
                                    {
                                        Crtable = ReadPosBytes(br, pos + 1617, 2, null),
                                        Clong = ReadPosBytes(br, pos + 1619, 2, "short"),
                                        Sumins = ReadPosBytes(br, pos + 1623, 4, "long"),
                                        Plong = ReadPosBytes(br, pos + 1627, 4, "long"),
                                    },
                                    new MgcoverHost
                                    {
                                        Crtable = ReadPosBytes(br, pos + 1635, 2, null),
                                        Clong = ReadPosBytes(br, pos + 1637, 2, "short"),
                                        Sumins = ReadPosBytes(br, pos + 1641, 4, "long"),
                                        Plong = ReadPosBytes(br, pos + 1645, 4, "long"),
                                    },
                                    new MgcoverHost
                                    {
                                        Crtable = ReadPosBytes(br, pos + 1653, 2, null),
                                        Clong = ReadPosBytes(br, pos + 1655, 2, "short"),
                                        Sumins = ReadPosBytes(br, pos + 1659, 4, "long"),
                                        Plong = ReadPosBytes(br, pos + 1663, 4, "long"),
                                    },
                                    new MgcoverHost
                                    {
                                        Crtable = ReadPosBytes(br, pos + 1671, 2, null),
                                        Clong = ReadPosBytes(br, pos + 1673, 2, "short"),
                                        Sumins = ReadPosBytes(br, pos + 1677, 4, "long"),
                                        Plong = ReadPosBytes(br, pos + 1681, 4, "long"),
                                    },
                                },
                                Totp = ReadPosBytes(br, pos + 1699, 4, "long"),
                                Reqno = ReadPosBytes(br, pos + 1704, 10, null)
                            }
                        );
                    }
                }
            }
            return ls_files;
        }

        public string insertPRGR(FileContent content)
        {
            //check if exists PRGR in DB
            bool founded = db.PRGRs.Any(t => t.GRUPNUM == content.GroupNumber && t.EFCTYYYYMM == content.EfctDate);
            if (founded)
            {
                return delPRGR(content.filePath);
            }

            //insert PRGR
            PRGR pRGR = new PRGR();
            pRGR.RECTYPE = Byte.Parse(content.Reccode);
            pRGR.KGRPTYPE = Byte.Parse(content.Gpclass);
            pRGR.GRUPNUM = content.GroupNumber;
            pRGR.EFCTYYYYMM = content.EfctDate;
            pRGR.KJSOWNAME = content.FileOwner.FileOwnNames[0].Fm;
            pRGR.KJGOWNAME = content.FileOwner.FileOwnNames[0].Gv;
            pRGR.KSUOWNAME = content.FileOwner.FileOwnNames[1].Fm;
            pRGR.KGIOWNAME = content.FileOwner.FileOwnNames[1].Gv;
            pRGR.KOWDOB = content.FileOwner.Birth;
            pRGR.KOWSEX = content.FileOwner.Sex;
            pRGR.KOWADDRCD = content.FileOwner.Ad_code;
            pRGR.KOWPOSTCDE = content.FileOwner.Zip;
            pRGR.BILLFREQ = content.Freq;
            pRGR.MOP = content.Mop;
            pRGR.KTOTCNT = content.Life_hosts.Count;
            pRGR.OCCDATE = content.Stddy;
            pRGR.CCDATE = content.Rcdate;
            pRGR.BRANCH = content.Bcode;
            pRGR.ZAGCYNUM = content.Agc_no;
            pRGR.ZAGNTNUM = content.Agt_no[0];
            pRGR.KJOINRAT = Int16.Parse(content.Joint);
            pRGR.KSNDCNT = Decimal.Parse(content.Tot_life);
            pRGR.ZDATSEND = DateTime.Now.ToString("yyyyMMdd");
            pRGR.ZVRSNSVF = Int16.Parse(content.File_ver);
            pRGR.ZVRSNSLT = content.Mg_ver;
            pRGR.ENTDATE = content.Senddate;
            pRGR.ENTTIME = content.Sendtime;
            pRGR.ZBKCODE = content.BankcdShow;
            pRGR.ZBKCLCDE = content.BankPerson;

            db.PRGRs.Add(pRGR);

            //insert PRCL
            foreach (LifeHostModel item in content.Life_hosts)
            {
                PRCL pRCL = new PRCL();
                pRCL.RECTYPE = Byte.Parse(item.Reccode);
                pRCL.GRUPNUM = item.GroupNumber;
                pRCL.EFCTYYYYMM = item.EfctDate;
                pRCL.MEMBSEL = item.Emp_cd;
                pRCL.KJSLANAME = item.Life.FileOwnNames[0].Fm;
                pRCL.KJGLANAME = item.Life.FileOwnNames[0].Gv;
                pRCL.KSULANAME = item.Life.FileOwnNames[1].Fm;
                pRCL.KGILANAME = item.Life.FileOwnNames[1].Gv;
                pRCL.CLNTNUM = item.Life.Cl_No;
                pRCL.KLADOB = item.Life.Birth;
                pRCL.KLASEX = item.Life.Sex;
                pRCL.KLAADDRCD = item.Life.Ad_code;
                pRCL.KLAPOSTCDE = item.Life.Zip;
                pRCL.OCCUP = item.Ocp_cd;
                pRCL.KBN1RLN = Int16.Parse(item.Death_rel);
                pRCL.KBN1PS = Int16.Parse(item.D_joint);
                pRCL.ZLNFLAG = item.Lneeds;
                pRCL.ZDPFLAG = item.Shitei;
                pRCL.ZCRTBL = item.Cov[0].Crtable;
                pRCL.ZRCESTRM = Int16.Parse(item.Cov[0].Clong);
                pRCL.ZSUMINS = Int32.Parse(item.Cov[0].Sumins);
                pRCL.KCOVPREM = Int32.Parse(item.Cov[0].Plong);
                pRCL.KTOTPRM = Int32.Parse(item.Totp);
                pRCL.ZSEQ10 = item.Reqno;
                pRCL.ZDATSEND = DateTime.Now.ToString("yyyyMMdd");
                db.PRCLs.Add(pRCL);
            }

            db.SaveChanges();

            //del file after inserting PRGR
            return delPRGR(content.filePath);
        }

        public string delPRGR(string filePath)
        {
            JsonResult result;
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(filePath))
                {
                    // If file found, delete it    
                    File.Delete(filePath);
                    result = new JsonResult { type = "success", value = "Import Data successfully" };
                }
                else result = new JsonResult { type = "error", value = "File not found" };
            }
            catch (IOException ioExp)
            {
                result = new JsonResult { type = "error", value = ioExp.Message };
            }

            return new JavaScriptSerializer().Serialize(result);
        }
    }
}
