using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1.Types
{
    public class Person
    {
        public string Nconst { get; set; }

        public string Primaryname { get; set; }

        public string Birthyear { get; set; }

        public string Deathyear { get; set; }

        public IList<Professions> Primaryprofessions { get; set; } = null;

        public IList<Title> Titles { get; set; } 


    }


}
