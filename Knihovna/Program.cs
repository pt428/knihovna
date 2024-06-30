using Knihovna.Models;
using Knihovna.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(
		builder.Configuration["ConnectionStrings:MonsterDb"]);
});
builder.Services.AddScoped<BookService>();
//builder.Services.AddScoped<EmployeeService>();
//builder.Services.AddScoped<ReaderService>();
builder.Services.AddScoped<BorrowedService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<LibraryUserService>();
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(opt =>
{
	opt.Password.RequiredLength = 8;
	opt.Password.RequireLowercase = true;
});
builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Account/Login");
builder.Services.ConfigureApplicationCookie(opt =>
{
	opt.Cookie.Name = ".AspNetCore.Identity.Application";
	opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
	opt.SlidingExpiration = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
//24.Knihovna
//Vytvo�te informa�n� syst�m knihovny, kter� umo��uje �ten���m vyhled�vat jednotliv� tituly podle
//autora/�, n�zvu, ISBN, ��nru, kl��ov�ch slov, atd. Ka�d� titul m��e b�t v knihovn� ve v�ce v�tisc�ch a
//m��e spadat do v�ce ��nr� (nau�n�, historick� literatura, beletrie, ...). U ka�d�ho autora je nutno uchovat
//z�kladn� informace (jm�no, p��jmen�, datum narozen�/�mrt�, jeho liter�rn� styl-��nr, ...) Syst�m bude
//umo��ovat spravovat informace jak o zam�stnanc�ch knihovny, tak o jejich �ten���ch. Syst�m umo��uje
//�ten���m rezervovat si jednotliv� tituly. �ten�� si m��e vyp�j�it �i rezervovat v�ce titul� najednou,
//p�i�em� rezervace se prov�d� na titul, zat�mco v�p�j�ka na konkr�tn� v�tisk. Pokud n�kter� z titul� nen�
//vr�cen v term�nu, je �ten��i ud�lena pokuta. U jednotliv�ch v�p�j�ek je zaznamen�n �daj o tom, kter� z
//knihovn�k� v�p�j�ku vytvo�il a kdo ji uzav�el (p�evzal vr�cen� knihy). Syst�m tak� disponuje funkc� pro
//zas�l�n� upom�nek - p�ed/po vypr�en� term�nu v�p�j�ky.