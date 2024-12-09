using HandIn_2_Gr_1.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{
    public interface IDataServiceUser
    {

        IList<User> GetUsers();

        void CreateUser(int userID, string userName, string password, string useremail);

        public User SearchUser(string username, string useremail, int userID);
        public void UpdateUser(int userID, string userName, string userPassword, string userEmail);
        void DeleteUser(int userID, string password);

    }
}
