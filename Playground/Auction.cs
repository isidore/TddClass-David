using System;

namespace Playground
{
    public enum AuctionState
    {
        NotStarted,
        Started,
        Closed
    }

    internal class Auction
    {
        public User Seller { get; }
        public string ItemDescription { get; }
        public double ItemPrice { get; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }
        public AuctionState State { get; internal set; }

        
        public Auction(User seller, string itemDescription, double itemPrice, DateTime startDateTime, DateTime endDateTime)
        {
            if (!seller.IsLoggedIn)
            {
                throw new UserNotLoggedInException();
            }

            if (!seller.IsSeller)
            {
                throw new Exception("User is not a seller");
            }

            if (DateTime.Now > startDateTime)
            {
                throw new AuctionInPastException();
            }

            if (endDateTime < startDateTime)
            {
                throw new AuctionEndedBeforeItStartException();
            }

            Seller = seller;
            ItemDescription = itemDescription;
            ItemPrice = itemPrice;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }

        public override string ToString()
        {
            return $"{nameof(Seller)}: {Seller}, {nameof(ItemDescription)}: {ItemDescription}, {nameof(ItemPrice)}: {ItemPrice}".Replace(",","\n");
        }

        public void StartAuction()
        {
            State = AuctionState.Started;
        }

        public void Bid(User bidder, int bidAmount)
        {
            if (State == AuctionState.NotStarted)
            {
                throw new AuctionNotStartedCantAcceptBidException();
            }
        }
    }
}