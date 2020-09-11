using System;
using System.Collections.Generic;
using System.Text;

namespace EConference.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IPaperRepository Papers { get; }

        ISP_Call SP_Call { get; }
    }
}
