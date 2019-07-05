using SinglePageAppBusiness.Interface;
using SinglePageAppDomain;
using SinglePageAppRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

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
        public string ReadPosBytes(BinaryReader br, int pos, int length)
        {
            byte[] fileData = null;

            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            fileData = br.ReadBytes(length);

            if (pos == 903 || pos == 470 || pos == 433 || pos == 905) return fileData[0] + "";
            return System.Text.Encoding.GetEncoding(932).GetString(fileData); //cuz file's format is Shift-JIS
        }

        public List<FileContent> getListFileContent(string filePath)
        {
            string dirName = filePath.Substring(filePath.Length - 2);
            string desc = db.SVSERVERs.Where(t => t.DIRECTORY == dirName).First().DESCRIPTION;

            List<FileContent> ls_files = new List<FileContent>();
            var files = Directory.GetFiles(filePath).Where(name => !name.ToLower().EndsWith(".log"));

            foreach (string file in files)
            {
                using (BinaryReader br = new BinaryReader(File.Open(file, FileMode.Open)))
                {
                    ls_files.Add(new FileContent
                    {
                        Reccode = ReadPosBytes(br, 0, 1),
                        Gpclass = ReadPosBytes(br, 1, 1),
                        GroupNumber = ReadPosBytes(br, 2, 8),
                        EfctDate = ReadPosBytes(br, 30, 6),
                        Freq = ReadPosBytes(br, 430, 2),
                        Mop = ReadPosBytes(br, 432, 1),
                        Stddy = ReadPosBytes(br, 437, 8),
                        Rcdate = ReadPosBytes(br, 445, 8),
                        Desc = desc,
                        Bcode = ReadPosBytes(br, 453, 2),
                        Agc_no = ReadPosBytes(br, 455, 5),
                        Joint = ReadPosBytes(br, 470, 2),
                        Tot_life = ReadPosBytes(br, 433, 4),
                        GroupDateTime = ReadPosBytes(br, 30, 6),
                        File_ver = ReadPosBytes(br, 903, 2),
                        Up_ver = ReadPosBytes(br, 905, 6),
                        Mg_ver = ReadPosBytes(br, 911, 6),
                        Senddate = ReadPosBytes(br, 917, 8),
                        Sendtime = ReadPosBytes(br, 925, 6),
                        BankcdShow = ReadPosBytes(br, 944, 4),
                        BankPerson = ReadPosBytes(br, 963, 12),
                        FileOwner = new Owner
                        {
                            Cl_No = ReadPosBytes(br, 36, 8),
                            Birth = ReadPosBytes(br, 144, 8),
                            Sex = ReadPosBytes(br, 152, 1),
                            FileOwnNames = new List<OwnNames>()
                            {
                                new OwnNames
                                {
                                    Fm = ReadPosBytes(br, 44, 30),
                                    Gv = ReadPosBytes(br, 74, 20)
                                },
                                new OwnNames
                                {
                                    Fm = ReadPosBytes(br, 94, 30),
                                    Gv = ReadPosBytes(br, 124, 20)
                                }
                            }
                        },
                        Agt_no = new List<string>()
                        {
                            ReadPosBytes(br, 460, 5),
                            ReadPosBytes(br, 465, 5)
                        }
                    });

                    int lhost_pos = (Int32.Parse(ReadPosBytes(br, 903, 2)) == 9) ? 1067 : 1059;
                    int lhost_length = (int)br.BaseStream.Length - lhost_pos;
                    int lhost_lengthObj = (Int32.Parse(ReadPosBytes(br, 903, 2)) == 9) ? 1729 : 1719;
                    int lhost_numberLifeHost = lhost_length / lhost_lengthObj;

                    ls_files[ls_files.Count - 1].Life_hosts = new List<LifeHostModel>();
                    for (int i = 0; i<lhost_numberLifeHost; i++)
                    {
                        int pos = lhost_pos + i * lhost_lengthObj;
                        ls_files[ls_files.Count - 1].Life_hosts.Add(
                            new LifeHostModel
                            {
                                Reccode = ReadPosBytes(br, pos, 1),
                                GroupNumber = ReadPosBytes(br, pos + 1, 8),
                                EfctDate = ReadPosBytes(br, pos + 9, 6),
                                Emp_cd = ReadPosBytes(br, pos + 15, 10),
                                Cov = new List<MgcoverHost>()
                                {
                                    new MgcoverHost
                                    {
                                        Crtable = ReadPosBytes(br, pos + 1617, 2),
                                        Clong = ReadPosBytes(br, pos + 1619, 2),
                                        Plong = ReadPosBytes(br, pos + 1621, 2),
                                        Sumins = ReadPosBytes(br, pos + 1623, 4),
                                    },
                                    new MgcoverHost
                                    {
                                        Crtable = ReadPosBytes(br, pos + 1635, 2),
                                        Clong = ReadPosBytes(br, pos + 1637, 2),
                                        Plong = ReadPosBytes(br, pos + 1639, 2),
                                        Sumins = ReadPosBytes(br, pos + 1641, 4),
                                    },
                                    new MgcoverHost
                                    {
                                        Crtable = ReadPosBytes(br, pos + 1653, 2),
                                        Clong = ReadPosBytes(br, pos + 1655, 2),
                                        Plong = ReadPosBytes(br, pos + 1657, 2),
                                        Sumins = ReadPosBytes(br, pos + 1659, 4),
                                    },
                                    new MgcoverHost
                                    {
                                        Crtable = ReadPosBytes(br, pos + 1671, 2),
                                        Clong = ReadPosBytes(br, pos + 1673, 2),
                                        Plong = ReadPosBytes(br, pos + 1675, 2),
                                        Sumins = ReadPosBytes(br, pos + 1677, 4),
                                    },
                                }
                            }
                        );
                    }
                }
            }
            return ls_files;
        }
    }
}
