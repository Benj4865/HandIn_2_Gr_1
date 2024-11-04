using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;
using System.Collections.Generic;
using Xunit;

namespace xUnit_Handin_2
{
    public class DataServicePersonTest
    {
        [Fact]
        public void GetPerson_ValidID_ReturnsPerson()
        {
            var validId = "nm0000001"; //Id exists = Fred Astaire
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
            var invalidId = "nm10000000000000"; //Non existing ID
            var service = new DataServicePerson();

            var result = DataServicePerson.GetPerson(invalidId);

            Assert.Null(result);
        }
        [Fact]
        public void SearchByProfession_ValidProfession_ReturnsPeople()
        {
            var profession = "actor";
            var service = new DataServicePerson();
            var result = service.SearchByProfession(profession);

            Assert.NotNull(result);
            Assert.NotEmpty(profession);
            Assert.All(result, person => Assert.IsNotType<Person>(person));
        }
        [Fact]
        public void SearchByProfession_InvalidProfession_ReturnsEmptyList()
        {
            var invalidProfession = "FLooferWoofer";
            var service = new DataServicePerson();

            var result = service.SearchByProfession(invalidProfession);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // Tests wether or not the name of a specific person is correct
        [Fact]
        public void CheckNameOnPerson()
        {
            var Service = new DataServicePerson();
            var person = DataServicePerson.GetPerson("nm11345295");
            Assert.NotNull(person);
            Assert.Equal("María Alejandra Mosquera", person.Primaryname);
        }

        [Fact]
        public void FindKnowForTitles_InvalidNconst_ReturnsEmptyList()
        {
            var invalidNconst = "nm10000000000000";
            var result = DataServicePerson.FindKnownForTitles(nm10000000000000);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetPerson_NullID_ThrowsArgumentNullException()
        {
            string nullId = null;
            var exception = Assert.Throws<ArgumentNullException>(() => DataServicePerson.GetPerson(nullId));
            Assert.Equal("Value cannot be null.(Parameter 'id')", exception.Message);
        }

        [Fact]
        public void SearchByProfession_EmptyProfession_ReturnsEmptyList()
        {
            var emptyProfession = "";
            var service = new DataServicePerson();

            var result = service.SearchByProfession(emptyProfession);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        // Tests wether or not a specific episode of a tv-series is returned, and tests the name of it
        [Fact]
        public void CheckEpisodesOnTConst()
        {
            var service = new DataServiceTitle();
            IList<Title> titleList = DataServiceTitle.FindEpisodesFromSeriesTconst("tt0108778");
            Assert.NotEmpty(titleList);

            Title episode = titleList[0];
            Assert.Equal("The One Where Chandler Can't Remember Which Sister", episode.PrimaryTitle);
        }


    }
}