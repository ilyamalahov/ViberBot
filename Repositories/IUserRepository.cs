using System.Collections.Generic;
using System.Threading.Tasks;
using Viber.Bot;

namespace ViberBot.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(string userId);
        Task<bool> Add(User user);
        Task<bool> Delete(string userId);
    }
}