using System.Collections.Generic;

namespace Playground
{
    public class BidCloser : IAuctionCloser
    {
        public Dictionary<string, string> GetEmailsForClose(Bid highBid, string itemDescription, User seller)
        {
            var emails = new Dictionary<string, string>();
            emails.Add(seller.Email,
                $"Your {itemDescription} auction sold to bidder {highBid.Bidder.Email} for {highBid.Price}.");
            emails.Add(highBid.Bidder.Email,
                $"Congratulations! You won an auction for a {itemDescription} from {seller.Email} for {highBid.Price}");
            return emails;
        }

        public bool IsValidFor(Bid highBid)
        {
            return highBid.Bidder != null;
        }
    }
}