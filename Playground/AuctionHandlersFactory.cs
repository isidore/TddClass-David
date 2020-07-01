namespace Playground
{
    static internal class AuctionHandlersFactory
    {
        public static IHandler[] GetAuctionHandlers()
        {
            var handlers = new IHandler[]
            {
                new DownloadbleSoftwareHandler(), new CarShippingHandler(), new NormalShippingHandler(), new LuxuryTaxHandler(),
                new LogExpensiveItemsHandler(),
            };
            return handlers;
        }
    }
}