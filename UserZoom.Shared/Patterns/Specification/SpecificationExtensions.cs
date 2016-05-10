using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.Specification
{
    public static class SpecificationExtensions
    {
        public static IEnumerable<ISpecification<TId, TObject>> GetAddSpecs<TId, TObject>(this IEnumerable<ISpecification<TId, TObject>> specs)
            where TId : IEquatable<TId>
            where TObject : ICanBeIdentifiable<TId>
        {
            return specs.Where(spec => spec.GetType().Name.Contains("Add"));
        }

        public static IEnumerable<ISpecification<TId, TObject>> GetUpdateSpecs<TId, TObject>(this IEnumerable<ISpecification<TId, TObject>> specs)
            where TId : IEquatable<TId>
            where TObject : ICanBeIdentifiable<TId>
        {
            return specs.Where(spec => spec.GetType().Name.Contains("Update"));
        }

        public static async Task<IEnumerable<ISpecification<TId, TObject>>> RunSpecsAsync<TId, TObject>(this IEnumerable<ISpecification<TId, TObject>> specs, TObject objectToValidate)
            where TId : IEquatable<TId>
            where TObject : ICanBeIdentifiable<TId>
        {
            List<ISpecification<TId, TObject>> failedSpecs = new List<ISpecification<TId, TObject>>();

            foreach (ISpecification<TId, TObject> spec in specs)
            {
                if (!await spec.IsSatisfiedByAsync(objectToValidate))
                    failedSpecs.Add(spec);
            }

            return failedSpecs;
        }
    }
}
