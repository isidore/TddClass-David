using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Playground
{
    [TestClass]
    [UseReporter(typeof(DiffReporter))]
    public class AuctionTests
    {
        [TestMethod]
        public void TestCreateAuction()
        {
            // create a an action class
            // 
            var seller = new User("John","Knoll","jpknoll@notucdavis.com","great","nowhere");
            UserTest userTest = new UserTest();
            var (users, scott) = userTest.SetupScott(true);
            users.MakeSeller(scott);
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);
            var auction = new Auction(scott, "item description", 0.10, startTime, endTime);

            Assert.AreEqual(auction.Seller, scott);
            Assert.AreEqual(auction.ItemDescription, "item description");
            Assert.AreEqual(auction.ItemPrice, 0.10, 0.01);
            Assert.AreEqual(auction.StartDateTime, startTime);
            Assert.AreEqual(auction.EndDateTime, endTime     );
            //

        }

    }
}         