﻿using HandIn_2_Gr_1.Types;
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

        public IList<Person> SearchByName(string name);
        public IList<Person> SearchByProfession(string professionname);

        public IList<Title> FindKnownForTitles(string Nconst);

    }
}
