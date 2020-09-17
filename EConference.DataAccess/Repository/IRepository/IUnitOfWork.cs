using System;
using System.Collections.Generic;
using System.Text;

namespace EConference.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        ISP_Call SP_Call { get; }

        IConferenceNameRepo ConferenceName { get; }

        IPaperRepository Papers { get; }

        void Save();
    }
}
