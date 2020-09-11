using EConference.DataAccess.Data;
using EConference.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EConference.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            SP_Call = new SP_Call(_db);
            ConferenceName = new ConferenceNameRepo(_db);
            Papers = new PaperRepository(_db);
        }

        public ISP_Call SP_Call { get; private set; }
        public IConferenceNameRepo ConferenceName { get; private set; }
        public IPaperRepository Papers { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
