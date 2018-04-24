using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Initialization
{
    public class InvalidTransformTypeException : Exception
    {
        public InvalidTransformTypeException(string transformType, string reducerName)
            : base($"Reducer {reducerName} had invalid state type {transformType}")
        {

        }
    }
}
