using EConference.DataAccess.Data;
using EConference.DataAccess.Repository.IRepository;
using EConference.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EConference.DataAccess.Repository
{
    public class PaperRepository : Repository<RegisterPaper>, IPaperRepository
    {
        private readonly ApplicationDbContext _db;

        public PaperRepository(ApplicationDbContext db): base (db)
        {
            _db = db;
        }

        public void Update(RegisterPaper paper)
        {
            var objFromDb = _db.PapersRegistered.FirstOrDefault(s => s.Id == paper.Id);
            
            if (objFromDb != null)
            {
                objFromDb.PaperId = paper.PaperId;
                objFromDb.PaperTitle = paper.PaperTitle;
                objFromDb.PaperTopic = paper.PaperTopic;
                objFromDb.Publisher = paper.Publisher;
                objFromDb.Participant = paper.Participant; 
                
                _db.SaveChanges();
            }
        }
    }
}
