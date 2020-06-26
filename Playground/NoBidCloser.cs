using System.Collections.Generic;

namespace Playground
{
    public class NoBidCloser : IAuctionCloser
    {
        public Dictionary<string, string> GetEmailsForClose(Bid highBid, string itemDescription, User seller)
        {
            var emails = new Dictionary<string, string>();
            //send email to seller
            emails.Add(seller.Email, $"Sorry, your auction for {itemDescription} did not have any bidders.");
            return emails;
        }
    }
}