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
        Task<UserProfile> GetProfileForUserName(string userName);
        Task<UserProfile> UpdateUserProfile(UserProfile profile);
        #endregion profiles

        #region useraddress
        Task<UserAddress> GetAddressById(int id);
        Task<List<UserAddress>> GetAddressesForUser(string userName);
        Task<UserAddress> UpdateUserAddress(UserAddress address);
        Task<UserAddress> InsertUserAddress(UserAddress address);
        Task<bool> DeleteUserAddress(UserAddress address);
        #endregion useraddress

        #region useremaillocation
        Task<UserEmailLocation> GetUserEmailLocationById(int id);
        Task<List<UserEmailLocation>> GetUserEmailLocationsForUser(string userName);
        Task<UserEmailLocation> UpdateUserEmailLocation(UserEmailLocation emailLoc);
        Task<UserEmailLocation> InsertUserEmailLocation(UserEmailLocation emailLoc);
        Task<bool> DeleteUserEmailLocation(UserEmailLocation emailLoc);
        #endregion useremaillocation

        #region userpushlocation
        Task<UserPushLocation> GetUserPushLocationById(int id);
        Task<List<UserPushLocation>> GetUserPushLocationsForUser(string userName);
        Task<UserPushLocation> UpdateUserPushLocation(UserPushLocation pushLoc);
        Task<UserPushLocation> InsertUserPushLocation(UserPushLocation pushLoc);
        Task<bool> DeleteUserPushLocation(UserPushLocation pushLoc);
        #endregion userpushlocation

        #region usersmslocation
        Task<UserSmsLocation> GetUserSmsLocationById(int id);
        Task<List<UserSmsLocation>> GetUserSmsLocationsForUser(string userName);
        Task<UserSmsLocation> UpdateUserSmsLocation(UserSmsLocation smsLoc);
        Task<UserSmsLocation> InsertUserSmsLocation(UserSmsLocation smsLoc);
        Task<bool> DeleteUserSmsLocation(UserSmsLocation smsLoc);
        #endregion usersmslocation
    }

    public class DAO : IDAO
    {
        #region usersmslocation
        public async Task<UserSmsLocation> GetUserSmsLocationById(int id)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM usersmslocation WHERE UsersmsLocationID = @id LIMIT 1";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@id", Value = id, DbType = System.Data.DbType.Int32 });

                var reader = command.ExecuteReader();


                UserSmsLocation smsLoc = new UserSmsLocation();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        smsLoc.UserSmsLocationID = Convert.ToInt32(reader["UserPushLocationID"]);
                        smsLoc.UserID = Convert.ToInt32(reader["UserID"]);
                        smsLoc.PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]);
                        smsLoc.UserAddressID = Convert.ToInt32(reader["UserAddressID"]);
                    }
                }

                return smsLoc;
            }
        }
        public async Task<List<UserSmsLocation>> GetUserSmsLocationsForUser(string userName)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM usersmslocation WHERE UserID = (SELECT UserID FROM userprofile WHERE Username = @userName)";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = userName, DbType = System.Data.DbType.String });

                var reader = command.ExecuteReader();

                List<UserSmsLocation> smsLocs = new List<UserSmsLocation>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserSmsLocation smsLoc = new UserSmsLocation()
                        {
                            UserSmsLocationID = Convert.ToInt32(reader["UserSmsLocationID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]),
                            UserAddressID = Convert.ToInt32(reader["UserAddressID"])
                        };
                    }
                }

                return smsLocs;
            }
        }
        public async Task<UserSmsLocation> UpdateUserSmsLocation(UserSmsLocation smsLoc)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"UPDATE usersmslocation set PhoneNumber = @phoneNumber, UserAddressID = @userAddressID WHERE UserSmsLocationID = @userSmsLocationID AND UserID = @userID";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@phoneNumber", Value = smsLoc.PhoneNumber, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = smsLoc.UserAddressID, DbType = System.Data.DbType.Int32 });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userSmsLocationID", Value = smsLoc.UserSmsLocationID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = smsLoc.UserID, DbType = System.Data.DbType.Int32 });

                await command.ExecuteNonQueryAsync();

                //get the updated address
                return await GetUserSmsLocationById(smsLoc.UserSmsLocationID);
            }
        }
        public async Task<UserSmsLocation> InsertUserSmsLocation(UserSmsLocation smsLoc)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO usersmslocation (UserID, PhoneNumber, UserAddressID)
                                        VALUES (@userID, @phoneNumber, @userAddressID);
                                        SELECT AUTO_INCREMENT FROM information_schema.TABLES WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = 'usersmslocation'; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@phoneNumber", Value = smsLoc.PhoneNumber, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = smsLoc.UserAddressID, DbType = System.Data.DbType.String });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = smsLoc.UserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@schema", Value = db.Schema, DbType = System.Data.DbType.String });

                int smsLocID = Convert.ToInt32(await command.ExecuteScalarAsync()) - 1;

                //get the new address with ID and all
                return await GetUserSmsLocationById(smsLocID);
            }
        }
        public async Task<bool> DeleteUserSmsLocation(UserSmsLocation smsLoc)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"DELETE FROM usersmslocation WHERE UserID = @userID AND UserSmsLocationID = @userSmsLocationID; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = smsLoc.UserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userSmsLocationID", Value = smsLoc.UserSmsLocationID, DbType = System.Data.DbType.Int32 });

                int rows = await command.ExecuteNonQueryAsync();

                if (rows > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion usersmslocation


        #region userepushlocation
        public async Task<UserPushLocation> GetUserPushLocationById(int id)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM userpushlocation WHERE UserPushLocationID = @id LIMIT 1";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@id", Value = id, DbType = System.Data.DbType.Int32 });

                var reader = command.ExecuteReader();


                UserPushLocation pushLoc = new UserPushLocation();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pushLoc.UserPushLocationID = Convert.ToInt32(reader["UserPushLocationID"]);
                        pushLoc.UserID = Convert.ToInt32(reader["UserID"]);
                        pushLoc.PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]);
                        pushLoc.UserAddressID = Convert.ToInt32(reader["UserAddressID"]);
                    }
                }

                return pushLoc;
            }
        }
        public async Task<List<UserPushLocation>> GetUserPushLocationsForUser(string userName)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM userpushlocation WHERE UserID = (SELECT UserID FROM userprofile WHERE Username = @userName)";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = userName, DbType = System.Data.DbType.String });

                var reader = command.ExecuteReader();

                List<UserPushLocation> pushLocs = new List<UserPushLocation>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserPushLocation pushLoc = new UserPushLocation()
                        {
                            UserPushLocationID = Convert.ToInt32(reader["UserPushLocationID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]),
                            UserAddressID = Convert.ToInt32(reader["UserAddressID"])
                        };
                    }
                }

                return pushLocs;
            }
        }
        public async Task<UserPushLocation> UpdateUserPushLocation(UserPushLocation pushLoc)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"UPDATE userpushlocation set PhoneNumber = @phoneNumber, UserAddressID = @userAddressID WHERE UserPushLocationID = @userPushLocationID AND UserID = @userID";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@phoneNumber", Value = pushLoc.PhoneNumber, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = pushLoc.UserAddressID, DbType = System.Data.DbType.Int32 });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userPushLocationID", Value = pushLoc.UserPushLocationID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = pushLoc.UserID, DbType = System.Data.DbType.Int32 });

                await command.ExecuteNonQueryAsync();

                //get the updated address
                return await GetUserPushLocationById(pushLoc.UserPushLocationID);
            }
        }
        public async Task<UserPushLocation> InsertUserPushLocation(UserPushLocation pushLoc)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO userpushlocation (UserID, PhoneNumber, UserAddressID)
                                        VALUES (@userID, @phoneNumber, @userAddressID);
                                        SELECT AUTO_INCREMENT FROM information_schema.TABLES WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = 'userpushlocation'; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@phoneNumber", Value = pushLoc.PhoneNumber, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = pushLoc.UserAddressID, DbType = System.Data.DbType.String });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = pushLoc.UserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@schema", Value = db.Schema, DbType = System.Data.DbType.String });

                int pushLocID = Convert.ToInt32(await command.ExecuteScalarAsync()) - 1;

                //get the new address with ID and all
                return await GetUserPushLocationById(pushLocID);
            }
        }
        public async Task<bool> DeleteUserPushLocation(UserPushLocation pushLoc)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"DELETE FROM userpushlocation WHERE UserID = @userID AND UserPushLocationID = @userPushLocationID; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = pushLoc.UserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userPushLocationID", Value = pushLoc.UserPushLocationID, DbType = System.Data.DbType.Int32 });

                int rows = await command.ExecuteNonQueryAsync();

                if (rows > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion userpushlocation


        #region useremaillocation
        public async Task<UserEmailLocation> GetUserEmailLocationById(int id)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM useremaillocation WHERE UserEmailLocationID = @id LIMIT 1";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@id", Value = id, DbType = System.Data.DbType.Int32 });

                var reader = command.ExecuteReader();


                UserEmailLocation emailLoc = new UserEmailLocation();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        emailLoc.UserEmailLocationID = Convert.ToInt32(reader["UserEmailLocationID"]);
                        emailLoc.UserID = Convert.ToInt32(reader["UserID"]);
                        emailLoc.EmailAddress = reader["EmailAddress"].ToString();
                        emailLoc.UserAddressID = Convert.ToInt32(reader["UserAddressID"]);
                    }
                }

                return emailLoc;
            }
        }
        public async Task<List<UserEmailLocation>> GetUserEmailLocationsForUser(string userName)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM useremaillocation WHERE UserID = (SELECT UserID FROM userprofile WHERE Username = @userName)";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = userName, DbType = System.Data.DbType.String });

                var reader = command.ExecuteReader();

                List<UserEmailLocation> emailLocs = new List<UserEmailLocation>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserEmailLocation emailLoc = new UserEmailLocation()
                        {
                            UserEmailLocationID = Convert.ToInt32(reader["UserEmailLocationID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            UserAddressID = Convert.ToInt32(reader["UserAddressID"])
                        };
                    }
                }

                return emailLocs;
            }
        }
        public async Task<UserEmailLocation> UpdateUserEmailLocation(UserEmailLocation emailLoc)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"UPDATE useremaillocation set EmailAddress = @emailAddress, UserAddressID = @userAddressID WHERE UserEmailLocationID = @userEmailLocationID AND UserID = @userID";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@emailAddress", Value = emailLoc.EmailAddress, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = emailLoc.UserAddressID, DbType = System.Data.DbType.Int32 });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userEmailLocationID", Value = emailLoc.UserEmailLocationID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = emailLoc.UserID, DbType = System.Data.DbType.Int32 });

                await command.ExecuteNonQueryAsync();

                //get the updated address
                return await GetUserEmailLocationById(emailLoc.UserEmailLocationID);
            }
        }
        public async Task<UserEmailLocation> InsertUserEmailLocation(UserEmailLocation emailLoc)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO useremaillocation (UserID, EmailAddress, UserAddressID)
                                        VALUES (@userID, @emailAddress, @userAddressID);
                                        SELECT AUTO_INCREMENT FROM information_schema.TABLES WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = 'useremaillocation'; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@emailAddress", Value = emailLoc.EmailAddress, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = emailLoc.UserAddressID, DbType = System.Data.DbType.String });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = emailLoc.UserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@schema", Value = db.Schema, DbType = System.Data.DbType.String });

                int emailLocID = Convert.ToInt32(await command.ExecuteScalarAsync()) - 1;

                //get the new address with ID and all
                return await GetUserEmailLocationById(emailLocID);
            }
        }
        public async Task<bool> DeleteUserEmailLocation(UserEmailLocation emailLoc)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"DELETE FROM useremaillocation WHERE UserID = @userID AND UserEmailLocationID = @userEmailLocationID; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = emailLoc.UserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userEmailLocationID", Value = emailLoc.UserEmailLocationID, DbType = System.Data.DbType.Int32 });

                int rows = await command.ExecuteNonQueryAsync();

                if (rows > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion useremaillocation

        #region useraddress
        public async Task<UserAddress> GetAddressById(int id)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM useraddress WHERE UserAddressID = @id LIMIT 1";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@id", Value = id, DbType = System.Data.DbType.Int32 });

                var reader = command.ExecuteReader();


                UserAddress address = new UserAddress();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        address.UserAdressID = Convert.ToInt32(reader["UserAddressID"]);
                        address.UserID = Convert.ToInt32(reader["UserID"]);
                        address.StreetAddress = reader["StreetAddress"].ToString();
                        address.City = reader["City"].ToString();
                        address.State = reader["State"].ToString();
                        address.Zip = Convert.ToInt32(reader["Zip"]);
                        address.Latitude = Convert.ToDouble(reader["Latitude"]);
                        address.Longitude = Convert.ToDouble(reader["Longitude"]);
                    }
                }

                return address;
            }
        }

        public async Task<List<UserAddress>> GetAddressesForUser(string userName)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM useraddress WHERE UserID = (SELECT UserID FROM userprofile WHERE Username = @userName)";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = userName, DbType = System.Data.DbType.String });

                var reader = command.ExecuteReader();


                List<UserAddress> addresses = new List<UserAddress>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserAddress address = new UserAddress()
                        {
                            UserAdressID = Convert.ToInt32(reader["UserAddressID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            StreetAddress = reader["StreetAddress"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            Zip = Convert.ToInt32(reader["Zip"]),
                            Latitude = Convert.ToDouble(reader["Latitude"]),
                            Longitude = Convert.ToDouble(reader["Longitude"])
                        };
                        addresses.Add(address);
                    }
                }

                return addresses;
            }
        }

        public async Task<UserAddress> UpdateUserAddress(UserAddress address)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"UPDATE useraddress set StreetAddress = @streetAddress, City = @city, State = @state, Zip = @zip, Latitude = @latitude, Longitude = @longitude WHERE UserAddressID = @userAddressID AND UserID = @userID";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@streetAddress", Value = address.StreetAddress, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@city", Value = address.City, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@state", Value = address.State, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@zip", Value = address.Zip, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@latitude", Value = address.Latitude, DbType = System.Data.DbType.Double });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@longitude", Value = address.Longitude, DbType = System.Data.DbType.Double });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = address.UserAdressID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = address.UserID, DbType = System.Data.DbType.Int32 });

                await command.ExecuteNonQueryAsync();

                //get the updated address
                return await GetAddressById(address.UserAdressID);
            }
        }

        public async Task<UserAddress> InsertUserAddress(UserAddress address)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO useraddress (UserID, StreetAddress, City, State, Zip, Latitude, Longitude)
                                        VALUES (@userID, @streetAddress, @city, @state, @zip, @latitude, @longitude);
                                        SELECT AUTO_INCREMENT FROM information_schema.TABLES WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = 'useraddress'; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@streetAddress", Value = address.StreetAddress, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@city", Value = address.City, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@state", Value = address.State, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@zip", Value = address.Zip, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@latitude", Value = address.Latitude, DbType = System.Data.DbType.Double });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@longitude", Value = address.Longitude, DbType = System.Data.DbType.Double });

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = address.UserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@schema", Value = db.Schema, DbType = System.Data.DbType.String });

                int addressID = Convert.ToInt32(await command.ExecuteScalarAsync()) - 1;

                //get the new address with ID and all
                return await GetAddressById(addressID);
            }
        }

        public async Task<bool> DeleteUserAddress(UserAddress address)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"DELETE FROM useraddress WHERE UserID = @userID AND UserAddressID = @userAddressID; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = address.UserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = address.UserAdressID, DbType = System.Data.DbType.Int32 });
                
                int rows = await command.ExecuteNonQueryAsync();

                if (rows > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion useraddress

        #region profiles
        public async Task<UserProfile> GetProfileForUserName(string userName)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM userprofile WHERE Username = @UserName LIMIT 1";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@UserName", Value = userName, DbType = System.Data.DbType.String });

                var reader = command.ExecuteReader();


                UserProfile userProfile = new UserProfile();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userProfile.UserID = Convert.ToInt32(reader["UserID"]);
                        userProfile.UserName = reader["Username"].ToString();
                        userProfile.EmailOptIn = Convert.ToBoolean(reader["EmailOptIn"]);
                        userProfile.SmsOptIn = Convert.ToBoolean(reader["SmsOptIn"]);
                        userProfile.PushOptIn = Convert.ToBoolean(reader["PushOptIn"]);
                    }
                }

                return userProfile;
            }
        }

        public async Task<UserProfile> UpdateUserProfile(UserProfile profile)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"UPDATE userprofile SET EmailOptIn = @emailOptIn, Smsoptin = @smsOptIn, Pushoptin = @pushOptIn WHERE Username = @userName ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = profile.UserName, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@emailOptIn", Value = profile.EmailOptIn, DbType = System.Data.DbType.Boolean });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@smsOptIn", Value = profile.SmsOptIn, DbType = System.Data.DbType.Boolean });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@pushOptIn", Value = profile.PushOptIn, DbType = System.Data.DbType.Boolean });

                var reader = await command.ExecuteNonQueryAsync();

                //get the new user with ID and all
                return await GetProfileForUserName(profile.UserName);
            }
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
