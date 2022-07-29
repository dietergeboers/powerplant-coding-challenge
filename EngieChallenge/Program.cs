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
    try
    {
    EngieChallenge input = body.Create();
    EngieSolver solver = new EngieSolver(input);
    PowerPlantSchedule schedule = solver.solve();
    if(schedule == null)
    {
        return "{\"error\": \"no feasible solution found\"}";
    }
    return schedule.ToJson();


    } catch(Exception e)
    {
        return e.ToString(); 
    }
}

app.MapPost("/productionplan", ProductionPlan);
app.Run("http://localhost:8888");
