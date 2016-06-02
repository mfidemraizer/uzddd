using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Specification;
using UserZoom.Shared.Redis;
using System.Linq.Expressions;

namespace UserZoom.Shared.Patterns.Repository
{
    public class RedisRepository<TDomainObjectId, TDomainObject> : Repository<TDomainObjectId, TDomainObject>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
    {
        public RedisRepository(IIdGenerator<TDomainObjectId> idGenerator, IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> specs)
            : base(idGenerator, specs)
        {
        }

        private IDatabase Database { get; } = ConnectionMultiplexerFactory.Current.GetDatabase(0);

        public override Task<IMultipleObjectResult<ICollection<TDomainObject>, TDomainObject>> GetByCriteria(Expression<Func<TDomainObject, bool>> criteriaExpr, long from = 0, int count = 10)
        {
            throw new NotImplementedException();
        }

        public async override Task<ISingleObjectResult<TDomainObject>> GetByIdAsync(TDomainObjectId id)
        {
            return new SingleObjectResult<TDomainObject>
            (
                "Ok",
                JsonConvert.DeserializeObject<TDomainObject>
                (
                    await Database.HashGetAsync
                    (
                        $"userzoom:{nameof(TDomainObject).ToLowerInvariant()}",
                        id.ToString()
                    )
                )
            );
        }

        public async override Task<IBasicResult> RemoveAsync(TDomainObject domainObject)
        {
            Contract.Assert(await Database.HashExistsAsync($"userzoom:{nameof(TDomainObject).ToLowerInvariant()}", domainObject.Id.ToString()));

            Contract.Assert
            (
                await Database.HashDeleteAsync
                (
                    $"userzoom:{nameof(TDomainObject).ToLowerInvariant()}",
                    domainObject.Id.ToString()
                )
            );

            return new BasicResult("Ok");
        }

        protected async override Task OnAddAsync(TDomainObject domainObject)
        {
            Contract.Assert(!await Database.HashExistsAsync($"userzoom:{nameof(TDomainObject).ToLowerInvariant()}", domainObject.Id.ToString()));

            await Database.HashSetAsync
            (
                $"userzoom:{nameof(TDomainObject).ToLowerInvariant()}",
                domainObject.Id.ToString(),
                JsonConvert.SerializeObject(domainObject)
            );
        }

        protected async override Task OnUpdateAsync(TDomainObject domainObject)
        {
            Contract.Assert(await Database.HashExistsAsync($"userzoom:{nameof(TDomainObject).ToLowerInvariant()}", domainObject.Id.ToString()));

            await Database.HashSetAsync
            (
                $"userzoom:{nameof(TDomainObject).ToLowerInvariant()}",
                domainObject.Id.ToString(),
                JsonConvert.SerializeObject(domainObject)
            );
        }
    }
}
