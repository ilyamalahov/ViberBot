using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Viber.Bot;

namespace ViberBot.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(string connectionString, IServiceProvider provider)
        {
            this.connectionString = connectionString;
            this.logger = provider.GetRequiredService<ILogger<UserRepository>>();
        }

        public async Task<User> Get(string userId)
        {
            var sql = "SELECT * FROM Users WHERE UserId = @userId";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    return await connection.QuerySingleOrDefaultAsync<User>(sql, new { userId });
                }
            }
            catch (SqliteException ex)
            {
                logger.LogError("Database error: {ex.Message}", ex.Message);

                return null;
            }
            catch (Exception ex)
            {
                logger.LogError("Error: {ex.Message}", ex.Message);

                return null;
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var sql = "SELECT * FROM Users";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    return await connection.QueryAsync<User>(sql);
                }
            }
            catch (SqliteException ex)
            {
                logger.LogError("Database error: {ex.Message}", ex.Message);

                return null;
            }
            catch (Exception ex)
            {
                logger.LogError("Error: {ex.Message}", ex.Message);

                return null;
            }
        }

        public async Task<bool> Add(User user)
        {
            // INJECT POCO Entity
            var sql = "INSERT INTO Users (UserId) SET (@UserId)";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    var insertedRows = await connection.ExecuteAsync(sql, user);

                    return insertedRows > 0;
                }
            }
            catch (SqliteException ex)
            {
                logger.LogError("Database error: {ex.Message}", ex.Message);

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("Error: {ex.Message}", ex.Message);

                return false;
            }
        }

        public async Task<bool> Delete(string userId)
        {
            var sql = "DELETE FROM Users WHERE UserId = @userId";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    var deletedRows = await connection.ExecuteAsync(sql, new { userId });

                    return deletedRows > 0;
                }
            }
            catch (SqliteException ex)
            {
                logger.LogError("Database error: {ex.Message}", ex.Message);

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("Error: {ex.Message}", ex.Message);

                return false;
            }
        }
    }
}