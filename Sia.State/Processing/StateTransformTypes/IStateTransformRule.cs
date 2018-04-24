using Sia.Data.Incidents.Models;
using Sia.State.Generation.Transform;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Processing.StateTransformTypes
{
    public interface IStateTransformRule<TState>
    {
        IStateTransform<TState> GetTransform(Event ev);
    }

    public interface ITransformMetadata<TMetadata>
    {
        TMetadata Metadata { get; set; }
    }

    public abstract class StateTransformRule<TMetadata, TState>
        : IStateTransformRule<TState>,
        ITransformMetadata<TMetadata>
    {
        public TMetadata Metadata { get; set; }

        public abstract IStateTransform<TState> GetTransform(Event ev);
    }
}
