﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Models.DatabaseContext;
using RMS.Models.EntityModels;
using RMS.Repositories.Base;
using RMS.Repositories.Contracts;

namespace RMS.Repositories
{
    public class VehicleRepository:Repository<Vehicle>,IVehicleRepository
    {
        
        public VehicleRepository(DbContext db) : base(db)
        {
        }

        public override ICollection<Vehicle> GetAll()
        {
            return db.Set<Vehicle>().Include(c => c.VehicleType).AsNoTracking().ToList();
        }

        public ICollection<Vehicle> SearchByText(string searchText)
        {
            return
                db.Set<Vehicle>()
                    .Include(c => c.VehicleType)
                    .Where(c => c.BrandName.StartsWith(searchText)||c.ModelName.StartsWith(searchText)||c.RegNo.StartsWith(searchText)
                    ||c.ChassisNo.StartsWith(searchText)||c.VehicleType.Name.StartsWith(searchText)||c.SeatCapacity.ToString().StartsWith(searchText))
                    .ToList();
        }

        public override Vehicle FindById(int id)
        {
            return db.Set<Vehicle>().Where(c=>c.Id==id).Include(c=>c.VehicleType).AsNoTracking().SingleOrDefault();
        }
    }
}
