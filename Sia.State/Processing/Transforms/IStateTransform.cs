using Newtonsoft.Json.Linq;
using Sia.Data.Incidents.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Processing.Transforms
{
    public interface IStateTransform<TState>
    {
        bool Apply(ref TState currentState);
    }
}
