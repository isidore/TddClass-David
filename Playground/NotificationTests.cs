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
            (Users users, User scott, User bob, Auction auction) =AuctionTests.CreateAuctionWorld();
            auction.StartAuction();
            //create auction world
            //start an auction
            //(no bidders)
            //close the auction 
            auction.EndAuction();
            //verify notification to seller
            Approvals.Verify(PostOffice.GetInstance().FindEmail(scott.Email, auction.ItemDescription));
        }
    }

}
/*
If an auction closes with no bidders then notify the seller “Sorry, your auction for <itemName> did not have any bidders.” 
If an auction closes with at least one bid then notify the seller “Your <itemName> auction sold to bidder <bidderEmail> for <highBidAmount>.” and notify the high bidder “Congratulations! You won an auction for a <itemName> from <sellerEmail> for <highBidAmount>.” 

 */
