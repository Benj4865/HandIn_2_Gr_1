using HandIn_2_Gr_1.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{
    public interface IDataServicePerson 
    {  
        public Person GetPerson(string nconst);

        public IList<Person> SearchByName(string name, int pagesize, int page);
        
        public Person createPerson(Person newPerson);

        public Person updatePerson(string nconst, string primaryname, string birthyear, string deathyear, string primaryprofession, string knownForTitles);
        public IList<Person> SearchByProfession(string professionname, int pagesize, int page);

        //public IList<Title> FindKnownForTitles(string Nconst);

    }
}
