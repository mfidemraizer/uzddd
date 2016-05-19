using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Domain.Studies;
using UserZoom.Shared;
using UserZoom.Shared.Patterns.Domain;

namespace UserZoom.Domain.AccountManagement
{
    public class Account : DomainObject
    {
        public virtual ISet<Study> Studies { get; set; }
    }
}
