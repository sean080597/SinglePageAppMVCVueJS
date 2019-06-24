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

            return System.Text.Encoding.UTF8.GetString(fileData);
        }
    }
}
