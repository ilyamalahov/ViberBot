using System.Collections.Generic;
using System.Threading.Tasks;
using Viber.Bot;

namespace ViberBot.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Add(User user);
        Task<bool> Delete(User user);
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(string id);
    }
}