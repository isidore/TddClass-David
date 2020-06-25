using System;

namespace eBabyServices
{
    public interface Auctionable
    {
        void HandleAuctionEvents(DateTime now);
    }
}