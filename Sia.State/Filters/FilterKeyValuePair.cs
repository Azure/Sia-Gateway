using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Filters
{
    public class FilterKeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// Anonymous type used to avoid reimplementing GetHashCode
        /// See https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        /// </summary>
        private object _hashableMe;
        internal object HashableMe
        {
            get
            {
                if (_hashableMe is null)
                {
                    _hashableMe = new { Key, Value };
                }
                return _hashableMe;
            }
        }
    }

    public class FilterKeyValuePairComparer : IEqualityComparer<FilterKeyValuePair>
    {
        public bool Equals(FilterKeyValuePair x, FilterKeyValuePair y)
            => string.Equals(x.Key, y.Key, StringComparison.InvariantCulture)
            && string.Equals(x.Value, y.Value, StringComparison.InvariantCulture);
        public int GetHashCode(FilterKeyValuePair obj) => obj.HashableMe.GetHashCode();
    }
}
