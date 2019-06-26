using DataLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TestWebApplication.WebApi.Common;

namespace WebAPI.Controllers
{
    public class EmployeeController : ApiController
    {
        SQLHelper _sQLHelper;

        public EmployeeController()
        {
            _sQLHelper = new SQLHelper();
        }

        [HttpPost]
        public IList<Employee> GetEmployees()
        => _sQLHelper.LoadListData<Employee>(null, "Get_Student", "GetEmployeeURL");
    }
}