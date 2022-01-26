using Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var dbString = builder.Configuration.GetConnectionString("SchoolContext");
builder.Services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(dbString));
builder.Services.AddScoped<ISchoolService, SchoolService>();
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

app.MapGet("/api/school/quick", () => Results.Ok("Hello world"));
app.MapGet("/api/school/enrollments", async (ISchoolService schoolService) => Results.Ok(await schoolService.GetEnrollmentCount()));
app.MapGet("/api/school/enrollments/{start}/{count}", async (int start, int count, ISchoolService schoolService) 
    => Results.Ok(await schoolService.GetEnrollments(start, count)));


app.Run("https://localhost:7011");
