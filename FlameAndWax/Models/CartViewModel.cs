﻿using FlameAndWax.Data.Constants;
using System.Collections.Generic;

namespace FlameAndWax.Models
{
    public class CartViewModel
    {        
        public Constants.ModeOfPayment ModeOfPayment { get; set; }
        public Constants.Courier Courier { get; set; }
        public IList<ProductViewModel> CartProducts { get; set; }
        
    }
}
