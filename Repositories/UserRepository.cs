using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ViberBot.Entities;

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

        /// <summary>
        /// Получает список всех пользователей Viber, подписанных на канал
        /// </summary>
        /// <returns>Список пользователей Viber</returns>
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

        /// <summary>
        /// Получает пользователя Viber по его уникальному идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор пользователя Viber</param>
        /// <returns>Пользователь Viber</returns>
        public async Task<User> Get(string id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @id";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    return await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
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

        /// <summary>
        /// Добавляет пользователя Viber в базу данных
        /// </summary>
        /// <param name="user">Пользователь Viber</param>
        /// <returns>Результат добавления</returns>
        public async Task<bool> Add(User user)
        {
            // INJECT POCO Entity
            var sql = "INSERT INTO Users (Id, Name) SET (@Id, @Name)";

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            var sql = "DELETE FROM Users WHERE Id = @id";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    var deletedRows = await connection.ExecuteAsync(sql, new { id });

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