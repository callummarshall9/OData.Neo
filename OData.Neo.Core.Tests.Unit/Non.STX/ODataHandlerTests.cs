using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OData.Neo.Core.Models.OTokens;
using OData.Neo.Core.Non.STX;
using OData.Neo.Core.Services.Foundations.OTokenizations;
using OData.Neo.Core.Services.Foundations.Projections;
using OData.Neo.Core.Services.Foundations.Tokenizations;
using OData.Neo.Core.Services.Orchestrations.OTokenizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OData.Neo.Core.Tests.Unit.Non.STX
{
    public class ODataHandlerTests
    {
        [Fact]
        public async Task ShouldHandleCorrectly()
        {
            //given
            IServiceCollection serviceCollection = new ServiceCollection();

            var orchestrationService = new OTokenizationOrchestrationService(
                new TokenizationService(new TokenizationValidationService()),
                new ProjectionService(new ProjectionValidationService()),
                new OTokenizationService(new OTokenizationValidationService()));

            var foos = new Foo[]
            {
                new Foo()
                {
                    Id = Guid.NewGuid(),
                    Name = "Foo A"
                },
                new Foo()
                {
                    Id = Guid.NewGuid(),
                    Name = "Foo B"
                }
            };

            var bars = foos.SelectMany(f => Enumerable.Range(1, new Random().Next(2, 20))
                .Select((i, _) => new Bar() { FooId = f.Id, Name = $"{f.Name} - {i}" }))
                .ToArray();

            var testHandler = new FooHandlerTest(bars);

            serviceCollection.AddTransient<IODataExpandRelationshipHandler<Foo>>(_ => testHandler);

            var handler = new ODataHandler(orchestrationService, serviceCollection.BuildServiceProvider());

            await handler.HandleAsync(foos.AsQueryable(), "$expand=Bars");

            string x = "foo";
        }
    }

    public class FooHandlerTest : IODataExpandRelationshipHandler<Foo>
    {
        private IEnumerable<Bar> barStore { get; set; }

        public FooHandlerTest(IEnumerable<Bar> barStore)
        {
            this.barStore = barStore;
        }

        public Task HandleRelationshipAsync(IQueryable<Foo> data, IEnumerable<string> properties)
        {
            if (properties.Contains("Bars"))
            {
                foreach(var foo in data)
                    foo.Bars = barStore.Where(b => b.FooId == foo.Id).ToArray();
            }

            return Task.CompletedTask;
        }
    }

    public class Foo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Bar> Bars { get; set; }
    }
    
    public class Bar
    {
        public string Name { get; set; }
        public Guid FooId { get; set; }
    }
}
