namespace Playground
{
    static internal class Handler
    {
        public static void Handle(Auction auction)
        {
            if (auction.Category == AuctionCategory.DownloadableSoftware)
            {
                auction.ShippingFee = 0;
            }
        }
    }
}