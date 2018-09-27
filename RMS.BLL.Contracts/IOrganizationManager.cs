﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RMS.Models.EntityModels;

namespace RMS.BLL.Contracts
{
    public interface IOrganizationManager:IManager<Organization>
    {
        ICollection<Organization> SearchByText(string searchText);
    }
}
