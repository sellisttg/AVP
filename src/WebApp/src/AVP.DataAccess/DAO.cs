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

        #region profiles
        Task<UserProfile> GetProfileForUserID(int UserID);
        Task<UserProfile> UpdateUserProfile(UserProfile profile);
        #endregion profiles
    }

    public class DAO : IDAO
    {
        #region profiles
        public async Task<UserProfile> GetProfileForUserID(int UserID)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM userprofile WHERE UserID = @UserID LIMIT 1";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@UserID", Value = UserID, DbType = System.Data.DbType.Int32 });

                var reader = command.ExecuteReader();


                UserProfile userProfile = new UserProfile();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userProfile.UserID = Convert.ToInt32(reader["UserID"]);
                        userProfile.UserName = reader["Username"].ToString();
                        userProfile.EmailOptIn = Convert.ToBoolean(reader["EmailOptIn"]);
                        userProfile.EmailOptIn = Convert.ToBoolean(reader["SmsOptIn"]);
                        userProfile.EmailOptIn = Convert.ToBoolean(reader["PushOptIn"]);
                    }
                }

                return userProfile;
            }
        }

        public async Task<UserProfile> UpdateUserProfile(UserProfile profile)
        {
            UserProfile userProfile = new UserProfile();
            return userProfile;
        }
        #endregion profiles

        #region users
        public async Task<ApplicationUser> GetUser(string userName)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM userprofile WHERE Username = @userName LIMIT 1";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = userName, DbType = System.Data.DbType.String });

                var reader = command.ExecuteReader();


                ApplicationUser user = new ApplicationUser();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.UserID = Convert.ToInt32(reader["UserID"]);
                        user.UserName = reader["Username"].ToString();
                        user.PasswordHash = reader["PasswordHash"].ToString();
                        user.EmailOptIn = Convert.ToBoolean(reader["EmailOptIn"]);
                        user.EmailOptIn = Convert.ToBoolean(reader["SmsOptIn"]);
                        user.EmailOptIn = Convert.ToBoolean(reader["PushOptIn"]);
                    }
                }

                return user;
            }
        }

        public async Task<ApplicationUser> AddUser(ApplicationUser user)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO userprofile (Username, EmailOptIn, Smsoptin, Pushoptin, PasswordHash) 
                                        VALUES (@userName, @emailOptIn, @smsOptIn, @pushOptIn, @passwordHash) ";

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
