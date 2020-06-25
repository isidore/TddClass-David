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
            var (_, scott ) = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);
            var auction = new Auction(scott, "item description", 10, startTime, endTime);

            Assert.AreEqual(auction.Seller, scott);
            Assert.AreEqual(auction.ItemDescription, "item description");
            Assert.AreEqual(auction.ItemPrice, 10);
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

            Assert.ThrowsException<Exception>(() => new Auction(scott, "item description", 10, startTime, endTime));
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
                new Auction(scott, "item description", 10, startTime, endTime));
        }

        [TestMethod]
        public void TestCantCreateAuctionIfStartTimeLessThanNow()
        {
            var (_, scott) = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(-8.0);
            var endTime = DateTime.Now.AddSeconds(3.0);

            Assert.ThrowsException<AuctionInPastException>(() =>
                new Auction(scott, "item description", 10, startTime, endTime));
        }

        [TestMethod]
        public void TestCantCreateAuctionIfEndTimeLessThanStartTime()
        {

            var (_, scott) = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(10.0);
            var endTime = DateTime.Now.AddSeconds(8.0);

            Assert.ThrowsException<AuctionEndedBeforeItStartException>(() =>
                new Auction(scott, "item description", 10, startTime, endTime));

        }

        [TestMethod]
        public void TestAuctionToAcceptBids()
        {
            // need a logged in seller?
            var (_, scott) = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);
            var auction = new Auction(scott, "item description", 10, startTime, endTime);
            // verify the auction status is not started
            Assert.AreEqual(auction.State, AuctionState.NotStarted);
            // Start the auction 
            auction.StartAuction();
            // verify the auction status is started
            Assert.AreEqual(auction.State, AuctionState.Started);
        }

        [TestMethod]
        public void TestAuctionNotStartedCantAcceptBids()
        {
            // logged in seller 
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();            
            // when the user tries to make a bid of 1 on not-started auction they get an exception
            Assert.ThrowsException<AuctionNotStartedCantAcceptBidException>(() =>
                auction.Bid(bob, 100));
        }

        [TestMethod]
        public void TestCantAcceptBidsIfBidderIsntLoggedIn()
        {
            // set up auction world
            // start the auction
            // log out bob (bidder)
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            auction.StartAuction();
            users.Logout(bob.UserName);
            // when the user tries to make a bid of 1 on not-started auction they get an exception
            Assert.ThrowsException<AuctionCantAcceptBidSinceBidderNotLoggedInException>(() =>
                auction.Bid(bob, 1));
            // verify that bob tries to bid of 4 and receives exception
        }

        [TestMethod]
        public void TestLowerBidDoesntBecomeHighBid()
        {
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            auction.StartAuction();
            auction.Bid(bob, 200);

            // verify that the high bid is 2
            Assert.AreEqual(200, auction.HighBid.Price);
            // bob submits lower bid of 1
            auction.Bid(bob, 100);

            // verify that high bid doesn't change
            Assert.AreEqual(200, auction.HighBid.Price);
        }


        /*
          
          testHigherBidBecomesHighBid() (happy path)AA
         */

        private static (Users, User, User, Auction) CreateAuctionWorld()
        {
            var (users, scott) = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);
            var auction = new Auction(scott, "item description", 10, startTime, endTime);
            // we need a logged in user
            var bob = new User("Bob", "Jones", "email@email.com", "bjones", "something");
            users.Register(bob);
            users.Login(bob.UserName, bob.Password);
            return (users, scott, bob, auction);
        }

        

        private static (Users, User) CreateLoggedInSeller()
        {
            var userTest = new UserTest();
            var (users, scott) = userTest.SetupScott(true);
            users.MakeSeller(scott);
            users.Login(scott.UserName, scott.Password);
            return (users, scott);
        }
    }
}