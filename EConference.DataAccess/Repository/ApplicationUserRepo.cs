using EConference.DataAccess.Data;
using EConference.Models; 
using EConference.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EConference.DataAccess.Repository
{
    public class ApplicationUserRepo : Repository<ApplicationUser>, IApplicationUserRepo
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
