﻿using FlameAndWax.Data.Models;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Repositories.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<EmployeeModel>
    {
        Task UpdateProfilePicture(string profileLink);
        Task<int> Login(EmployeeModel employee);
        Task ModifyEmployeeStatus(int employeeId, Constants.Constants.EmployeeAccountStatus accountStatus);
    }
}
