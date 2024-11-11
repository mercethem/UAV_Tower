using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);



// Servisleri ekle
builder.Services.AddRazorPages();

var app = builder.Build();

// Uygulama yapýlandýrmasý
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Geliþtirme modunda hata sayfasý
}
else
{
    app.UseExceptionHandler("/Error"); // Hata sayfasý
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Ana sayfayý Login sayfasýna yönlendirme
app.MapGet("/", () => Results.Redirect("/MainPage"));

app.MapRazorPages();

app.Run();

