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
//Vytvoøte informaèní systém knihovny, který umožòuje ètenáøùm vyhledávat jednotlivé tituly podle
//autora/ù, názvu, ISBN, žánru, klíèových slov, atd. Každý titul mùže být v knihovnì ve více výtiscích a
//mùže spadat do více žánrù (nauèná, historická literatura, beletrie, ...). U každého autora je nutno uchovat
//základní informace (jméno, pøíjmení, datum narození/úmrtí, jeho literární styl-žánr, ...) Systém bude
//umožòovat spravovat informace jak o zamìstnancích knihovny, tak o jejich ètenáøích. Systém umožòuje
//ètenáøùm rezervovat si jednotlivé tituly. Ètenáø si mùže vypùjèit èi rezervovat více titulù najednou,
//pøièemž rezervace se provádí na titul, zatímco výpùjèka na konkrétní výtisk. Pokud nìkterý z titulù není
//vrácen v termínu, je ètenáøi udìlena pokuta. U jednotlivých výpùjèek je zaznamenán údaj o tom, který z
//knihovníkù výpùjèku vytvoøil a kdo ji uzavøel (pøevzal vrácené knihy). Systém také disponuje funkcí pro
//zasílání upomínek - pøed/po vypršení termínu výpùjèky.