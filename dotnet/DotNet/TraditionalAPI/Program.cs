using Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbString = builder.Configuration.GetConnectionString("SchoolContext");
builder.Services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(dbString));

var dbOptions = new DbContextOptionsBuilder<SchoolContext>()
        .UseSqlServer(dbString)
        .Options;
builder.Services.AddSingleton(ctx => new Func<SchoolContext>(() => new SchoolContext(dbOptions)));

builder.Services.AddScoped<ISchoolService, SchoolService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
