﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Models.EntityModels;

namespace RMS.BLL.Contracts
{
    public interface IDepartmentManager:IManager<Department>
    {
        ICollection<Department> SearchByText(string searchText);
    }
}
