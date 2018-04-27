using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Initialization
{
    public class InvalidStateTypeException : Exception
    {
        public InvalidStateTypeException(string stateType, string reducerName)
            : base ($"Reducer {reducerName} was configured invalid state type {stateType}." +
                  " See ReducerConfiguration.ValidStateTypes for a full list of acceptable" +
                  " state types")
        {

        }
    }
}
