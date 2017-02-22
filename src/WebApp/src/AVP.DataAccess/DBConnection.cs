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
        
        public void Dispose()
        {
            Connection.Close();
        }
    }
}
