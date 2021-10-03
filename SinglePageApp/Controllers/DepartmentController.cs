using SinglePageAppBusiness.Interface;
using SinglePageAppDomain;
using SinglePageAppRepository;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;

namespace SinglePageApp.Controllers
{
    [RoutePrefix("api/department")]
    public class DepartmentController : ApiController
    {
        IEmployeeBusiness db;
        public DepartmentController(IEmployeeBusiness employeeBusiness)
        {
            db = employeeBusiness;
        }

        public IEnumerable<DepartmentDomainModel> Get()
        {
            return db.GetAllDepartment();
        }

        public IHttpActionResult Get(short id)
        {
            var department = db.GetSingleDepartment(id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        public IHttpActionResult Post(DEPARTMENT dEPARTMENT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.PostSingleDepartment(dEPARTMENT);
            }
            catch (DbUpdateException)
            {
                if (db.DEPARTMENTExists(dEPARTMENT.DEPARTMENT_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = dEPARTMENT.DEPARTMENT_ID }, dEPARTMENT);
        }

        public IHttpActionResult Put(short id, DEPARTMENT dEPARTMENT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dEPARTMENT.DEPARTMENT_ID)
            {
                return BadRequest();
            }

            try
            {
                db.PutSingleDepartment(dEPARTMENT);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.DEPARTMENTExists(id))
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
            if (!db.DeleteSingleDepartment(id))
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

        [Route("employee/{departID:int}")]
        public ICollection<EMPLOYEE> GetEmployeesByDepartmentID(int departID)
        {
            return db.GetAllEmployeeByDepartID(departID);
        }
    }
}
