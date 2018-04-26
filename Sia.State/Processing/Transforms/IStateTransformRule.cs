using Sia.Data.Incidents.Models;

namespace Sia.State.Processing.Transforms
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
