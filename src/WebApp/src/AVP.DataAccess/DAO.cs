using AVP.Models.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AVP.DataAccess
{
    public interface IDAO
    {
        #region incidents
        Task CreateIncidents(List<Incident> incidents);
        Task<List<Incident>> GetAllIncidents();
        #endregion incidents

        #region users
        Task<ApplicationUser> GetUser(string userName);
        Task<ApplicationUser> AddUser(ApplicationUser user);
        Task<ApplicationUser> UpdateUserPassword(ApplicationUser user);
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
        Task<List<UserEmailLocation>> GetUserEmailLocationsForNotification(Notification notification);
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
        Task<List<UserSmsLocation>> GetUserSMSLocationsForNotification(Notification notification);
        Task<UserSmsLocation> GetUserSmsLocationById(int id);
        Task<List<UserSmsLocation>> GetUserSmsLocationsForUser(string userName);
        Task<UserSmsLocation> UpdateUserSmsLocation(UserSmsLocation smsLoc);
        Task<UserSmsLocation> InsertUserSmsLocation(UserSmsLocation smsLoc);
        Task<bool> DeleteUserSmsLocation(UserSmsLocation smsLoc);
        #endregion usersmslocation

        #region subscribers
        Task<List<Subscriber>> GetAllSubscribers();
        Task AddSubscribersToIncident(List<Subscriber> subscribers);
        Task<List<Incident>> GetSubscribersForIncidents(List<Incident> incidents);
        #endregion subscribers

        #region notifications
        Task<List<Notification>> GetAllNotifications();
        Task<Notification> GetNotificationById(int id);
        Task<Notification> InsertNotification(Notification notification);
        Task<Notification> UpdateNotification(Notification notification);
        Task AddNotificationLocations(Notification notification);
        #endregion notifications
    }

    public class DAO : IDAO
    {
        private readonly ILogger _logger;
        public DAO(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DAO>();
        }
        #region notifications
        public async Task AddNotificationLocations(Notification notification)
        {
            await AddUserEmailLocations(notification);
            await AddUserSmsLocations(notification);
            await AddUserPushLocations(notification);
        }

        public async Task<List<Notification>> GetAllNotifications()
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"select * from notification";

                var reader = command.ExecuteReader();

                List<Notification> notifications = new List<Notification>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Notification notification = new Notification()
                        {
                            NotificationID = Convert.ToInt32(reader["NotificationID"]),
                            Message = reader["Message"].ToString(),
                            MessageDateTime = string.IsNullOrEmpty(reader["MessageDateTime"].ToString()) ? DateTime.Now : Convert.ToDateTime(reader["MessageDateTime"].ToString()),
                            SendingUserID = Convert.ToInt32(reader["SendingUserID"]),
                            IncidentID = Convert.ToInt32(reader["IncidentID"])
                        };

                        notifications.Add(notification);

                    }
                }

                return notifications;
            }
        }
        public async Task<Notification> GetNotificationById(int id)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"select * from notification";

                var reader = command.ExecuteReader();

                Notification notification = new Notification();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        notification.NotificationID = Convert.ToInt32(reader["NotificationID"]);
                        notification.Message = reader["Message"].ToString();
                        notification.MessageDateTime = string.IsNullOrEmpty(reader["MessageDateTime"].ToString()) ? DateTime.Now : Convert.ToDateTime(reader["MessageDateTime"].ToString());
                        notification.SendingUserID = Convert.ToInt32(reader["SendingUserID"]);
                        notification.IncidentID = Convert.ToInt32(reader["IncidentID"]);
                    }
                }

                return notification;
            }
        }
        public async Task<Notification> InsertNotification(Notification notification)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO notification (NotificationID, Message, MessageDateTime, SendingUserID, IncidentID)
                                        VALUES (@NotificationID, @Message, @MessageDateTime, @SendingUserID, @IncidentID);
                                        SELECT AUTO_INCREMENT FROM information_schema.TABLES WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = 'notification'; ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@NotificationID", Value = notification.NotificationID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@Message", Value = notification.Message, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@MessageDateTime", Value = notification.MessageDateTime, DbType = System.Data.DbType.DateTime });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@SendingUserID", Value = notification.SendingUserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@IncidentID", Value = notification.IncidentID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@schema", Value = db.Schema, DbType = System.Data.DbType.String });

                int notificationID = Convert.ToInt32(await command.ExecuteScalarAsync()) - 1;

                //get the new address with ID and all
                return await GetNotificationById(notificationID);
            }
        }
        public async Task<Notification> UpdateNotification(Notification notification)
        {

            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"UPDATE notification set Message = @message, MessageDateTime = @messageDateTime, SendingUserID = @SendingUserID, IncidentID = @incidentID WHERE NotificationID = @notificationID";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@message", Value = notification.Message, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@messageDateTime", Value = notification.MessageDateTime, DbType = System.Data.DbType.DateTime });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@SendingUserID", Value = notification.SendingUserID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@incidentID", Value = notification.IncidentID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@notificationID", Value = notification.NotificationID, DbType = System.Data.DbType.Int32 });

                await command.ExecuteNonQueryAsync();

                //get the updated address
                return await GetNotificationById(notification.NotificationID);
            }
        }
        #endregion notifications

        #region subscribers
        public async Task<List<Incident>> GetSubscribersForIncidents(List<Incident> incidents)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();
                //loop through each indicent and populate the respective subscriber data
                foreach (Incident incident in incidents)
                {
                    var command = db.Connection.CreateCommand();
                    command.CommandText = @"select up.UserID, ua.UserAddressID, ua.StreetAddress, ua.City, ua.State, ua.zip, ua.Latitude, ua.Longitude, up.Name, i.id as gisincidentid
                                            from incidentsubscribers isubs 
                                            left join useraddress ua on isubs.useraddressid = ua.useraddressid
                                            left join userprofile up on ua.userid = up.userid
                                            left join incident i on isubs.IncidentID = i.incidentid
                                            where isubs.IncidentID = @incidentId;";

                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@incidentId", Value = incident.IncidentID, DbType = System.Data.DbType.Int32 });

                    var reader = await command.ExecuteReaderAsync();

                    //process the subscribers and add them to the incident list

                    if (reader.HasRows)
                    {
                        incident.Subscribers = new List<Subscriber>();
                        while (reader.Read())
                        {


                            int UserID = 0;
                            int UserAddressID = 0;
                            bool hasUserID = Int32.TryParse(reader["UserID"].ToString(), out UserID);
                            bool hasUserAddressID = Int32.TryParse(reader["UserAddressID"].ToString(), out UserAddressID);
                            if (hasUserID && hasUserAddressID)
                            {
                                Subscriber subscriber = new Subscriber();

                                subscriber.SubscriberId = UserID;
                                subscriber.AddressId = UserAddressID;
                                subscriber.Address = $"{reader["StreetAddress"].ToString()} {reader["City"].ToString()}, {reader["State"].ToString()} {reader["Zip"].ToString()}";
                                subscriber.Lat = Convert.ToDouble(reader["Latitude"]);
                                subscriber.Lon = Convert.ToDouble(reader["Longitude"]);
                                subscriber.Name = reader["Name"].ToString();
                                subscriber.IncidentID = reader["gisincidentid"].ToString();

                                incident.Subscribers.Add(subscriber);
                            }

                        }
                    }
                    reader.Dispose();
                }
            }

            return incidents;
        }

        public async Task AddSubscribersToIncident(List<Subscriber> subscribers)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                foreach (Subscriber subscriber in subscribers)
                {
                    var command = db.Connection.CreateCommand();
                    //insert only where not exists
                    command.CommandText = @"INSERT INTO incidentsubscribers (IncidentID, UserAddressID)
                                            SELECT * FROM  (SELECT (SELECT IncidentID FROM incident WHERE id = @incId LIMIT 1), @userAddressId) AS tmp
                                            WHERE NOT EXISTS (
	                                            SELECT IncidentID, UserAddressID FROM incidentsubscribers WHERE IncidentID = (SELECT IncidentID FROM incident WHERE id = @incId LIMIT 1) AND UserAddressID = @userAddressId
                                            ) LIMIT 1;";

                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@incId", Value = subscriber.IncidentID, DbType = System.Data.DbType.Int32 });
                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressId", Value = subscriber.AddressId, DbType = System.Data.DbType.Int32 });

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Subscriber>> GetAllSubscribers()
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"select up.UserID, ua.UserAddressID, ua.StreetAddress, ua.City, ua.State, ua.zip, ua.Latitude, ua.Longitude, up.Name from userprofile up
                                      left join useraddress ua on up.userid = ua.UserID";

                var reader = command.ExecuteReader();


                List<Subscriber> subscribers = new List<Subscriber>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {


                        int UserID = 0;
                        int UserAddressID = 0;
                        bool hasUserID = Int32.TryParse(reader["UserID"].ToString(), out UserID);
                        bool hasUserAddressID = Int32.TryParse(reader["UserAddressID"].ToString(), out UserAddressID);
                        if (hasUserID && hasUserAddressID)
                        {
                            Subscriber subscriber = new Subscriber();

                            subscriber.SubscriberId = UserID;
                            subscriber.AddressId = UserAddressID;
                            subscriber.Address = $"{reader["StreetAddress"].ToString()} {reader["City"].ToString()}, {reader["State"].ToString()} {reader["Zip"].ToString()}";
                            subscriber.Lat = Convert.ToDouble(reader["Latitude"]);
                            subscriber.Lon = Convert.ToDouble(reader["Longitude"]);
                            subscriber.Name = reader["Name"].ToString();

                            subscribers.Add(subscriber);
                        }

                    }
                }

                return subscribers;
            }
        }
        #endregion subscribers

        #region incidents
        public async Task<Incident> GetIncidentByIdString(string id)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM incident where id = @id LIMIT 1";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@id", Value = id, DbType = System.Data.DbType.String });

                var reader = command.ExecuteReader();

                Incident incident = new Incident();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        incident.Id = reader["id"].ToString();
                        incident.Lat = Convert.ToDouble(reader["Latitude"]);
                        incident.Lon = Convert.ToDouble(reader["Longitude"]);
                        incident.IncidentType = reader["incidenttype"].ToString();
                        incident.IncidentName = reader["incidentname"].ToString();
                        incident.Radius = Convert.ToInt32(reader["incidentradius"]);
                        incident.IncidentID = Convert.ToInt32(reader["incidentid"]);
                    }
                }
                return incident;
            }

        }
        public async Task<List<Incident>> GetAllIncidents()
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT * FROM incident";

                var reader = command.ExecuteReader();

                List<Incident> incidents = new List<Incident>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Incident incident = new Incident()
                        {
                            Id = reader["id"].ToString(),
                            Lat = Convert.ToDouble(reader["Latitude"]),
                            Lon = Convert.ToDouble(reader["Longitude"]),
                            IncidentType = reader["incidenttype"].ToString(),
                            IncidentName = reader["incidentname"].ToString(),
                            Radius = Convert.ToInt32(reader["incidentradius"]),
                            IncidentID = Convert.ToInt32(reader["incidentid"])
                        };

                        incidents.Add(incident);
                    }
                }

                return incidents;
            }
        }
        public async Task CreateIncidents(List<Incident> incidents)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                foreach (Incident incident in incidents)
                {
                    var command = db.Connection.CreateCommand();
                    command.CommandText = @"INSERT INTO incident (id, incidentname, latitude, longitude, incidenttype, incidentradius)
                                        VALUES (@id, @incidentname, @latitude, @longitude, @incidenttype, @incidentradius);";

                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@id", Value = incident.Id, DbType = System.Data.DbType.String });
                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@incidentname", Value = incident.IncidentName, DbType = System.Data.DbType.String });

                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@latitude", Value = incident.Lat, DbType = System.Data.DbType.Double });
                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@longitude", Value = incident.Lon, DbType = System.Data.DbType.Double });


                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@incidentradius", Value = incident.Radius, DbType = System.Data.DbType.Int32 });
                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@incidenttype", Value = incident.IncidentType, DbType = System.Data.DbType.String });

                    int incidentCount = await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion incidents

        #region usersmslocation
        public async Task<List<UserSmsLocation>> GetUserSMSLocationsForNotification(Notification notification)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT usl.usersmslocationid, usl.userid, usl.phonenumber, usl.useraddressid FROM usersmslocation usl left join notificationsmslocation nsl on usl.usersmslocationid = nsl.usersmslocationid WHERE nsl.notificationid = @notificationId";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@notificationId", Value = notification.NotificationID, DbType = System.Data.DbType.Int32 });

                var reader = command.ExecuteReader();


                List<UserSmsLocation> userLocs = new List<UserSmsLocation>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserSmsLocation smsLoc = new UserSmsLocation()
                        {
                            UserSmsLocationID = Convert.ToInt32(reader["UserSmsLocationID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            PhoneNumber = Convert.ToInt64(reader["PhoneNumber"]),
                            UserAddressID = Convert.ToInt32(reader["UserAddressID"])
                        };

                        userLocs.Add(smsLoc);
                    }
                }

                return userLocs;
            }
        }
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
                        smsLoc.UserSmsLocationID = Convert.ToInt32(reader["UserSmsLocationID"]);
                        smsLoc.UserID = Convert.ToInt32(reader["UserID"]);
                        smsLoc.PhoneNumber = Convert.ToInt64(reader["PhoneNumber"]);
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
                            PhoneNumber = Convert.ToInt64(reader["PhoneNumber"]),
                            UserAddressID = Convert.ToInt32(reader["UserAddressID"])
                        };

                        smsLocs.Add(smsLoc);
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

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@phoneNumber", Value = smsLoc.PhoneNumber, DbType = System.Data.DbType.Int64 });
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

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@phoneNumber", Value = smsLoc.PhoneNumber, DbType = System.Data.DbType.Int64 });
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
                        pushLoc.PhoneNumber = Convert.ToInt64(reader["PhoneNumber"]);
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
                            PhoneNumber = Convert.ToInt64(reader["PhoneNumber"]),
                            UserAddressID = Convert.ToInt32(reader["UserAddressID"])
                        };

                        pushLocs.Add(pushLoc);
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

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@phoneNumber", Value = pushLoc.PhoneNumber, DbType = System.Data.DbType.Int64 });
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

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@phoneNumber", Value = pushLoc.PhoneNumber, DbType = System.Data.DbType.Int64 });
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
        public async Task<List<UserEmailLocation>> GetUserEmailLocationsForNotification(Notification notification)
        {
            using (var db = new DBConnection())
            {

                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"SELECT uel.useremaillocationid, uel.userid, uel.emailaddress, uel.useraddressid FROM useremaillocation uel left join notificationemaillocation nsl on uel.useremaillocationid = nsl.useremaillocationid WHERE nsl.notificationid = @notificationId";
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@notificationId", Value = notification.NotificationID, DbType = System.Data.DbType.Int32 });

                var reader = command.ExecuteReader();


                List<UserEmailLocation> userLocs = new List<UserEmailLocation>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            UserEmailLocation emailLoc = new UserEmailLocation();

                            emailLoc.UserEmailLocationID = Convert.ToInt32(reader["UserEmailLocationID"]);
                            emailLoc.UserID = Convert.ToInt32(reader["UserID"]);
                            emailLoc.EmailAddress = reader["EmailAddress"].ToString();
                            emailLoc.UserAddressID = Convert.ToInt32(reader["UserAddressID"]);
                            userLocs.Add(emailLoc);
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation($"Unable to read UserEmailLocation. Exception message is: {e.Message}");
                        }

                        
                    }
                }

                return userLocs;
            }
        }
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

                        emailLocs.Add(emailLoc);
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
                        address.UserAddressID = Convert.ToInt32(reader["UserAddressID"]);
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
                            UserAddressID = Convert.ToInt32(reader["UserAddressID"]),
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

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = address.UserAddressID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userID", Value = address.UserID, DbType = System.Data.DbType.Int32 });

                await command.ExecuteNonQueryAsync();

                //get the updated address
                return await GetAddressById(address.UserAddressID);
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
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userAddressID", Value = address.UserAddressID, DbType = System.Data.DbType.Int32 });

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
                        userProfile.Name = reader["Name"].ToString();
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
                command.CommandText = @"UPDATE userprofile SET EmailOptIn = @emailOptIn, Smsoptin = @smsOptIn, Pushoptin = @pushOptIn, Name = @Name WHERE Username = @userName ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = profile.UserName, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@Name", Value = profile.Name, DbType = System.Data.DbType.String });
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
        public async Task<ApplicationUser> UpdateUserPassword(ApplicationUser user)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();
                var command = db.Connection.CreateCommand();

                command.CommandText = @"UPDATE userprofile SET PasswordHash = @passwordHash WHERE Username = @userName";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = user.UserName, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@passwordHash", Value = user.PasswordHash, DbType = System.Data.DbType.String });

                var reader = await command.ExecuteNonQueryAsync();

                return await GetUser(user.UserName);
            }
        }

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
                        user.Name = reader["Name"].ToString();
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
                command.CommandText = @"INSERT INTO userprofile (Username, EmailOptIn, Smsoptin, Pushoptin, PasswordHash, Name) 
                                        VALUES (@userName, @emailOptIn, @smsOptIn, @pushOptIn, @passwordHash, @Name) ";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@userName", Value = user.UserName, DbType = System.Data.DbType.String });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@Name", Value = user.Name, DbType = System.Data.DbType.String });
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

        #region notification location helpers
        public async Task AddUserEmailLocations(Notification notification)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO notificationemaillocation (NotificationID, UserEmailLocationID)
                                        SELECT @notificationId, uel.UserEmailLocationID from useremaillocation uel where uel.UserAddressID in (select incs.UserAddressID from incidentsubscribers incs left join useraddress ua on incs.UserAddressId = ua.UserAddressId left join userprofile up on ua.UserID = up.UserID  where up.EmailOptIn = true AND incidentid = @incidentId)
                                        ON DUPLICATE KEY UPDATE UserEmailLocationID = uel.UserEmailLocationID;";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@notificationId", Value = notification.NotificationID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@incidentId", Value = notification.IncidentID, DbType = System.Data.DbType.Int32 });

                var reader = await command.ExecuteNonQueryAsync();

            }
        }

        public async Task AddUserSmsLocations(Notification notification)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO notificationsmslocation (NotificationID, UserSmsLocationID)
                                        SELECT @notificationId, usl.UserSmsLocationID from usersmslocation usl where usl.UserAddressID in (select incs.UserAddressID from incidentsubscribers incs left join useraddress ua on incs.UserAddressId = ua.UserAddressId left join userprofile up on ua.UserID = up.UserID  where up.SmsOptIn = true AND incidentid = @incidentId)
                                        ON DUPLICATE KEY UPDATE UserSmsLocationID = usl.UserSmsLocationID;";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@notificationId", Value = notification.NotificationID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@incidentId", Value = notification.IncidentID, DbType = System.Data.DbType.Int32 });

                var reader = await command.ExecuteNonQueryAsync();

            }
        }

        public async Task AddUserPushLocations(Notification notification)
        {
            using (var db = new DBConnection())
            {
                await db.Connection.OpenAsync();

                var command = db.Connection.CreateCommand();
                command.CommandText = @"INSERT INTO notificationpushlocation (NotificationID, UserPushLocationID)
                                        SELECT @notificationId, upl.UserPushLocationID from userpushlocation upl where upl.UserAddressID in (select incs.UserAddressID from incidentsubscribers incs left join useraddress ua on incs.UserAddressId = ua.UserAddressId left join userprofile up on ua.UserID = up.UserID  where up.PushOptIn = true AND incidentid = @incidentId)
                                        ON DUPLICATE KEY UPDATE UserPushLocationID = upl.UserPushLocationID;";

                command.Parameters.Add(new MySqlParameter() { ParameterName = "@notificationId", Value = notification.NotificationID, DbType = System.Data.DbType.Int32 });
                command.Parameters.Add(new MySqlParameter() { ParameterName = "@incidentId", Value = notification.IncidentID, DbType = System.Data.DbType.Int32 });

                var reader = await command.ExecuteNonQueryAsync();

            }
        }
        #endregion notification location helpers
    }
}
