using EConference.DataAccess.Data;
using EConference.Models; 
using EConference.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EConference.DataAccess.Repository
{
    public class PaperRepo : Repository<Papers>, IPaperRepository
    {
        private readonly ApplicationDbContext _db;

        public PaperRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Papers papers)
        {
            var objFromDB = _db.Papers.FirstOrDefault(a => a.Id == papers.Id);
            if (objFromDB != null)
            {
                objFromDB.PaperId = papers.PaperId;
                objFromDB.PaperTopic = papers.PaperTopic;
                objFromDB.PaperTitle = papers.PaperTitle;
                objFromDB.Publisher = papers.Publisher;
                objFromDB.Participant = papers.Participant;
                objFromDB.TimeZone = papers.TimeZone;
                objFromDB.Country = papers.Country;
                objFromDB.ConferenceName = papers.ConferenceName;
            }
        }
    }
}
