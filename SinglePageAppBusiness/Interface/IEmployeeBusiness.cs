using SinglePageAppDomain;
using SinglePageAppRepository;
using System.Collections.Generic;
using System.IO;

namespace SinglePageAppBusiness.Interface
{
    public interface IEmployeeBusiness
    {
        List<DepartmentDomainModel> GetAllDepartment();
        DepartmentDomainModel GetSingleDepartment(int departID);
        void PostSingleDepartment(DEPARTMENT department);
        void PutSingleDepartment(DEPARTMENT department);
        bool DeleteSingleDepartment(short departID);
        bool DEPARTMENTExists(short departID);

        //==================================================================
        List<EmployeeDomainModel> GetAllEmployee();
        List<EMPLOYEE> GetAllEmployeeWithDepartment();
        List<EMPLOYEE> GetAllEmployeeByDepartID(int departID);
        EmployeeDomainModel GetSingleEmployee(int empID);
        EMPLOYEE GetSingleEmployeeWithDepartment(int empID);
        void PostSingleEmployee(EMPLOYEE employee);
        void PutSingleEmployee(EmployeeDomainModel employee);
        bool DeleteSingleEmployee(short empID);
        bool EmployeeExists(short empID);

        //==================================================================
        void DBDispose();

        //==================================================================
        bool WriteBinFile(string msg, string filePath);
        string ReadBinFile(string filePath, int bytePos, int byteReq);

        //==================================================================
        List<SvserverItem> GetAllSVSERVER();
        List<SVSERVERSETTING> GetAllSVSERVERSetting();
        IEnumerable<MyFile> GetAllFilesByPath(string filePath);
        void WriteLogFile(string logPath, MyFile myFile, string datetime);
        void ChangeSettingValue(string key, string value);

        //read file
        string ReadPosBytes(BinaryReader br, int pos, int length, string type);
        List<FileContent> getListFileContent(string filePath);

        //add to db
        string delPRGR(string filePath);
        string insertPRGR(FileContent content);
    }
}
