using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using ViberBot.Entities;

namespace ViberBot.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(string id, string name, string avatar);
    }

    public class UserRepository
    {
        private readonly string connectionString;

        public UserRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<User> Get(string id, string name, string avatar)
        {
            var selectSql = @"SELECT * FROM Users WHERE Id = @id AND Name = @name";

            var insertSql = @"INSERT INTO Users (Id, Name, Avatar) VALUES (@id, @name, @avatar)";

            using (var connection = new SqlConnection(connectionString))
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(selectSql, new { id, name });

                if(user == null)
                {
                    var insertedRows = await connection.ExecuteAsync(insertSql, new { id, name, avatar });

                    if(insertedRows == 0)
                    {
                        return null;
                    }
                    
                    user = await connection.QueryFirstOrDefaultAsync<User>(selectSql, new { id, name });
                }

                return user;
            }
        }    
    }
}

namespace ViberBot.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}