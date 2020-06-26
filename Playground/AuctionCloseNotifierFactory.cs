using System;
using System.Collections.Generic;
using Playground;

public class AuctionCloseNotifierFactory
{
    private readonly Auction _auction;

    public AuctionCloseNotifierFactory(Auction auction)
    {
        _auction = auction;

    }

    public IAuctionNotifier GetNotifier()
    {

        if (_auction.HighBid.Bidder == null)
        {
            return new NoSaleAuctionNotifier();
        }
        else
        {
            return new SuccessfulAuctionNotifier();
        }
    }
}