using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1.Types
{
    public class Person
    {
        public string Nconst { get; set; } = null;

        public string Primaryname { get; set; } = null;

        public string Birthyear { get; set; } = null;

        public string Deathyear { get; set; } = null;

        public IList<Professions> Primaryprofessions { get; set; } = null;

        public IList<Title> KnownFor { get; set; } = null;

    }
}
