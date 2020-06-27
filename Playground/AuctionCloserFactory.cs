using System;
using System.Collections.Generic;
using System.Linq;
using Playground;

public class AuctionCloserFactory
{
    public static IAuctionCloser GetAuctionCloser(Bid highBid)
    {
        return new List<IAuctionCloser>() {new NoBidCloser(), new BidCloser()}.FirstOrDefault(closer => closer.IsValidFor(highBid));
    }
}
