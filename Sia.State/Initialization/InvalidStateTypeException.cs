using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Initialization
{
    public class InvalidStateTypeException : Exception
    {
        public InvalidStateTypeException(string stateType, string reducerName)
            : base ($"Reducer {reducerName} had invalid state type {stateType}")
        {

        }
    }
}
