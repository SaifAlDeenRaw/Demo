﻿using HealthPartner.Data.Repository.IRepository;
using HealthPartner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPartner.Data.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private ApplicationDBContext _db;

        public CoverTypeRepository(ApplicationDBContext db) : base(db)
        {
            _db=db;
        }

        public void Update(CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }   
    }
}
