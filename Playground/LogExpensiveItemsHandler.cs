using Playground;
public class LogExpensiveItemsHandler: IHandler
{
    public  void Handle(Auction auction)
    {
        if (auction.HighBid.Price > 10_000_00)
        {
            auction.logger.Log($"{auction.Seller.UserName} is selling an item of {auction.HighBid.Price} ");
        }
    }
}