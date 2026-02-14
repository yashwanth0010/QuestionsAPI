using Microsoft.EntityFrameworkCore;
using QuestionsAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuestionsDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQL"))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapGet("/questions", async (QuestionsDbContext context) =>
{
    var result = await context.QuestionsAndAnswers
        .Select(q => new QuestionsAndAnswers
        {
            Id = q.Id,
            Text = q.Text,
            Answer = q.Answer
        })
        .ToListAsync();

    return Results.Ok(result);
});

app.Run();
