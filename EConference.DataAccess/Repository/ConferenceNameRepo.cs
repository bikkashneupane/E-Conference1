using EConference.DataAccess.Data;
using EConference.Models; 
using EConference.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EConference.DataAccess.Repository
{
    public class ConferenceNameRepo : Repository<ConferenceName>, IConferenceNameRepo
    {
        private readonly ApplicationDbContext _db;

        public ConferenceNameRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ConferenceName conferenceName)
        {
            var objFromDB = _db.ConferenceName.FirstOrDefault(a=> a.Id == conferenceName.Id);
        }
    }
}
