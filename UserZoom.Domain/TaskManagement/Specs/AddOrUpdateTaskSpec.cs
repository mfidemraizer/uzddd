using FluentValidation;
using System;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.Specification;

namespace UserZoom.Domain.TaskManagement.Specs
{ 
    public sealed class AddOrUpdateTaskSpec : Specification<Guid, UZTask>
    {
        public AddOrUpdateTaskSpec()
        {
            RuleFor(t => t.Title).Length(1, 255);
        }

        public override async Task<bool> IsSatisfiedByAsync(UZTask someObject)
        {
            //if(string.IsNullOrEmpty(someObject.Title))
            //{
            //    BrokenRulesInternal.Add("Title", "A task must provide a title");
            //}

            return await base.IsSatisfiedByAsync(someObject);
        }
    }
}
