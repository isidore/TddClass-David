using System;

namespace Playground
{
    internal class Auction
    {
        public User Seller { get; }
        public string ItemDescription { get; }
        public double ItemPrice { get; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }

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
    }
}