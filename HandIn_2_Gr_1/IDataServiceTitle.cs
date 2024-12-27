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

        public IList<Title> SearchTitleByName(string name, int pagesize, int page);

        public Title SearchTitleByTConst(string tconst);

        public Title CreateTitle(string tconst, string titletype, string primaryTitle, string originalTitle, string isAdult, string startyear, string endyear, int runtimeMinutes, string genres, string posterlink, string plot);

        public Title updateTitle(string tconst, string titletype, string primaryTitle, string originalTitle, string isAdult, string startyear, string endyear, int runtimeMinutes, string genres, string posterlink, string plot);

        public void DeleteTitle(string tconst);

        public bool bookmarkTitle(string linkstring);
    }
}
