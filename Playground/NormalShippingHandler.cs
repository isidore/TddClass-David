namespace Playground
{
    public  class NormalShippingHandler: IHandler
    {
        public  void Handle(Auction auction)
        {
            if (auction.Category == AuctionCategory.None)
            {
                auction.ShippingFee = 1000;
            }
        }
    }
}