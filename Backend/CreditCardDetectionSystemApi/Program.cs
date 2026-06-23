using CreditCard_BusinessLayer;
using CreditCard_BusinessLayer.Services;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//1.إعداد الـ JWT Options من ملف appsettings
builder.Services.Configure<md_JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
var jwtOptions = builder.Configuration.GetSection("JwtSettings").Get<md_JwtOptions>();

//إضافة الوصول إلى الـ HttpContext داخل الـ Services
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<PersonService>();

builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<OtpWhatsappNotifications>();

builder.Services.AddMemoryCache();

//بعد ذلك سجل خدمتك الخاصة
builder.Services.AddScoped<AccessControlService>();
//2.إعداد خدمة التحقق(Authentication)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ClockSkew = TimeSpan.Zero
    };
});

// 3. تسجيل الخدمات (Dependency Injection)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FraudShield API", Version = "v1" });

    // 1. تعريف نظام الحماية (JWT)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "ادخل التوكن فقط (بدون كلمة Bearer إذا كان النوع Http Scheme)"
    });

    //   2.جعل الحماية متطلباً لكل الدالات المحمية بـ[Authorize]
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
});


    //ل// الـ AuthService التي سننشئها لاحقاً
    builder.Services.AddScoped<AuthService>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("FraudSystemPolicy", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    });

    builder.Services.AddHttpClient<TransactionsService>(client =>
    {
        client.BaseAddress = new Uri("http://127.0.0.1:5000/");
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    });

    builder.Services.AddHttpClient<GeoService>();
    builder.Services.AddScoped<CardsService>();
    builder.Services.AddScoped<TransactionsRepository>();

    builder.Services.AddLogging(logging =>
    {
        logging.AddConsole();
        logging.AddDebug();
    });
    builder.Services.AddScoped<IEncryptionService, EncryptionService>();
   var app = builder.Build();

    // --- إعداد الـ Pipeline (الترتيب الصحيح) ---

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseCors("FraudSystemPolicy");

    app.UseAuthentication(); // تحديد "من أنت"
    app.UseAuthorization();  // تحديد "ماذا تفعل"

    app.MapControllers();
    app.Run();
