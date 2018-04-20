using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Generation.Transform
{
    public interface IStateTransform<TState>
    {
        bool Apply(ref TState currentState);
    }
}
