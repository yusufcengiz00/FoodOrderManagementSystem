using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq; // İlk veya tek sonucu almak için gerekli

namespace FoodOrderManagementSystem.Models
{
    public class Context
    {
        public static string connectionstring = @"Server=(localdb)\MSSQLLocalDB;Database=FoodOrderDB;Integrated Security=true;";
        // Kayıt ekleme, güncelleme ve silme için
        public static void Execute(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Execute(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        // Context.cs içerisine ekle
        public static T ExecuteScalar<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                return db.ExecuteScalar<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        // Listeleme işlemleri için
        public static IEnumerable<T> Listeleme<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                return db.Query<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        // Tek bir satır getirme (Örn: Giriş yapan kullanıcı, ID'ye göre ürün)
        public static T Getir<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                return db.QueryFirstOrDefault<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}