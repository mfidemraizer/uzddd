using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared;
using UserZoom.Shared.Patterns.Domain;

namespace UserZoom.Domain
{
    [DebuggerDisplay("Task Id: {Id}")]
    public class UZTask : DomainObject, IEquatable<UZTask>, ICanBeIdentifiable<Guid>, ICanPerformDirtyChecking
    {
        public class IdComparer : IEqualityComparer<UZTask>
        {
            public bool Equals(UZTask x, UZTask y)
            {
                return CheckEquality(x, y);
            }

            public int GetHashCode(UZTask obj)
            {
                if (obj == null)
                    return -1;

                return obj.Id.GetHashCode();
            }
        }

        

        //[Required, MinLength(1)]
        public virtual string Title { get; set; }

        private static bool CheckEquality(UZTask a, UZTask b)
        {
            if (a == null && b == null)
                return false;

            if (a.Id == b.Id)
                return true;

            return false;
        }

        public bool Equals(UZTask other)
            => CheckEquality(this, other as UZTask);

        public override bool Equals(object obj) 
            => CheckEquality(this, obj as UZTask);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(ICanBeIdentifiable<Guid> other)
        {
            throw new NotImplementedException();
        }
    }
}
