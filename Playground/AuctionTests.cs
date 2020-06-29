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

        [TestMethod]
        public void TestAuctionHandleSellerFees()
        {
            //auction world 
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            auction.StartAuction();
            auction.Bid(bob, 1000);
            //bidder of 1000 
            auction.EndAuction();
            //close auction
            Assert.AreEqual(980, auction.AmountToSeller);
            //verify amount to seller is 98% (980) of high bid at close
        }

        [TestMethod]
        public void TestPlainAuctionBidderShippingFees()
        {
            //setup auction world
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            auction.StartAuction();
            auction.Bid(bob, 800);
            auction.EndAuction();
            Assert.AreEqual(1000,auction.ShippingFee);
            Assert.AreEqual(1800,auction.FinalPrice);
        } 
        
        [TestMethod]
        public void TestPlainAuctionBidderShippingFeesForSoftware()
        {

            //setup auction world
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            // set auction category to dl software
            auction.Category = AuctionCategory.DownloadableSoftware;
            auction.StartAuction();

            var bidAmount = 700;
            auction.Bid(bob, bidAmount);
            auction.EndAuction();
            // verify that shipping fee is 0
            Assert.AreEqual(0,auction.ShippingFee);
            // verify that the final price is the same as the high bid amount
            Assert.AreEqual(bidAmount,auction.FinalPrice);
        }       
        [TestMethod]
        public void TestPlainAuctionBidderShippingFeesForCar()
        {
            //setup auction world
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            // set auction category to car
            auction.Category = AuctionCategory.Car;
            auction.StartAuction();
            var bidAmount = 700;
            auction.Bid(bob, bidAmount);
            auction.EndAuction();
            // verify that shipping fee is 100,000
            Assert.AreEqual(100000,auction.ShippingFee);
            // verify that the final price is the same as the high bid amount + 100,000
            Assert.AreEqual(bidAmount+100000,auction.FinalPrice);
        }


        [TestMethod]
        public void TestLoggingCarSales()
        {
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            // set auction category to car
            auction.Category = AuctionCategory.Car;
            auction.StartAuction();
            var bidAmount = 800;
            auction.Bid(bob, bidAmount);
            auction.EndAuction();

            // verify that we logged the car sale
            Approvals.Verify(auction.GetLogs());
        }


        [TestMethod]
        public void TestLoggingAllSalesOver10_000()
        {
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            auction.StartAuction();
            var bidAmount = 11_000_00;
            auction.Bid(bob, bidAmount);
            auction.EndAuction();
            // verify that we logged sales over 1000000
            Approvals.Verify(auction.GetLogs());
        }

        [TestMethod]
        public void TestPlainAuctionBidderFeesForLuxuryCar()
        {
            //setup auction world
            (Users users, User scott, User bob, Auction auction) = CreateAuctionWorld();
            // set auction category to car
            auction.Category = AuctionCategory.Car;
            auction.StartAuction();
            var bidAmount = 600000000;
            auction.Bid(bob, bidAmount);
            auction.EndAuction();
            // verify that shipping fee is 100,000
            Assert.AreEqual(100000, auction.ShippingFee);
            Assert.AreEqual(bidAmount*0.04, auction.LuxuryTax);
            // verify that the final price is the same as the high bid amount + 100,000 + luxury tax
            Assert.AreEqual((1.04*bidAmount + 100000) , auction.FinalPrice);
        }
  


        // Log all sales over $10,000


        public static (Users, User, User, Auction) CreateAuctionWorld()
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