﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RMS.Models.EntityModels.Identity
{
    public class AppRole:IdentityRole<int,AppUserRole>
    {
    }
}
