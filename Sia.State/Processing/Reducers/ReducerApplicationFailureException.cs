using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Processing.Reducers
{
    public class ReducerApplicationFailureException : Exception
    {
        public ReducerApplicationFailureException(Exception innerException, string reducerName, long eventId)
            : base($"Failure to apply transform triggered by event {eventId.ToPathTokenString()} in reducer {reducerName}", innerException)
        { }
    }
}
