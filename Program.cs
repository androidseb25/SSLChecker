using Quartz;
using SSLChecker.Jobs;
using Environments = SSLChecker.Environments;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    int startTimeHour = Environments.StartHour;
    int startTimeMinute = Environments.StartMinute;
    var jobKey = new JobKey("SSLCheckerJob");
    q.AddJob<SSLCheckerJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("SSLCheckerJob-trigger")
        .WithDailyTimeIntervalSchedule(s =>
            s.WithIntervalInHours(Environments.CheckInterval)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(startTimeHour, startTimeMinute)))
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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
