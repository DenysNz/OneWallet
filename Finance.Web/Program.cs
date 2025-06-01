using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Finance.Data;
using Finance.Data.Models;
using Finance.Data.Repositories;
using Finance.Data.Repositories.Interfaces;
using Finance.Services;
using Finance.Services.JWtFeature;
using Finance.Services.Services;
using Finance.Services.Services.Interfaces;
using Finance.Web.Filters;
using Finance.Web.Initializers;

var builder = WebApplication.CreateBuilder(args);

// SQL connection
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/& ";
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
        //Other options go here
    }).AddEntityFrameworkStores<FinanceDbContext>()
    .AddDefaultTokenProviders();

//Jwt configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
        ValidAudience = jwtSettings.GetSection("validAudience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});

//Custom Services
builder.Services.AddScoped<JwtHandler>();
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUserDetailsService, UserDetailsService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ISendGridService, SendGridService>();
builder.Services.AddTransient<ISocialService, SocialService>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ISupportRepository, SupportRepository>();
builder.Services.AddScoped<ISupportService, SupportService>();
builder.Services.AddScoped<IPushSubscriptionRepository, PushSubscriptionRepository>();
builder.Services.AddScoped<IPushSubscriptionService, PushSubscriptionService>();
builder.Services.AddScoped<CaptchaActionFilterAttribute>();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

//Json option
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
//Init Identity roles and admin user
using (var scope = app.Services.CreateScope())
{
    await IdentityInitializer.InitAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
