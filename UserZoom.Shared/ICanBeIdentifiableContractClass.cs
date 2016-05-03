using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared
{
    [ContractClassFor(typeof(ICanBeIdentifiable<>))]
    public abstract class ICanBeIdentifiableContractClass<TId> : ICanBeIdentifiable<TId>
        where TId : IEquatable<TId>
    {
        public TId Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(Id != null && !Id.Equals(default(TId)));
        }
    }
}
