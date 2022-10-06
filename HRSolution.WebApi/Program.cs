using Abp.Extensions;
using HRSolution.Infrastructure.Domain;
using HRSolution.Infrastructure.Domain.Authentication;
using HRSolution.Services;
using HRSolution.Utilities.Common;
using HRSolution.Utilities.ExceptionUtility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

void WriteLogText(string ex, IWebHostEnvironment env)
{


    //HttpContext context = HttpContext.Current;
    String strErrFileName = String.Format("ErrorLog-{0}.txt", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2"));
    string path = env.WebRootPath;

    string file = Path.Combine(path, ResponseErrorMessageUtility.PrivateDocumentFolder + "\\ErrorLog.txt");

    StreamWriter writer;
    bool hasInner = false;
    using (writer = new StreamWriter(file, true))
    {
        writer.WriteLine(string.Format("=========================={0}===========================", DateTime.Now));
        writer.WriteLine(ex);

    }


}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var connect = config.GetSection("ConnectionStrings").Get<List<string>>().FirstOrDefault();


// this will handle ability to change something in your view and see it reflecting without rebuilding 
builder.Services.AddRazorPages()
.AddRazorRuntimeCompilation();




//Configure the Secret Key
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Expiration = TimeSpan.FromMinutes(10);
    });

builder.Services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
});


builder.Services.AddCors(
                options => options.AddPolicy(
                    name: "localhost",
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            config["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );



builder.Services.AddControllers();

//EntityFramework Core
builder.Services.AddDbContext<HRContext>(options => options.UseSqlServer(connect, null));


// For Identity  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HRContext>()
    .AddDefaultTokenProviders();


// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = ConfigHelper.AppSetting("ValidAudience", "JWT"),
        ValidIssuer = ConfigHelper.AppSetting("ValidIssuer", "JWT"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigHelper.AppSetting("Secret", "JWT")))
    };
});

// .NET Native DI Abstraction
RegisterServices(builder.Services);

void RegisterServices(IServiceCollection services)
{
    // Adding dependencies from another layers (isolated from Presentation)

    NativeInjectorBootStrapper.RegisterServices(services);
}


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
