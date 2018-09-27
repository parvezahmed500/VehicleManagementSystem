﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Models.EntityModels;

namespace RMS.BLL.Contracts
{
    public interface IEmployeeManager:IManager<Employee>
    {
        ICollection<Employee> SearchByText(string searchText);
        ICollection<Employee> GetAllDriver();
        ICollection<Employee> GetAllEmployees();
        Employee FindByLoginId(int id);
    }
}
