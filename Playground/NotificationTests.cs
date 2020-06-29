using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalUtilities.Persistence;
using eBabyServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Playground
{
    [TestClass]
    [UseReporter(typeof(DiffReporter))]

    public class NotificationTests
    {
        [TestMethod]
        public void TestNotifySellerNoBidders()
        {
            PostOffice.GetInstance().Clear();
            (Users users, User scott, User bob, Auction auction) =AuctionTests.CreateAuctionWorld();
            auction.StartAuction();
            auction.EndAuction();
            Approvals.Verify(PostOffice.GetInstance().FindEmail(scott.Email, auction.ItemDescription));
        }

        [TestMethod]
        public void TestNotificationEmailForSellerNoBids()
        {
            (Users users, User scott, User bob, Auction auction) =AuctionTests.CreateAuctionWorld();
            auction.StartAuction();
            
            Approvals.VerifyAll(auction.GetClosingEmailNotifications());
        }

        [TestMethod]
        public void TestNotifySellerThatTheItemSold()
        {
            //create auction world
            //start an auction
            //bob bids $5
            //verify closing notification emails
            (Users users, User scott, User bob, Auction auction) = AuctionTests.CreateAuctionWorld();
            auction.StartAuction();
            auction.Bid(bob, 5);
            Approvals.VerifyAll(auction.GetClosingEmailNotifications());
        }
    }

}

