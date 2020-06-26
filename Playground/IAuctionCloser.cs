using System.Collections.Generic;

namespace Playground
{
    public interface IAuctionCloser
    {
        Dictionary<string, string> GetEmailsForClose(Bid highBid, string itemDescription, User seller);
    }
}