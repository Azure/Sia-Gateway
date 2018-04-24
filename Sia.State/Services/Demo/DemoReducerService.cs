using System;
using System.Collections.Generic;
using System.Text;
using Sia.State.Filters;
using Sia.State.MetadataTypes.Transform;
using Sia.State.Processing.Reducers;
using Sia.State.Processing.StateSliceTypes;
using Sia.State.Processing.StateTransformTypes;

namespace Sia.State.Services.Demo
{
    public class DemoReducerService : FromCodeReducerService
    {
        public DemoReducerService() 
            : base(demoReducer)
        {
        }

        private static CombinedReducer demoReducer = new CombinedReducer()
        {
            Name = "Root",
            ApplicableEvents = new EventFilters()
            {
                EventTypes = new List<long>() { 1 }
            },
            Children = new Dictionary<string, IReducer>()
            {
                {
                    "Participants",
                    new CombinedReducer()
                    {
                        Name = "Root.Participants",
                        ApplicableEvents = new EventFilters()
                        {
                            EventTypes = new List<long>() {1}
                        },
                        Children = new Dictionary<string, IReducer>()
                        {
                            {
                                "Joined",
                                new Reducer<Map>()
                                {
                                    Name = "Root.Participants.Joined",
                                    InitialState = new Map(),
                                    ApplicableEvents = new EventFilters()
                                    {
                                        EventTypes = new List<long>() {1}
                                    },
                                    Cases = new List<ReducerCase<Map>>
                                    {
                                        new ReducerCase<Map>()
                                        {
                                            MatchTriggeringEvents = new EventFilters()
                                            {
                                                EventTypes = new List<long>() {1},
                                                DataKey = "action",
                                                DataValue = "Joined"
                                            },
                                            StateTransformToApply = new AddToMapRule()
                                            {
                                                Metadata = new PartitionMetadata()
                                                {
                                                    PartitionBySourceKeys = new List<string>()
                                                    {
                                                        "service",
                                                        "team",
                                                        "alias"
                                                    }
                                                }
                                            }
                                        },
                                        new ReducerCase<Map>()
                                        {
                                            MatchTriggeringEvents = new EventFilters()
                                            {
                                                EventTypes = new List<long>() {1},
                                                DataKey = "action",
                                                DataValue = "Left"
                                            },
                                            StateTransformToApply = new RemoveFromMapRule()
                                            {
                                                Metadata = new PartitionMetadata()
                                                {
                                                    PartitionBySourceKeys = new List<string>()
                                                    {
                                                        "service",
                                                        "team",
                                                        "alias"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "Left",
                                new Reducer<Map>()
                                {
                                    Name = "Root.Participants.Left",
                                    InitialState = new Map(),
                                    ApplicableEvents = new EventFilters()
                                    {
                                        EventTypes = new List<long>() {1}
                                    },
                                    Cases = new List<ReducerCase<Map>>
                                    {
                                        new ReducerCase<Map>()
                                        {
                                            MatchTriggeringEvents = new EventFilters()
                                            {
                                                EventTypes = new List<long>() {1},
                                                DataKey = "action",
                                                DataValue = "Left"
                                            },
                                            StateTransformToApply = new AddToMapRule()
                                            {
                                                Metadata = new PartitionMetadata()
                                                {
                                                    PartitionBySourceKeys = new List<string>()
                                                    {
                                                        "service",
                                                        "team",
                                                        "alias"
                                                    }
                                                }
                                            }
                                        },
                                        new ReducerCase<Map>()
                                        {
                                            MatchTriggeringEvents = new EventFilters()
                                            {
                                                EventTypes = new List<long>() {1},
                                                DataKey = "action",
                                                DataValue = "Joined"
                                            },
                                            StateTransformToApply = new RemoveFromMapRule()
                                            {
                                                Metadata = new PartitionMetadata()
                                                {
                                                    PartitionBySourceKeys = new List<string>()
                                                    {
                                                        "service",
                                                        "team",
                                                        "alias"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            InitialState = new Dictionary<string, object>()
            {
                {
                    "Participants",
                    new Dictionary<string, object>()
                    {
                        {
                            "Joined",
                            new Map()
                        },
                        {
                            "Left",
                            new Map()
                        }
                    }
                }
            }
        };
    }
}
