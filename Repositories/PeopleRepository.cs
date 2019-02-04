using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dapper;
using ViberBot.Entities;
using ViberBot.Models;

namespace ViberBot.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly string connectionString;

        public PeopleRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<People> GetPeopleByIdAsync(string userId)
        {
            var peopleSelectSql = @"SELECT p.*
                                    FROM dbo.People p
                                    INNER JOIN dbo.PeopleLinkContact plc ON p.Id = plc.PeopleId
                                    INNER JOIN dbo.PeopleContact pc ON plc.ContactId = pc.Id
                                    WHERE pc.InfoTextId = @userId";

            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<People>(peopleSelectSql, new { userId });
            }
        }

        public async Task<People> GetOrAddPeopleAsync(string userId, string userName, string userAvatarUrl)
        {
            var peopleSelectSql = @"SELECT p.*
                                    FROM dbo.People p
                                    INNER JOIN dbo.PeopleLinkContact plc ON p.Id = plc.PeopleId
                                    INNER JOIN dbo.PeopleContact pc ON plc.ContactId = pc.Id
                                    WHERE pc.InfoTextId = @userId AND pc.InfoText = @userName";

            using (var connection = new SqlConnection(connectionString))
            {
                var people = await connection.QueryFirstOrDefaultAsync<People>(peopleSelectSql, new { userId, userName });

                return people ?? await InsertPeople(userId, userName, userAvatarUrl);
            }
        }

        public async Task UpdateContactServiceStateAsync(string userId, ServiceState state)
        {
            var updateSql = @"UPDATE PeopleContact SET ContactServiceStateId = @stateId WHERE InfoTextId = @userId";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(updateSql, new { userId, stateId = (int)state });
            }
        }

        private async Task<People> InsertPeople(string userId, string userName, string userAvatarUrl)
        {
            var contactInsertSql = @"   DECLARE @TempPeopleContact table(NewContactId bigint);

                                        INSERT INTO dbo.PeopleContact (ContactTypeId, InfoTextId, InfoText, ContactServiceStateId)
                                        OUTPUT INSERTED.Id INTO @TempPeopleContact
                                        VALUES(@ContactTypeId, @InfoTextId, @InfoText, @ContactServiceStateId);

                                        SELECT NewContactId FROM @TempPeopleContact;";

            var peopleInsertSql = @"DECLARE @TempPeople table(NewPeopleID uniqueidentifier);

                                    INSERT INTO People (F, I, O, XmlInfo)
                                    OUTPUT INSERTED.Id INTO @TempPeople
                                    VALUES (@F, @I, @O, @XmlInfo);  

                                    SELECT NewPeopleID FROM @TempPeople;";

            var peopleContactInsertSql = @"INSERT INTO PeopleLinkContact (PeopleId, ContactId, Remark) VALUES (@peopleId, @contactId, '')";

            using (var connection = new SqlConnection(connectionString))
            {
                // Open database connection
                await connection.OpenAsync();

                // Begin insert transaction
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // New contact object
                        var contact = new PeopleContact
                        {
                            InfoTextId = userId,
                            InfoText = userName,
                            ContactTypeId = (int)ContactType.Phone,
                            ContactServiceStateId = (int)ServiceState.ConversationStarted
                        };

                        // Insert contact
                        var contactId = await connection.ExecuteScalarAsync<long>(contactInsertSql, contact, transaction);

                        // New people object
                        var people = new People
                        {
                            F = userName,
                            I = "",
                            O = "",
                            XmlInfo = XElement.Parse($"<img>{userAvatarUrl}</img>")
                        };

                        // Insert people
                        var peopleId = await connection.ExecuteScalarAsync<Guid>(peopleInsertSql, people, transaction);

                        // Insert contact people link
                        await connection.ExecuteAsync(peopleContactInsertSql, new { peopleId, contactId }, transaction);

                        // Commit insert transaction
                        transaction.Commit();

                        // 
                        people.Id = peopleId;

                        // 
                        return people;
                    }
                    catch (Exception)
                    {
                        // Rollback transaction due to database error
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }
    }

    public interface IPeopleRepository
    {
        Task<People> GetPeopleByIdAsync(string userId);
        Task<People> GetOrAddPeopleAsync(string userId, string userName, string userAvatarUrl);
        Task UpdateContactServiceStateAsync(string userId, ServiceState state);
    }
}

namespace ViberBot.Models
{
    public enum ServiceState
    {
        ConversationStarted = 1,
        Subscribed = 2,
        Unsubscribed = 3
    }

    public enum ContactType
    {
        Phone = 1,
        Email = 2,
        Instagram = 3,
        Viber = 4,
        WhatsApp = 5,
        Vk = 6
    }
}