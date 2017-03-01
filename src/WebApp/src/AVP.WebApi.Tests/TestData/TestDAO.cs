using AVP.DataAccess;
using AVP.Models.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Tests.TestData
{
    public class TestDAO : IDAO
    {
        //private ConcurrentDictionary<int, UserProfile> _profiles = new ConcurrentDictionary<int, UserProfile>();
        //private static ConcurrentDictionary<int, UserAddress> _addresses = new ConcurrentDictionary<int, UserAddress>();
        //private static ConcurrentDictionary<int, UserEmailLocation> _emailLocations = new ConcurrentDictionary<int, UserEmailLocation>();
        //private static ConcurrentDictionary<int, UserPushLocation> _pushLocations = new ConcurrentDictionary<int, UserPushLocation>();
        //private static ConcurrentDictionary<int, UserSmsLocation> _smsLocations = new ConcurrentDictionary<int, UserSmsLocation>();

        //public TestDAO()
        //{
        //    UserProfile profile = new UserProfile()
        //    {
        //        UserID = 1,
        //        UserName = "Sam",
        //        EmailOptIn = true,
        //        SmsOptIn = true,
        //        PushOptIn = true
        //    };

        //    _profiles.TryAdd(profile.UserID, profile);

        //    UserAddress address = new UserAddress()
        //    {
        //        UserAdressID = 1,
        //        UserID = 1,
        //        StreetAddress = "123 Any Street",
        //        City = "Sacramento",
        //        State = "CA",
        //        Zip = 95746,
        //        Latitude = 123.23,
        //        Longitude = 12.34
        //    };

        //    _addresses.TryAdd(address.UserAdressID, address);

        //    UserEmailLocation emailLoc = new UserEmailLocation()
        //    {
        //        UserEmailLocationID = 1,
        //        UserID = 1,
        //        UserAddressID = 1,
        //        EmailAddress = "sellis@trinitytg.com"
        //    };

        //    _emailLocations.TryAdd(emailLoc.UserEmailLocationID, emailLoc);

        //    UserPushLocation pushLoc = new UserPushLocation()
        //    {
        //        UserPushLocationID = 1,
        //        UserID = 1,
        //        UserAddressID = 1,
        //        PhoneNumber = 9165555555
        //    };

        //    _pushLocations.TryAdd(pushLoc.UserPushLocationID, pushLoc);

        //    UserSmsLocation smsLoc = new UserSmsLocation()
        //    {
        //        UserSmsLocationID = 1,
        //        UserID = 1,
        //        UserAddressID = 1,
        //        PhoneNumber = 9165555555
        //    };

        //    _smsLocations.TryAdd(smsLoc.UserSmsLocationID, smsLoc);
        //}
        #region notifications
        public async Task AddNotificationLocations(Notification notification)
        {
            //returns null
        }

        public async Task<Notification> UpdateNotification(Notification notification)
        {
            return notification;
        }

        public async Task<Notification> GetNotificationById(int id)
        {
            Notification notification = new Notification()
            {
                NotificationID = 1,
                Message = "Danger: Tornado",
                MessageDateTime = DateTime.Now,
                SendingUserID = 3,
                IncidentID = 1
            };
            return notification;
        }

        public async Task<List<Notification>> GetAllNotifications()
        {
            List<Notification> list = new List<Notification>();
            list.Add(new Notification()
            {
                NotificationID = 1,
                Message = "Danger: Tornado",
                MessageDateTime = DateTime.Now,
                SendingUserID = 3,
                IncidentID = 1
            });

            list.Add(new Notification()
            {
                NotificationID = 2,
                Message = "Danger: Tsunami",
                MessageDateTime = DateTime.Now,
                SendingUserID = 4,
                IncidentID = 2
            });

            return list;
        }

        public async Task<Notification> InsertNotification(Notification notification)
        {
            return notification;
        }
        #endregion notifications

        #region subscribers
        public async Task<List<Incident>> GetSubscribersForIncidents(List<Incident> incidents)
        {
            return incidents;
        }
        public async Task AddSubscribersToIncident(List<Subscriber> subscribers)
        {
            //returns void, error handling is in controller
        }
        public async Task<List<Subscriber>> GetAllSubscribers()
        {
            List<Subscriber> subscribers = new List<Subscriber>();

            subscribers.Add(new Subscriber() {
                SubscriberId = 1,
                AddressId = 1,
                Address = "2015 J St. Sacramento, CA 95811",
                Lat = 38.5767909,
                Lon = -121.4811453,
                Name = "Sam Ellis"
            });

            subscribers.Add(new Subscriber()
            {
                SubscriberId = 2,
                AddressId = 2,
                Address = "2015 J St. Sacramento, CA 95811",
                Lat = 38.5767909,
                Lon = -121.4811453,
                Name = "Shawn Sampo"
            });

            return subscribers;
        }
        #endregion subscribers
        #region incidents
        public async Task<List<Incident>> GetAllIncidents()
        {
            List<Incident> incidents = new List<Incident>();
            incidents.Add(new Incident()
            {
                Id = "Tsu470731",
                Lat = 37.788,
                Long = -119.718,
                IncidentType = "Tsunami",
                Radius = 30
            });

            return incidents;
        }
        public async Task CreateIncidents(List<Incident> incidents)
        {
            //returns void, error handling is in controller
        }
        #endregion
        #region users
        public async Task<ApplicationUser> GetUser(string userName) {
            return new ApplicationUser() { UserName = "Test Test" };
        }
        public async Task<ApplicationUser> AddUser(ApplicationUser user)
        {
            user.UserID = 1;
            return user;
        }

        public async Task<ApplicationUser> UpdateUserPassword(ApplicationUser user)
        {
            return user;
        }
        #endregion users

        #region profiles
        public async Task<UserProfile> GetProfileForUserName(string userName)
        {
            return new UserProfile() {
                UserID = 1,
                UserName = "Big Tester",
                SmsOptIn = true,
                EmailOptIn = true,
                PushOptIn = true,
            };
        }
        public async Task<UserProfile> UpdateUserProfile(UserProfile profile)
        {
            return profile;
        }
        #endregion profiles

        #region useraddress
        public async Task<UserAddress> GetAddressById(int id)
        {
            return new UserAddress()
            {
                UserAddressID = 1,
                UserID = 1,
                StreetAddress = "123 Any Street",
                City = "Sacramento",
                State = "CA",
                Zip = 95746,
                Latitude = 123.14,
                Longitude = 34.56
            };
        }
        public async Task<List<UserAddress>> GetAddressesForUser(string userName)
        {
            List<UserAddress> list = new List<UserAddress>();
            UserAddress address = new UserAddress()
            {
                UserAddressID = 1,
                UserID = 1,
                StreetAddress = "123 Any Street",
                City = "Sacramento",
                State = "CA",
                Zip = 95746,
                Latitude = 45.67,
                Longitude = 34.56
            };

            list.Add(address);

            return list;
        }
        public async Task<UserAddress> UpdateUserAddress(UserAddress address)
        {
            return address;
        }
        public async Task<UserAddress> InsertUserAddress(UserAddress address)
        {
            address.UserAddressID = 1;
            return address;
        }
        public async Task<bool> DeleteUserAddress(UserAddress address)
        {
            return true;
        }
        #endregion useraddress

        #region useremaillocation
        public async Task<List<UserEmailLocation>> GetUserEmailLocationsForNotification(Notification notification)
        {
            List<UserEmailLocation> emailLocs = new List<UserEmailLocation>();

            emailLocs.Add(new UserEmailLocation()
            {
                UserEmailLocationID = 1,
                EmailAddress = "sellis@trinitytg.com",
                UserAddressID = 1,
                UserID = 1
            });

            return emailLocs;
        }
        public async Task<UserEmailLocation> GetUserEmailLocationById(int id)
        {
            return new UserEmailLocation()
            {
                UserEmailLocationID = 1,
                UserAddressID = 1,
                EmailAddress = "sellis@trinitytg.com",
                UserID = 1
            };
        }
        public async Task<List<UserEmailLocation>> GetUserEmailLocationsForUser(string userName)
        {
            List<UserEmailLocation> emailLocs = new List<UserEmailLocation>();

            emailLocs.Add(new UserEmailLocation()
            {
                UserEmailLocationID = 1,
                EmailAddress = "sellis@trinitytg.com",
                UserAddressID = 1,
                UserID = 1
            });

            return emailLocs;
        }
        public async Task<UserEmailLocation> UpdateUserEmailLocation(UserEmailLocation emailLoc)
        {
            return emailLoc;
        }
        public async Task<UserEmailLocation> InsertUserEmailLocation(UserEmailLocation emailLoc)
        {
            emailLoc.UserEmailLocationID = 1;
            return emailLoc;
        }
        public async Task<bool> DeleteUserEmailLocation(UserEmailLocation emailLoc)
        {
            return true;
        }
        #endregion useremaillocation

        #region userpushlocation
        public async Task<UserPushLocation> GetUserPushLocationById(int id)
        {
            return new UserPushLocation()
            {
                UserPushLocationID = 1,
                UserAddressID = 1,
                PhoneNumber = 9165555555,
                UserID = 1
            };
        }
        public async Task<List<UserPushLocation>> GetUserPushLocationsForUser(string userName)
        {
            List<UserPushLocation> list = new List<UserPushLocation>();
            list.Add(new UserPushLocation()
            {
                UserPushLocationID = 1,
                UserAddressID = 1,
                PhoneNumber = 9165555555,
                UserID = 1
            });

            return list;
        }
        public async Task<UserPushLocation> UpdateUserPushLocation(UserPushLocation pushLoc)
        {
            return pushLoc;
        }
        public async Task<UserPushLocation> InsertUserPushLocation(UserPushLocation pushLoc)
        {
            pushLoc.UserPushLocationID = 1;
            return pushLoc;
        }
        public async Task<bool> DeleteUserPushLocation(UserPushLocation pushLoc)
        {
            return true;
        }
        #endregion userpushlocation

        #region usersmslocation

        public async Task<UserSmsLocation> GetUserSmsLocationById(int id)
        {
            return new UserSmsLocation()
            {
                UserSmsLocationID = 1,
                UserAddressID = 1,
                UserID = 1,
                PhoneNumber = 9165555555
            };
        }
        public async Task<List<UserSmsLocation>> GetUserSMSLocationsForNotification(Notification notification)
        {
            List<UserSmsLocation> smsList = new List<UserSmsLocation>();
            smsList.Add(new UserSmsLocation()
            {
                UserSmsLocationID = 1,
                UserAddressID = 1,
                UserID = 1,
                PhoneNumber = 9165555555
            });

            return smsList;
        }
        public async Task<List<UserSmsLocation>> GetUserSmsLocationsForUser(string userName)
        {
            List<UserSmsLocation> smsList = new List<UserSmsLocation>();
            smsList.Add(new UserSmsLocation()
            {
                UserSmsLocationID = 1,
                UserAddressID = 1,
                UserID = 1,
                PhoneNumber = 9165555555
            });

            return smsList;
        }
        public async Task<UserSmsLocation> UpdateUserSmsLocation(UserSmsLocation smsLoc)
        {
            return smsLoc;
        }
        public async Task<UserSmsLocation> InsertUserSmsLocation(UserSmsLocation smsLoc)
        {
            smsLoc.UserSmsLocationID = 1;
            return smsLoc;
        }
        public async Task<bool> DeleteUserSmsLocation(UserSmsLocation smsLoc)
        {
            return true;
        }
        #endregion usersmslocation
    }
}
