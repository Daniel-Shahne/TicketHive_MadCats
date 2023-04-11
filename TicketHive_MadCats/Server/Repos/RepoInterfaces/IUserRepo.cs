using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Server.Repos.RepoInterfaces
{
    public interface IUserRepo
    {
        public Task<CustomUser?> GetUserById(int id);
        public Task<CustomUser?> UpdateUser(UpdateUserModel updateUserModel);
    }
}
