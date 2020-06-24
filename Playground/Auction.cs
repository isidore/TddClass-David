﻿using System;

namespace Playground
{
    internal class Auction
    {
        public User Seller { get; }
        public string ItemDescription { get; }
        public double ItemPrice { get; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }

        public Auction(User seller, string itemDescription, double itemPrice, DateTime startDateTime, DateTime endDateTime)
        {
            if (!seller.IsSeller)
            {
                throw new Exception("User is not a seller");
            }

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
    }
}