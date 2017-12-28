using MySpectrum.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySpectrum.DataAccessLayer
{
   public class LocalDatabaseHelper
    {
        public LocalDatabaseHelper()
        {
        }

        /// <summary>
        /// Generic type method to insert any type of object to local SQLite database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">data to be insert</param>
        /// <param name="database_path">SQLite database path</param>
        /// <returns>if InsertData is success or not</returns>
        public static bool InsertData<T>(ref T data, string database_path)
        {
            using (var SQLiteConnection = new SQLite.SQLiteConnection(database_path))
            {
                SQLiteConnection.CreateTable<T>();
                if (SQLiteConnection.Insert(data) != 0)
                {
                    return true;
                }
            }

                return false;
        }

        public static List<User> ReadData(string database_path)
        {
            List<User> user_list = new List<User>();

            using (var SQLiteConnection = new SQLite.SQLiteConnection(database_path))
            {
                user_list = SQLiteConnection.Table<User>().ToList();
            }

                return user_list;
        }

        public static bool DeletData(string database_path)
        {
            using (var SQLiteConnection = new SQLite.SQLiteConnection(database_path))
            {
                if (SQLiteConnection.DeleteAll<User>() != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
