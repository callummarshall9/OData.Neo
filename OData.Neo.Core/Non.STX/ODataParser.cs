using Microsoft.Extensions.DependencyInjection;
using OData.Neo.Core.Services.Orchestrations.OTokenizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OData.Neo.Core.Non.STX
{
    public class ODataHandler
    {
        private readonly IOTokenizationOrchestrationService tokenizationOrchestrationService;
        private readonly IServiceProvider serviceProvider;

        public ODataHandler(IOTokenizationOrchestrationService tokenizationOrchestrationService,
            IServiceProvider serviceProvider)
        {
            this.tokenizationOrchestrationService = tokenizationOrchestrationService;
            this.serviceProvider = serviceProvider;
        }

        public async Task<IQueryable<T>> HandleAsync<T>(IQueryable<T> data, string query) where T : class
        {
            var rootToken = tokenizationOrchestrationService.OTokenizeQuery(query);

            var hasExpand = rootToken.Children.Any(c => 
                c.ProjectedType == Models.ProjectedTokens.ProjectedTokenType.Keyword 
                && c.RawValue == "$expand");

            if (hasExpand)
            {
                IEnumerable<IODataExpandRelationshipHandler<T>> handlers = serviceProvider.GetServices<IODataExpandRelationshipHandler<T>>();

                var expansionTokens = rootToken.Children.Where(c =>
                    c.ProjectedType == Models.ProjectedTokens.ProjectedTokenType.Keyword
                    && c.RawValue == "$expand")
                    .ToArray();

                var requestedPropertyNames = expansionTokens
                    .SelectMany(e => e.Children)
                    .Where(c => c.ProjectedType == Models.ProjectedTokens.ProjectedTokenType.Property)
                    .Select(c => c.RawValue)
                    .ToArray();

                foreach (var handler in handlers)
                    await handler.HandleRelationshipAsync(data, requestedPropertyNames);
            }

            var hasFilter = rootToken.Children.Any(c =>
                c.ProjectedType == Models.ProjectedTokens.ProjectedTokenType.Keyword
                && c.RawValue == "$filter");

            if (hasFilter)
            {
                IEnumerable<IODataFilterRelationshipHandler<T>> handlers = serviceProvider.GetServices<IODataFilterRelationshipHandler<T>>();

                var filterTokens = rootToken.Children.Where(c =>
                    c.ProjectedType == Models.ProjectedTokens.ProjectedTokenType.Keyword
                    && c.RawValue == "$filter")
                    .ToArray();

                foreach (var handler in handlers)
                    await handler.HandleRelationshipAsync(data, filterTokens);
            }

            throw new NotFiniteNumberException();
        }
    }
}
