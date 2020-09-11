using EConference.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EConference.DataAccess.Repository.IRepository
{
    public interface IPaperRepository : IRepository<RegisterPaper>
    {
        void Update(RegisterPaper paper);
    }
}
