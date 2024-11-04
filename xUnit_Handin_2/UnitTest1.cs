using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;

namespace xUnit_Handin_2
{
    public class UnitTest1
    {
        [Fact]
        public void TestIsEpisode()
        {

        }


        [Fact]
        public void CheckNameOnPerson()
        {
            var Service = new DataServicePerson();
            Person person = DataServicePerson.GetPerson("nm11345295");
            Assert.Equal("María Alejandra Mosquera", person.Primaryname);

        }
    }
}