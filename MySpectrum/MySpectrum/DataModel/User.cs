using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySpectrum.DataModel
{

    public enum UserGender
    {
        Male,
        Female,
        None
    }

    public class User
    {
        public User()
        { }

        public User(string firstName, string lastName, string gender, string age, string userName, string userPassworrd)
        {
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Age = age;
            UserName = userName;
            UserPassworrd = userPassworrd;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Gender { get; set; }

        public string Age { get; set; }

        public string UserName { get; set; }

        public string UserPassworrd { get; set; }


        public override string ToString()
        {
            return string.Format("[{0}, {1} ,{2}, {3}, {4}]",FirstName, LastName,Gender, Age, UserName);
        }
    }
}
