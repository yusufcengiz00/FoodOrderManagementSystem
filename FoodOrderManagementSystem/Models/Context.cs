using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FoodOrderManagementSystem.Models
{
    public class Context
    {
        public static string connectionstring = @"Server=(localdb)\MSSQLLocalDB;Database=FoodOrderDB;Integrated Security=true;";

        // CUD İşlemleri
        public static void Execute(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Execute(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        // Tekil Değer Getirme
        public static T ExecuteScalar<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                return db.ExecuteScalar<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        // Listeleme
        public static IEnumerable<T> Listeleme<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                return db.Query<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        // Tekil Kayıt Getirme
        public static T Getir<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                return db.QueryFirstOrDefault<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}