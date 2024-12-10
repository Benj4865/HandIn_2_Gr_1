using HandIn_2_Gr_1.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{
    public interface IDataServiceTitle
    {

        public Title SearchTitleByName(string name);

        public Title CreateTitle(string tconst, string titletype, string primaryTitle, string originalTitle, string isAdult, string startyear, string endyear, int runtimeMinutes, string genres, string posterlink, string plot);

        public Title updateTitle(string tconst, string titletype, string primaryTitle, string originalTitle, string isAdult, string startyear, string endyear, int runtimeMinutes, string genres, string posterlink, string plot);

    }
}
