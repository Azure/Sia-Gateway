﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models.Configuration
{
    public class EventDataTransform
    {
        public string SourceKey { get; set; }
        public string DestinationIntermediateKey { get; set; }
        public string Default { get; set; }
            = string.Empty;
        public bool DefaultOnEmptyString { get; set; }
           = false;
    }
}
