using AVP.Models.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AVP.DataAccess
{
    public interface IDBConnection : IDisposable
    {
        
    }

    public class DBConnection : IDBConnection
    {
        public readonly MySqlConnection Connection;
        public readonly string Schema;
        
        public DBConnection(DbConnectionSettings settings)
        {
            Schema = settings.Schema;
            Connection = new MySqlConnection(settings.ConnectionString);
        }
        
        public void Dispose()
        {
            Connection.Close();
        }
    }

    public class DbConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
    }
}
