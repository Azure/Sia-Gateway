using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Initialization
{
    public class InvalidTransformTypeException : Exception
    {
        public InvalidTransformTypeException(string transformRuleType, string reducerName)
            : base($"Reducer {reducerName} was configured with invalid transform rule type {transformRuleType}"
                  + "See StateTransformConfiguration for a list of valid transform rule types")
        {

        }
    }
}
