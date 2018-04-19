﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models.MetadataTypes.Transform
{
    public class PartitionMetadata
    {
        public IList<string> PartitionBySourceKeys { get; set; }
            = new List<string>();
    }
}
