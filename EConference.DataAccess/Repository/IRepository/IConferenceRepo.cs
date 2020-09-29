using EConference.Models;

namespace EConference.DataAccess.Repository.IRepository
{
    public interface IConferenceRepo : IRepository<Conference>
    {
        public void Update(Conference conference);
    }
}
