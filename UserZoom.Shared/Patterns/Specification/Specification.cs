using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using FluentValidation.Results;
using UserZoom.Shared.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserZoom.Shared.Patterns.Specification
{
    public abstract class Specification<TId, TObject> : AbstractValidator<TObject>, ISpecification<TId, TObject>
        where TId : IEquatable<TId>
        where TObject : ICanBeIdentifiable<TId>
    {
        protected Dictionary<string, string> BrokenRulesInternal { get; } = new Dictionary<string, string>();
        public IImmutableDictionary<string, string> BrokenRules => BrokenRulesInternal.ToImmutableDictionary();

        public virtual async Task<bool> IsSatisfiedByAsync(TObject someObject)
        {
            FluentValidation.Results.ValidationResult result = await ValidateAsync(someObject);

            BrokenRulesInternal.AddRange
            (
                result.Errors.ToDictionary
                (
                    error => error.PropertyName,
                    error => error.ErrorMessage
                )
            );

            List<System.ComponentModel.DataAnnotations.ValidationResult> dataAnnotationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext context = new System.ComponentModel.DataAnnotations.ValidationContext(someObject);

            if (!Validator.TryValidateObject(someObject, context, dataAnnotationResults, true))
            {
                BrokenRulesInternal.AddRange
                (
                    dataAnnotationResults.ToDictionary
                    (
                        r => r.MemberNames.First(),
                        r => r.ErrorMessage
                    )
                );
            }

            return BrokenRulesInternal.Count == 0;
        }
    }
}
