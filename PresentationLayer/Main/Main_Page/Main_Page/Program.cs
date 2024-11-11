using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);



// Servisleri ekle
builder.Services.AddRazorPages();

var app = builder.Build();

// Uygulama yap�land�rmas�
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Geli�tirme modunda hata sayfas�
}
else
{
    app.UseExceptionHandler("/Error"); // Hata sayfas�
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Ana sayfay� Login sayfas�na y�nlendirme
app.MapGet("/", () => Results.Redirect("/MainPage"));

app.MapRazorPages();

app.Run();

