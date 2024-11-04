namespace xUnit_Handin_2
{
    public class DataServicePersonTests
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
        public void TestIsEpisode()
        {

        }

        // Tests wether or not the name of a specific person is correct
        [Fact]
        public void CheckNameOnPerson()
        {

            var result = DataServicePerson.GetPerson(invalidId);

        }


    }
}