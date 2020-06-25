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
        /*
        – testAccessors() : verify fields are set correctly
        – testCantCreateAuctionIfUserNotSeller() : Only sellers can create auctions
        – testCantCreateAuctionIfSellerNotLoggedIn() : Must be loggedin to create
        – testCantCreateAuctionIfStartTimeLessThanNow() : Start must be > now
        – testCantCreateAuctionIfEndTimeLessThanStartTime() :End must be > start
        */
        [TestMethod]
        public void TestCreateAuction()
        {
            var scott = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);
            var auction = new Auction(scott, "item description", 0.10, startTime, endTime);

            Assert.AreEqual(auction.Seller, scott);
            Assert.AreEqual(auction.ItemDescription, "item description");
            Assert.AreEqual(auction.ItemPrice, 0.10, 0.01);
            Assert.AreEqual(auction.StartDateTime, startTime);
            Assert.AreEqual(auction.EndDateTime, endTime);
            //

        }

        [TestMethod]
        public void TestCantCreateAuctionIfUserNotSeller()
        {
            var userTest = new UserTest();
            var (users, scott) = userTest.SetupScott(true);
            users.Login(scott.UserName, scott.Password);
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);

            Assert.ThrowsException<Exception>(() => new Auction(scott, "item description", 0.10, startTime, endTime));
        }

        [TestMethod]
        public void TestCantCreateAuctionIfNotLoggedIn()
        {
            var userTest = new UserTest();
            var (users, scott) = userTest.SetupScott(true);
            // check if user is a seller
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);

            Assert.ThrowsException<UserNotLoggedInException>(() =>
                new Auction(scott, "item description", 0.10, startTime, endTime));
        }

        [TestMethod]
        public void TestCantCreateAuctionIfStartTimeLessThanNow()
        {
            var scott = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(-8.0);
            var endTime = DateTime.Now.AddSeconds(3.0);

            Assert.ThrowsException<AuctionInPastException>(() =>
                new Auction(scott, "item description", 0.10, startTime, endTime));
        }

        [TestMethod]
        public void TestCantCreateAuctionIfEndTimeLessThanStartTime()
        {

            var scott = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(10.0);
            var endTime = DateTime.Now.AddSeconds(8.0);

            Assert.ThrowsException<AuctionEndedBeforeItStartException>(() =>
                new Auction(scott, "item description", 0.10, startTime, endTime));

        }

        [TestMethod]
        public void TestAuctionToAcceptBids()
        {
            // need a logged in seller?
            var scott = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);
            var auction = new Auction(scott, "item description", 0.10, startTime, endTime);
            // verify the auction status is not started
            Assert.AreEqual(auction.State, AuctionState.NotStarted);
            // Start the auction 
            auction.StartAuction();
            // verify the auction status is started
            Assert.AreEqual(auction.State, AuctionState.Started);
        }

        private static User CreateLoggedInSeller()
        {
            var userTest = new UserTest();
            var (users, scott) = userTest.SetupScott(true);
            users.MakeSeller(scott);
            users.Login(scott.UserName, scott.Password);
            return scott;
        }
    }
}