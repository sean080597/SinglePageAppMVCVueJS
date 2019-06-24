using SinglePageAppBusiness.Interface;
using SinglePageAppDomain;
using SinglePageAppRepository;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;

namespace SinglePageApp.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        IEmployeeBusiness db;
        public EmployeeController(IEmployeeBusiness employeeBusiness)
        {
            db = employeeBusiness;
        }

        public IEnumerable<EmployeeDomainModel> Get()
        {
            return db.GetAllEmployee();
        }

        public IHttpActionResult Get(short id)
        {
            var employee = db.GetSingleEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        public IHttpActionResult Post(EMPLOYEE employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.PostSingleEmployee(employee);
            }
            catch (DbUpdateException)
            {
                if (db.EmployeeExists(employee.EMPLOYEE_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employee.EMPLOYEE_ID }, employee);
        }

        public IHttpActionResult Put(short id, EmployeeDomainModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EMPLOYEE_ID)
            {
                return BadRequest();
            }

            try
            {
                db.PutSingleEmployee(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult Delete(short id)
        {
            if (!db.DeleteSingleEmployee(id))
            {
                return NotFound();
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.DBDispose();
            }
            base.Dispose(disposing);
        }

        [Route("all")]
        public IEnumerable<EMPLOYEE> GetEmployeeWithDepartment()
        {
            return db.GetAllEmployeeWithDepartment();
        }

        [Route("all/{empID:int}")]
        public EMPLOYEE GetEmployeeWithDepartment(int empID)
        {
            return db.GetSingleEmployeeWithDepartment(empID);
        }
    }
}
