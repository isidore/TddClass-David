using System;
using System.Timers;

namespace eBabyServices
{
    public class AuctionTimer
    {
        private Auctionable auctions;
        private Timer timer;

        public void CheckAuction(Auctionable auctions)
        {
            this.auctions = auctions;
        }

        public void Start()
        {
            timer = new Timer();
            timer.Elapsed += TimerTick;
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            auctions.HandleAuctionEvents(DateTime.Now);
        }
    }
}