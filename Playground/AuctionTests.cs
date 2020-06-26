using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalUtilities.Persistence;
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

        [TestMethod]
        public void TestHigherBidBecomesHighBid()
        {
            // create started auction with item price of 1
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            auction.StartAuction();
            // create brenda
            var brenda = new User("brenda", "Jones", "email@email.com", "brendajones", "something");
            users.Register(brenda);
            users.Login(brenda.UserName, brenda.Password);
            // brenda bids 2
            auction.Bid(brenda, 2);
            // verify high bid is 2 and it's brenda
            Assert.AreEqual(brenda, auction.HighBid.Bidder);
            Assert.AreEqual(2, auction.HighBid.Price);
            // bob bids 5
            auction.Bid(bob, 5);
            Assert.AreEqual(bob, auction.HighBid.Bidder);
            Assert.AreEqual(5, auction.HighBid.Price);
            // verify high bid is 5 and it's bob
        }

        [TestMethod]
        public void TestAuctionCanClose()
        {
            // create world
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            auction.StartAuction();

            // start auction
            // close the auction
            auction.EndAuction();
            Assert.AreEqual(AuctionState.Closed, auction.State);
            // verify auction closed
        }
        //If an auction closes with no bidders then notify the seller “Sorry, your auction for <itemName> did not have any bidders.” 
        // If an auction closes with at least one bid then notify the seller “Your<itemName> auction sold to bidder<bidderEmail> for <highBidAmount>.” and notify the high bidder “Congratulations! You won an auction for a<itemName> from<sellerEmail> for <highBidAmount>.” 
        //user//story 8
        // As an auction I want to adjust the price of a sale so that I can handle fees
        // To the seller’s amount: Subtract a 2% transaction fee
        // To the high bidder’s amount: add $10 shipping fees for all items sold unless the item category is Downloadable Software (or a car)
        // If the item is a car add $1000 shipping fee
        // If a car sold for over $50,000 add 4% luxury tax
        // As an auction I want to log certain sales so that I have an audit trail
        // Log all car sales
        // Log all sales over $10,000





        private static (Users, User, User, Auction) CreateAuctionWorld()
        {
            var (users, scott) = CreateLoggedInSeller();
            var startTime = DateTime.Now.AddSeconds(1.0);
            var endTime = DateTime.Now.AddSeconds(3.0);
            var auction = new Auction(scott, "item description", 2, startTime, endTime);
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