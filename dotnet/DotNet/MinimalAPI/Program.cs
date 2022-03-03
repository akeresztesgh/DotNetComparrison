using Database;
using Microsoft.AspNetCore.Mvc;
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
app.MapPost("/api/school/enrollments/enrollmentsSplit", async ([FromBody] List<long> enrollmentIds, ISchoolService schoolService)
    => Results.Ok(await schoolService.GetEnrollmentsSplit(enrollmentIds)));
app.MapPost("/api/school/enrollments/v1", async ([FromBody] PagingViewModel paging, ISchoolService schoolService)
    => {
        if (paging == null || paging.Count > 500)
        {
            Results.BadRequest();
        }
        Results.Ok(await schoolService.GetEnrollmentsV1(paging));
        });
app.MapPost("/api/school/enrollments/v2", async ([FromBody] PagingViewModel paging, ISchoolService schoolService)
    =>
        {
            if (paging == null || paging.Count > 500)
            {
                Results.BadRequest();
            }
            if (string.IsNullOrWhiteSpace(paging.OrderBy))
            {
                paging.OrderBy = "enrollmentid";
            }

            try
            {
                Results.Ok(await schoolService.GetEnrollmentsV2(paging));
            }
            catch (Exception ex)
            {
                Results.BadRequest(ex.ToString());
            }
        });


app.Run("https://localhost:7011");
