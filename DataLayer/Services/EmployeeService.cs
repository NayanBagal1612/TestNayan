using DataLayer.Common;
using DataLayer.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class EmployeeService
    {
        public async Task<IList<Employee>> GetEmployee()
        => await Helper.ExecutePostCall<IList<Employee>, Type>(null, Configuration.GetEmployeeURL);
    }
}
