using System;
using System.Collections.Generic;
using eBabyServices;

namespace Playground
{
    public enum AuctionState
    {
        NotStarted,
        Started,
        Closed
    }

    public class Auction
    {
        public User Seller { get; }
        public string ItemDescription { get; }
        public  int ItemPrice { get; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }
        public AuctionState State { get; internal set; }
        public Bid HighBid { get; private set; }


        public Auction(User seller, string itemDescription, int itemPrice, DateTime startDateTime, DateTime endDateTime)
        {
            if (!seller.IsLoggedIn)
            {
                throw new UserNotLoggedInException();
            }

            if (!seller.IsSeller)
            {
                throw new Exception("User is not a seller");
            }

            if (DateTime.Now > startDateTime)
            {
                throw new AuctionInPastException();
            }

            if (endDateTime < startDateTime)
            {
                throw new AuctionEndedBeforeItStartException();
            }
            HighBid = new Bid()
            {
                Price = ItemPrice - 1
            };


            Seller = seller;
            ItemDescription = itemDescription;
            ItemPrice = itemPrice;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }

        public override string ToString()
        {
            return $"{nameof(Seller)}: {Seller}, {nameof(ItemDescription)}: {ItemDescription}, {nameof(ItemPrice)}: {ItemPrice}".Replace(",","\n");
        }

        public void StartAuction()
        {
            State = AuctionState.Started;
        }

        public void Bid(User bidder, int bidAmount)
        {
            if (State == AuctionState.NotStarted)
            {
                throw new AuctionNotStartedCantAcceptBidException();
            }

            if (!bidder.IsLoggedIn)
            {
                throw new AuctionCantAcceptBidSinceBidderNotLoggedInException();
            }
            // When someone bids we compare the bid to existing bids
            // if the bidder's bid amount is higher than the current high bid, then the B's B amt becomes the new high bid
            // if there are no bids yet, then if the bid amount >= the original price, we set this new bid as the high bid
            // if the bidder's bid amount is < the current high bid or original price, we do not assign this as the high bid

            var currentBid = new Bid()
            {
                Price = bidAmount,
                Bidder = bidder
            };

            if (HighBid.Price < currentBid.Price)
            {
                HighBid = currentBid;
            }


        }

        public void OnClose()
        {
            State = AuctionState.Closed;
            var emails = GetClosingEmailNotifications();

            foreach (var email in emails)
            {
                PostOffice.GetInstance().SendEMail(email.Key, email.Value);
            }
        }

        //Making a factory
        //Make Auction Close Notifier Factory
        //method: get auction closer takes an auction and returns an auction close notifation
        //we need an empty auction notifier close no sale
        //not empty auction notifier close sale
        // get closing email notification (seller and the highest bid) return notification


        public Dictionary<string, string> GetClosingEmailNotifications()
        {
            var emails = new Dictionary<string, string>();

            if (HighBid.Bidder == null)
            {
                //send email to seller
                emails.Add(Seller.Email, $"Sorry, your auction for {ItemDescription} did not have any bidders.");
            }
            else
            {
                emails.Add(Seller.Email, $"Your {ItemDescription} auction sold to bidder {HighBid.Bidder.Email} for {HighBid.Price}.");
                emails.Add(HighBid.Bidder.Email, $"Congratulations! You won an auction for a {ItemDescription} from {Seller.Email} for {HighBid.Price}");

            }

            return emails;
        }
    }

    public class Bid
    {
        public int Price { get; set; }
        public User Bidder { get; set; }

    }
}