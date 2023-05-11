using BlazorApp1.Shared;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlazorApp1.Server.GrpcServices
{
    
    public class WeatherService : WeatherForecasts.WeatherForecastsBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        [Authorize]
        public override Task<WeatherReply> GetWeather(Empty request, ServerCallContext context)
        {
            var reply = new WeatherReply();
            var rng = new Random();

            var httpContext = context.GetHttpContext();
            //var userId = httpContext.User.Claims
            //.Where(_ => _.Type == ClaimTypes.NameIdentifier)
            //.Select(_ => Guid.Parse(_.Value))
            //.First();

            reply.Forecasts.Add(Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }));

            return Task.FromResult(reply);
        }
    }


}
