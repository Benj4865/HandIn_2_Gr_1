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


        
        [Fact]
        public void CheckNameOnPerson()
        {
            var Service = new DataServicePerson();
            var person = Service.GetPerson("nm11345295");
            Assert.NotNull(person);
            Assert.Equal("María Alejandra Mosquera", person.Primaryname);
        }

        [Fact]
        public void createAndDeleteUser_test()
        {
            var dataservice = new DataServiceUser();

            var usernew = new User
            {
                UserName = "unituser123",
                UserPassword = "unituser123",
                UserEmail = ""
            };

            dataservice.CreateUser(usernew.UserName, usernew.UserPassword, usernew.UserEmail);

            IList<User> user = dataservice.SearchUser("unituser123", "unituser123", 0, 1, 1);
            Assert.Equal("unituser123", user[0].UserName);

            dataservice.DeleteUser(user[0].UserID, usernew.UserPassword);

            Assert.Null(dataservice.SearchUID(user[0].UserID).UserName);
            
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