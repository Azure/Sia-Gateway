using Sia.Data.Incidents.Models;
using Sia.Domain;
using Sia.Shared.Protocol.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sia.Gateway.Protocol.Pagination
{
    public class EventTimelinePaginationRequest
        : PaginationByCursorRequest<Data.Incidents.Models.Event, Domain.Event, TimelinePaginationCursor>
    {
        public long CursorId { get; set; }
        public DateTime CursorTime { get; set; }
        public override IPaginationByCursorSelectors<Data.Incidents.Models.Event, Domain.Event, TimelinePaginationCursor> Selectors()
            => _selectorsInstance;

        private EventTimelinePaginationSelectors _selectorsInstance { get; }
            = new EventTimelinePaginationSelectors();


        public override TimelinePaginationCursor GetCursorValue()
            => new TimelinePaginationCursor()
            {
                CursorId = CursorId,
                CursorTime = CursorTime
            };


    }

    public class EventTimelinePaginationSelectors
        : PaginationByCursorSelectors<Data.Incidents.Models.Event, Domain.Event, TimelinePaginationCursor>
    {
        public override Func<Domain.Event, TimelinePaginationCursor> DtoValueSelector
            => (ev) => new TimelinePaginationCursor()
            {
                CursorId = ev.Id,
                CursorTime = ev.Occurred
            };

        public override Expression<Func<Data.Incidents.Models.Event, TimelinePaginationCursor>> DataValueSelector
            => (ev) => new TimelinePaginationCursor()
            {
                CursorId = ev.Id,
                CursorTime = ev.Occurred
            };
    }
}
