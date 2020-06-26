using Playground;

public interface IAuctionNotifier
{
    bool AppliesTo(Auction auction);


}

public class NoSaleAuctionNotifier : IAuctionNotifier
{
    public bool AppliesTo(Auction auction)
    {
        throw new System.NotImplementedException();
    }
}