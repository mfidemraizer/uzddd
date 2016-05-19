using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Domain.AccountManagement;
using UserZoom.Shared;
using UserZoom.Shared.Patterns.Domain;

namespace UserZoom.Domain.Studies
{
    public class Study : DomainObject, IHasTimestamp
    {
        public virtual Account Account { get; set; }
        public virtual DateTimeOffset DateAdded { get; set; }
    }
}
