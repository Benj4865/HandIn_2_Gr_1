﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1.Types
{
    public class Paging
    {
        public IList<Title> titles { get; set; }

        public IList<User> users { get; set; }

        public IList<Person> people {  get; set; }        

        public string nextpage { get; set; }

        public string previouspage { get; set; }

    }
}
