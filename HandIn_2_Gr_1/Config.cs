using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{
    public static class Config
    {
        public static int Logged_In_User()
        {
            int loggedInID = 1;

            return loggedInID;
        }

        public static string GetConnectionString()
        {
            var filepath = "C:/Users/NotAtAllPostGresPW.txt";

            //Below is the password data. Upon testing on diffrent systems, it sould be replaced with the actual password,
            //found in the report.
            var password = File.ReadAllText(filepath);

            return "Host=cit.ruc.dk;Port=5432;Username=cit01;Password=" + password + ";Database=cit01";
        }

    }

}

