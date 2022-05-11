using HealthPartner.Data.Repository.IRepository;
using HealthPartner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPartner.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDBContext _db;

        public CategoryRepository(ApplicationDBContext db) : base(db)
        {
            _db=db;
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }   
    }
}
