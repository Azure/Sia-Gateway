using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Generation.Transform
{
    public class StateTransform<T>
    {
        public string Key { get; set; }
        public StateTransformOperation Operation { get; set; }
        public string Value { get; set; }
        public T Metadata { get; set; }

        public JObject Apply(JObject existingState)
        {
            switch(Operation)
            {
                case StateTransformOperation.AddToMap:
                    return AddToMap(existingState);
                case StateTransformOperation.RemoveFromMap:
                    return RemoveFromMap(existingState);
                default:
                    return existingState;
            }
        }

        private JObject AddToMap(JObject existingState)
        {
            if (existingState.)
        }

        private JObject RemoveFromMap(JObject existingState)
        {

        }
    }

    public enum StateTransformOperation
    {
        AddToMap,
        RemoveFromMap
    }
}
