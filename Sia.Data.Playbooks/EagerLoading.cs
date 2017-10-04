using Microsoft.EntityFrameworkCore;
using Sia.Data.Playbooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Data.Playbooks
{
    public static class EagerLoading
    {
        public static IQueryable<EventType> WithEagerLoading(this DbSet<EventType> table)
        {
            return table
                .Include(et => et.ActionAssociations)
                    .ThenInclude(ettaa => ettaa.Action)
                        .ThenInclude(act => act.ActionTemplate)
                            .ThenInclude(at => at.Sources)
                .Include(et => et.ActionAssociations)
                    .ThenInclude(ettaa => ettaa.Action)
                        .ThenInclude(act => act.ConditionSets)
                            .ThenInclude(cs => cs.Conditions)
                                .ThenInclude(cond => cond.ConditionSource);
        }
    }
}
