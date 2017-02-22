using AVP.Models.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.DataAccess
{
    public interface IDAO
    {
        #region users
        Task<ApplicationUser> GetUser(string userName);
        Task<ApplicationUser> AddUser(ApplicationUser user);

        #endregion users
    }

    public class DAO : IDAO
    {
        #region users
        public async Task<ApplicationUser> GetUser(string userName)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"select * from userprofile where Username = @userName";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = userName, DbType = System.Data.DbType.String });

                var reader = command.ExecuteReader();

                return new ApplicationUser();
            }
        }

        public async Task<ApplicationUser> AddUser(ApplicationUser user)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"insert into userprofile (Username, EmailOptIn, Smsoptin, Pushoptin, PasswordHash) 
                                        values (@userName, @emailOptIn, @smsOptIn, @pushOptIn, @passwordHash) ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = user.UserName, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@passwordHash", Value = user.PasswordHash, DbType = System.Data.DbType.String });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@emailOptIn", Value = user.EmailOptIn, DbType = System.Data.DbType.Boolean });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@smsOptIn", Value = user.SMSOptIn, DbType = System.Data.DbType.Boolean });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@pushOptIn", Value = user.PushOptIn, DbType = System.Data.DbType.Boolean });

                var reader = await command.ExecuteNonQueryAsync();

                //get the new user with ID and all
                return await GetUser(user.UserName);
            }
        }


        #endregion users
    }
}
