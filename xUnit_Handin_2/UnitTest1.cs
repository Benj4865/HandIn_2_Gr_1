using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;

namespace xUnit_Handin_2
{
    public class DataServicePersonTest
    {

        [Fact]
        public void GetPerson_ValidID_ReturnsPerson()
        {
            var validId = "nm0000001";
            var service = new DataServicePerson();

            var result = DataServicePerson.GetPerson(validId);

            Assert.NotNull(result);
            Assert.Equal(validId, result.Nconst);
            Assert.NotNull(result.Primaryname);
            Assert.IsType<string>(result.Primaryname);
            Assert.IsType<string>(result.Nconst);
            Assert.IsType<string>(result.Birthyear);

        }
        [Fact]
        public void GetPerson_InvalidID_ReturnsNull()
        {
            var invalidId = "nm10000000";
            var service = new DataServicePerson();

            var result = DataServicePerson.GetPerson(invalidId);

            Assert.Null(result);
        }
        [Fact]
        public void ()
        {
        
        }

        [Fact]
        public void TestIsEpisode()
        {

        }

        // Tests wether or not the name of a specific person is correct
        [Fact]
        public void CheckNameOnPerson()
        {
            var Service = new DataServicePerson();
            Person person = DataServicePerson.GetPerson("nm11345295");
            Assert.Equal("María Alejandra Mosquera", person.Primaryname);
        }


        // Tests wether or not a specific episode of a tv-series is returned, and tests the name of it
        [Fact]
        public void CheckEpisodesOnTConst()
        {
            var service = new DataServiceTitle();
            IList<Title> titleList = DataServiceTitle.FindEpisodesFromSeriesTconst("tt0108778");
            Title episode = titleList[0];
            Assert.Equal("The One Where Chandler Can't Remember Which Sister", episode.PrimaryTitle);
        }


    }
}