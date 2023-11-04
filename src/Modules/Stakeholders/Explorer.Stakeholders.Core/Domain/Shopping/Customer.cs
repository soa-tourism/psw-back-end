﻿using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.Shopping
{
    public class Customer: Entity
    {
        public long TouristId { get; init; }
        public List<TourPurchaseToken>? PurchaseTokens { get; init; }
        public long ShoppingCartId { get; init; } 

        public Customer() { }
        public Customer(long toruistId, long shoppingCartId)
        {
            TouristId = toruistId;
            PurchaseTokens = new List<TourPurchaseToken>();
            ShoppingCartId = shoppingCartId;
        }

        //ovo zovem iz nekog servisa 
        public void CustomersPurchaseTokens (TourPurchaseToken purchaseToken)
        {
            PurchaseTokens.Add(purchaseToken);
        }
    }
}