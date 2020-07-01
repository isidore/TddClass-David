namespace Playground
{
  public  class CarShippingHandler: IHandler
    {
        public  void Handle(Auction auction)
        {
            if (auction.Category == AuctionCategory.Car)
            {
                auction.logger.Log($"{auction.Seller.UserName} is selling a {auction.ItemDescription} ");
                auction.ShippingFee = 100000;
            }
        }
    }
}