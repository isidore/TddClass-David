using System.Collections.Generic;

namespace Playground
{
    internal class BidCloser
    {
        private static Dictionary<string, string> GetEmailsForBids(Bid highBid, string itemDescription, User seller)
        {
            var emails = new Dictionary<string, string>();
            emails.Add(seller.Email,
                $"Your {itemDescription} auction sold to bidder {highBid.Bidder.Email} for {highBid.Price}.");
            emails.Add(highBid.Bidder.Email,
                $"Congratulations! You won an auction for a {itemDescription} from {seller.Email} for {highBid.Price}");
            return emails;
        }
    }
}