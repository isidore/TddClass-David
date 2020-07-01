namespace Playground
{
  public class DownloadbleSoftwareHandler : IHandler
    {
        public  void Handle(Auction auction)
        {
            if (auction.Category == AuctionCategory.DownloadableSoftware)
            {
                auction.ShippingFee = 0;
            }
        }
    }
}