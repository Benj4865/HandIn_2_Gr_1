using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1.Types
{
    public class User
    {
        public int UserID { get; set; } = 0;

        //This is added to give at place to selfrefrence in the webapi
        public string userlink {  get; set; }

        public string UserName { get; set; } = null;

        public string UserPassword { get; set; } = null;

        public string UserEmail { get; set; } = null;
        

    }
}
