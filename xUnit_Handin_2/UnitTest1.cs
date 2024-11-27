using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;
using System.Collections.Generic;
using Xunit;

namespace xUnit_Handin_2
{
    public class DataServicePersonTest
    {

        // The Following test is coded with help from Co-Pilot
        [Fact]
        public void GetPerson_ValidID_ReturnsPerson()
        {
            var validId = "nm0000001 "; //Id exists = Fred Astaire
            var service = new DataServicePerson();

            var result = service.GetPerson(validId);

            Assert.NotNull(result);
            Assert.Equal(validId, result.Nconst);
            Assert.NotNull(result.Primaryname);
            Assert.IsType<string>(result.Primaryname);
            Assert.IsType<string>(result.Nconst);
            Assert.IsType<string>(result.Birthyear);

        }

        // The Following test is coded with help from Co-Pilot
        [Fact]
        public void GetPerson_InvalidID_ReturnsNull()
        {
            string invalidId = "nm10000000000000"; //Non existing ID
            var service = new DataServicePerson();

            Person result = service.GetPerson(invalidId);

            Assert.Null(result);

        }

        // The Following test is coded with help from Co-Pilot
        [Fact]
        public void SearchByProfession_ValidProfession_ReturnsPeople()
        {
            var profession = "actor";
            var service = new DataServicePerson();
            var result = service.SearchByProfession(profession);
            // IList<Title> titleList = DataServiceTitle.FindEpisodesFromSeriesTconst("tt0108778");

            Assert.NotNull(result);
            var person = result[0];
            Assert.NotEmpty(result[0].Primaryname);
            
        }

        // The Following test is coded with help from Co-Pilot
        [Fact]
        public void SearchByProfession_InvalidProfession_ReturnsEmptyList()
        {
            var invalidProfession = "FLooferWoofer";
            var service = new DataServicePerson();

            var result = service.SearchByProfession(invalidProfession);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        
        [Fact]
        public void CheckNameOnPerson()
        {
            var Service = new DataServicePerson();
            var person = Service.GetPerson("nm11345295");
            Assert.NotNull(person);
            Assert.Equal("María Alejandra Mosquera", person.Primaryname);
        }



        // The Following test is coded with help from Co-Pilot
        [Fact]
        public void SearchByProfession_EmptyProfession_ReturnsEmptyList()
        {
            var emptyProfession = "";
            var service = new DataServicePerson();

            var result = service.SearchByProfession(emptyProfession);

            Assert.NotNull(result);
            Assert.Empty(result);
        }


        
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