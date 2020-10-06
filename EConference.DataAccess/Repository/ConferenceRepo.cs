using EConference.DataAccess.Data;
using EConference.Models;
using EConference.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EConference.DataAccess.Repository
{
    public class ConferenceRepo : Repository<Conference>, IConferenceRepo
    {
        private readonly ApplicationDbContext _db;

        public ConferenceRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Conference conference)
        {
            _db.Conferences.Update(conference);
        }
    }
}
