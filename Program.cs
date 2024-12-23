using UAV_Tower.Data;
using UAV_Tower.Services;
using Microsoft.EntityFrameworkCore;
using radarApi;
using DataBackup;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;



string controlDump1090 = "dump1090";
var processlerDump1090 = Process.GetProcesses();
bool programVarMiDump1090 = processlerDump1090.Any(p => p.ProcessName.ToLower().Contains(controlDump1090.ToLower()));
if (programVarMiDump1090)
{
    var process = Process.GetProcessesByName(controlDump1090);

    if (process.Length > 0)
    {
        foreach (var p in process)
        {
            p.Kill();
            string sdrRadar = @"C:\UAV_Tower\DesktopLayer\ExternalPrograms\sdr_radar";
            string sdrRadarName = @"dump1090.bat"; //==> Be carefull that name is .bat
            string sdrRadarCommand = ".exe --interactive --net --net-ro-port 30002 --net-beast --mlat --gain 48.0 --quiet --ppm 53 --net-http-port 30003";
            Programs.Programs dump1090 = new Programs.Programs(sdrRadarName, sdrRadar);
            dump1090.Run(sdrRadarCommand);
        }
    }
}
else
{
    string sdrRadar = @"C:\UAV_Tower\DesktopLayer\ExternalPrograms\sdr_radar";
    string sdrRadarName = @"dump1090.bat"; //==> Be carefull that name is .bat
    string sdrRadarCommand = ".exe --interactive --net --net-ro-port 30002 --net-beast --mlat --gain 48.0 --quiet --ppm 53 --net-http-port 30003";
    Programs.Programs dump1090 = new Programs.Programs(sdrRadarName, sdrRadar);
    dump1090.Run(sdrRadarCommand);
}



string controlSatellite = "satellite";
var processlerSatellite = Process.GetProcesses();
bool programVarMiSatellite = processlerSatellite.Any(p => p.ProcessName.ToLower().Contains(controlSatellite.ToLower()));
if (!programVarMiSatellite)
{

    string screenshooter = @"C:\UAV_Tower\DesktopLayer\ExternalPrograms";
    string screenshooterName = @"satellite.exe";
    Programs.Programs satellite = new Programs.Programs(screenshooterName, screenshooter);
    satellite.Run();
}



string controlWeatherAnalysis = "weather_analysis";
var processlerWeatherAnalysis = Process.GetProcesses();
bool programVarMiWeatherAnalysis = processlerWeatherAnalysis.Any(p => p.ProcessName.ToLower().Contains(controlWeatherAnalysis.ToLower()));
if (!programVarMiWeatherAnalysis)
{

    string weatherAnalysis = @"C:\UAV_Tower\DesktopLayer\ExternalPrograms";
    string weatherAnalysisName = @"weather_analysis.exe";
    Programs.Programs weather = new Programs.Programs(weatherAnalysisName, weatherAnalysis);
    weather.Run();
}



// Creating an instance of the BackupProcessor class
BackupProcessor backupProcessor = new BackupProcessor();
string backupLocation = @"C:\UAV_Tower\DataLayer\Backup";
backupProcessor.StartBackup(backupLocation);
Console.WriteLine("Backup completed.");

/*---------------------------------------------------------------------------------------------------------------------------------------------*/




var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserAuthenticationService>();


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddRazorPages();

var app = builder.Build();


var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();


var runFlightDataProcessingAsync = new RunFlightDataProcessingAsync();
var mongoDbTask = runFlightDataProcessingAsync.RunFlightDataProcessingAsyncMongoDB();
var postgreSqlTask = runFlightDataProcessingAsync.RunFlightDataProcessingAsyncPostgreSql();
var redisTask = runFlightDataProcessingAsync.RunFlightDataProcessingAsyncRedis();


lifetime.ApplicationStopping.Register(() => OnApplicationStopping());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapGet("/", () => Results.Redirect("/Account/Login"));
app.MapRazorPages();

//START
app.Run();

await Task.WhenAll(mongoDbTask, postgreSqlTask, redisTask);


void OnApplicationStopping()
{
    Console.WriteLine("STOPPING...");
    Task.Run(async () => await RunTasksBeforeShutdown());
}

async Task RunTasksBeforeShutdown()
{
    controlDump1090 = controlDump1090 + ".exe";
    controlWeatherAnalysis = controlWeatherAnalysis + ".exe";
    controlSatellite = controlSatellite + ".exe";
    Process.GetProcessesByName(controlDump1090).ToList().ForEach(p => p.Kill());
    Process.GetProcessesByName(controlWeatherAnalysis).ToList().ForEach(p => p.Kill());
    Process.GetProcessesByName(controlSatellite).ToList().ForEach(p => p.Kill());
    await Task.WhenAll(mongoDbTask, postgreSqlTask, redisTask);
}
