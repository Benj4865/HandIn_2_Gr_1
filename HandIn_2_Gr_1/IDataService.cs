using HandIn_2_Gr_1.Models;
using HandIn_2_Gr_1.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{
    public interface IDataService
    {
        public Person GetActor(string nconst);
        IList<Person> SearchByProfession(string professionstype);

        //IList<Title> SearchByGerne(string GenreName);




    }
}
