
using OData.Neo.Core.Brokers.Expressions;
using OData.Neo.Core.Services.Coordinations.OQueries;
using OData.Neo.Core.Services.Foundations.OExpressions;
using OData.Neo.Core.Services.Foundations.OTokenizations;
using OData.Neo.Core.Services.Foundations.Projections;
using OData.Neo.Core.Services.Foundations.Tokenizations;
using OData.Neo.Core.Services.Orchestrations.OQueries;
using OData.Neo.Core.Services.Orchestrations.OTokenizations;

namespace OData.Neo.Core.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<IOQueryCoordinationService, OQueryCoordinationService>();

            builder.Services.AddTransient<IOTokenizationOrchestrationService, OTokenizationOrchestrationService>();
            builder.Services.AddTransient<IOQueryOrchestrationService, OQueryOrchestrationService>();

            builder.Services.AddTransient<ITokenizationService, TokenizationService>();
            builder.Services.AddTransient<ITokenizationValidationService, TokenizationValidationService>();

            builder.Services.AddTransient<IProjectionService, ProjectionService>();
            builder.Services.AddTransient<IProjectionValidationService, ProjectionValidationService>();

            builder.Services.AddTransient<IOTokenizationService, OTokenizationService>();
            builder.Services.AddTransient<IOTokenizationValidationService, OTokenizationValidationService>();

            builder.Services.AddTransient<IOQueryOrchestrationService, OQueryOrchestrationService>();
            builder.Services.AddTransient<IOExpressionService, OExpressionService>();

            builder.Services.AddTransient<IExpressionBroker, ExpressionBroker>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
