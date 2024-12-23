using UAV_Tower.Data;
using UAV_Tower.Services;
using Microsoft.EntityFrameworkCore;
using radarApi;
using DataBackup;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting; // BÜYÜK HARF: IHostApplicationLifetime için gerekli namespace

// CHECH RunFlightDataProcessingAsync

string controlDump1090 = "dump1090";
// Çalışan süreçlerin listesini alıyoruz
var processlerDump1090 = Process.GetProcesses();
// Belirli bir programın çalışıp çalışmadığını kontrol ediyoruz
bool programVarMiDump1090 = processlerDump1090.Any(p => p.ProcessName.ToLower().Contains(controlDump1090.ToLower()));

if (programVarMiDump1090)
{
    // Programın çalışan süreçlerini alıyoruz
    var process = Process.GetProcessesByName(controlDump1090);

    if (process.Length > 0)
    {
        // Program çalışıyorsa, programı kapatıyoruz
        foreach (var p in process)
        {
            p.Kill();
            //sdr-radar run
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
    //sdr-radar run
    string sdrRadar = @"C:\UAV_Tower\DesktopLayer\ExternalPrograms\sdr_radar";
    string sdrRadarName = @"dump1090.bat"; //==> Be carefull that name is .bat
    string sdrRadarCommand = ".exe --interactive --net --net-ro-port 30002 --net-beast --mlat --gain 48.0 --quiet --ppm 53 --net-http-port 30003";
    Programs.Programs dump1090 = new Programs.Programs(sdrRadarName, sdrRadar);
    dump1090.Run(sdrRadarCommand);
}

string controlSatellite = "satellite";
// Çalışan süreçlerin listesini alıyoruz
var processlerSatellite = Process.GetProcesses();
// Belirli bir programın çalışıp çalışmadığını kontrol ediyoruz
bool programVarMiSatellite = processlerSatellite.Any(p => p.ProcessName.ToLower().Contains(controlSatellite.ToLower()));
if (!programVarMiSatellite)
{
    //satellite-screenshooter
    string screenshooter = @"C:\UAV_Tower\DesktopLayer\ExternalPrograms";
    string screenshooterName = @"satellite.exe"; //==> Be carefull that name is .bat
    Programs.Programs satellite = new Programs.Programs(screenshooterName, screenshooter);
    satellite.Run();
}

string controlWeatherAnalysis = "weather_analysis";
// Çalışan süreçlerin listesini alıyoruz
var processlerWeatherAnalysis = Process.GetProcesses();
// Belirli bir programın çalışıp çalışmadığını kontrol ediyoruz
bool programVarMiWeatherAnalysis = processlerWeatherAnalysis.Any(p => p.ProcessName.ToLower().Contains(controlWeatherAnalysis.ToLower()));

if (!programVarMiWeatherAnalysis)
{
    //weather_analysis
    string weatherAnalysis = @"C:\UAV_Tower\DesktopLayer\ExternalPrograms";
    string weatherAnalysisName = @"weather_analysis.exe"; //==> Be carefull that name is .bat
    Programs.Programs weather = new Programs.Programs(weatherAnalysisName, weatherAnalysis);
    weather.Run();
}

// Creating an instance of the BackupProcessor class
BackupProcessor backupProcessor = new BackupProcessor();
// Calling the StartBackup method on the instance, passing the backup directory
string backupLocation = @"C:\UAV_Tower\DataLayer\Backup"; // You can change this as per your preference // Kullanıcıdan yedekleme dizini alıyoruz
backupProcessor.StartBackup(backupLocation); // Yedekleme işlemini başlatan fonksiyon, parametre olarak yedekleme dizinini alır
// Inform the user that the backup process is complete
Console.WriteLine("Backup completed."); // Yedekleme tamamlandı mesajı
//Environment.Exit(0); // Automatically closes the application after completion













// Asenkron görevlerinizi başlatmadan önce uygulamayı başlatın
var builder = WebApplication.CreateBuilder(args);

// İstenilen bağımlılıkları ve servisleri ekleyin
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserAuthenticationService>();

// Veritabanı bağlantısını ve diğer servisleri ekleyin
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddRazorPages();

var app = builder.Build();

// BÜYÜK HARF: IHostApplicationLifetime hizmetini alıyoruz
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

// **Asenkron görevlerin başlatılması** - app.Run() çağrılmadan önce başlatıyoruz.
var runFlightDataProcessingAsync = new RunFlightDataProcessingAsync();
var mongoDbTask = runFlightDataProcessingAsync.RunFlightDataProcessingAsyncMongoDB();
var postgreSqlTask = runFlightDataProcessingAsync.RunFlightDataProcessingAsyncPostgreSql();
var redisTask = runFlightDataProcessingAsync.RunFlightDataProcessingAsyncRedis();

// BÜYÜK HARF: Uygulama kapanmadan önce çalışacak fonksiyonu kaydediyoruz
lifetime.ApplicationStopping.Register(() => OnApplicationStopping());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Giriş ekranı ve Razor Pages yapılandırmaları
app.MapGet("/", () => Results.Redirect("/Account/Login"));
app.MapRazorPages();

// Uygulamanın ana işleyişini başlat
app.Run();

await Task.WhenAll(mongoDbTask, postgreSqlTask, redisTask);  // Bu görevlerin tamamlanmasını bekleriz

// BÜYÜK HARF: Uygulama kapanmadan önce yapılacak işlemleri tanımladık
void OnApplicationStopping()
{
    // BÜYÜK HARF: Kapanmadan önce çalışacak işlemleri burada başlatıyoruz
    Console.WriteLine("Uygulama kapanmadan önce çalıştırılıyor...");

    // BÜYÜK HARF: Bu metod, görevlerinizi başlatmak için çağrılıyor
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
    // BÜYÜK HARF: Asenkron görevlerin tamamlanmasını bekliyoruz
    await Task.WhenAll(mongoDbTask, postgreSqlTask, redisTask); // Uygulama kapanmadan önce tüm görevler tamamlanacak
}
