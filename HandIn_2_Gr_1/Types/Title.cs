using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1.Types
{
    public class Title
    {
        public string Tconst { get; set; }

        public string? TitleType { get; set; }

        public string? PrimaryTitle { get; set; }

        public string? OriginalTitle { get; set; }

        public bool IsAdult { get; set; }

        public string? StartYear { get; set; }

        public string? EndYear { get; set; }

        public int RuntimeMinutes { get; set; }

        public IList<Genre>? Genre { get; set; }


        //Below is a custom attribute, indicating wether or not the title is a in a series or is a standalone movie
        public bool IsEpisode { get; set; } = false;


    }
}
