using System.Data.SqlClient;
namespace BusTicketBooking
{
    internal class BusUser
    {
        public static string email;
        public static long? mob;
        public static string name;
        public static string gen;
        public static string dob;
        public static string city;
        public static void setEmail(string em)
        {
            email = em;
        }
        public static SqlConnection getCon()
        {
            return new SqlConnection(@"data source=(localdb)\mssqllocaldb;
        initial catalog=master;integrated security=true");
        }
        public static void setName(string nam)
        {
            name= nam;
        }
        public static void setGen(string gn)
        {
            gen = gn;
        }
        public static void setDob(string db) 
        {
            dob= db;
        }
        public static void setCity(string c)
        {
            city = c;
        }
        public static void setMob(long? mb)
        {
            mob = mb;
        }
        public static string getEmail()
        {
            return email;
        }
        public static long? getMob()
        {
            return mob;
        }
        public static string getName()
        {
            return name;
        }
        public static string getGen()
        {
            return gen;
        }
        public static string getDob()
        {
            return dob;
        }
        public static string getCity()
        {
            return city;
        }
    }
}
