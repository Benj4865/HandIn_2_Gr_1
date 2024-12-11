using HandIn_2_Gr_1.Types;

namespace WebAPI.PostModels
{
    public class PersonBody
    {
        public string Nconst { get; set; } = null;

        public string Primaryname { get; set; } = null;

        public string Birthyear { get; set; } = null;

        public string Deathyear { get; set; } = null;

        public string Primaryprofessions { get; set; } = null;

        public string KnownFor { get; set; } = null;
    }
}
