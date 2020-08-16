﻿using Detached.Samples.RestApi.Models.Core;
using System;

namespace Detached.Samples.RestApi.Models.Outputs
{
    public class InvoiceListItemOutput
    {
        public int Id { get; set; }
 
        public Item Type { get; set; }
 
        public Item Customer { get; set; }

        public DateTime DateTime { get; set; }
    }
}
