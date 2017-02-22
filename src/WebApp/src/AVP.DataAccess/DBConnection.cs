using AVP.Models.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.DataAccess
{
    public interface IDBConnection : IDisposable
    {
        
    }

    public class DBConnection : IDBConnection
    {
        public readonly MySqlConnection Connection;
        
        public DBConnection()
        {
            Connection = new MySqlConnection("host=13.64.66.218;port=3306;user id=root;password=Jjyk454ie7Jb;database=avp2017;");
        }

        //#region users
        //public ApplicationUser GetUser(string user_name)
        //{
        //    ApplicationUser user = new ApplicationUser();
        //    try
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        //        {
        //            var command = new MySqlCommand(@"select * from users where user_name = @user_name", connection);
        //            using (command)
        //            {
        //                connection.Open();
        //                command.Parameters.AddWithValue("@user_name", user_name);

        //                connection.Close();
        //            }
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        string message = e.Message;
        //    }

        //    return user;
        //}
        //#endregion users

        //todo: implement IDisposable
        public void Dispose()
        {
            Connection.Close();
        }
    }
}
