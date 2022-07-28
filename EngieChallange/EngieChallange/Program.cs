using Engie;
using Engie.model;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

string ProductionPlan([FromBody] EngieInput body)
{
    TopX<I> test = new TopX<I>(new CompareI(),2);
    test.Add(new I(3));
    test.Add(new I(4));
    test.Add(new I(1));
    while (test.Count > 0)
        Console.WriteLine(test.Dequeue().i);

    EngieChallange input = body.Create();

    EngieSolver solver = new EngieSolver(input);
    PowerPlantSchedule schedule = solver.solve(-1);

    return schedule.ToJson();
}

app.MapPost("/productionplan", ProductionPlan);
app.Run("https://localhost:8000");
