using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest_HandIn_2_Gr_1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void getDataOnActor()
        {
            var actorId = "nm11345295";

            Assert.IsNotNull(actorId);
        }
    }
}
