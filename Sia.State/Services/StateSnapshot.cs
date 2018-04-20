using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State
{
    public class StateSnapshot
    {
        public object State { get; set; }
        public DateTime AsOf { get; set; }
            = DateTime.UtcNow;
        public DateTime LastAccessed { get; set; }
            = DateTime.UtcNow;
    }
}
