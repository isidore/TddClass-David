using System;
using Playground;

public class AuctionCloserFactory
{
    public static IAuctionCloser GetAuctionCloser(Bid highBid)
    {
        IAuctionCloser closer;
        if (highBid.Bidder == null)
        {
            closer = new NoBidCloser();
        }
        else
        {
            closer = new BidCloser();
        }

        return closer;
    }
}
