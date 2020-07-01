namespace Playground
{
   public class LuxuryTaxHandler: IHandler
    {
        public  void Handle(Auction auction)
        {
            if ((auction.Category == AuctionCategory.Car) && auction.HighBid.Price > 5000000)
            {
                auction.LuxuryTax = (int) (auction.HighBid.Price * 0.04);
            }
        }
    }
}