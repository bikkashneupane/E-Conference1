using EConference.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EConference.DataAccess.Repository.IRepository
{
    public interface IPaperRepository : IRepository<Papers>
    {
       void Update(Papers papers);
    }
}
