using Engie;
using Engie.model;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

string ProductionPlan([FromBody] EngieInput body)
{

    EngieChallenge input = body.Create();

    EngieSolver solver = new EngieSolver(input);
    PowerPlantSchedule schedule = solver.solve();
    if(schedule == null)
    {
        return "{\"error\": \"no feasable sollution found\"}";
    }
    return schedule.ToJson();
}

app.MapPost("/productionplan", ProductionPlan);
app.Run("http://localhost:8888");
