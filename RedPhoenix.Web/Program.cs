using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RedPhoenix.Module;
using RedPhoenix.Web.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using RedPhoenix.Web.Hub;
using Microsoft.ApplicationInsights.DependencyCollector;
using RedPhoenix.Data.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(ConfigureSwaggerGen);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>(
    (module, o) => { module.EnableSqlCommandTextInstrumentation = true; }
);
builder.Services.AddLogging();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(b =>
{
    b.RegisterModule(new AppModule(GetSetting(builder.Configuration)));
}));

builder.Services.AddScoped(container =>
{
    var loggerFactory = container.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

    return new ClientIpCheckActionFilter(
        builder.Configuration["AdminSafeList"], logger);

});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(ConfigureJwtBearer);

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
    config.AddPolicy(Policies.User, Policies.UserPolicy());
});

builder.Services.AddMvc(opt => opt.Filters.Add<GlobalExceptionFilter>())
    .AddJsonOptions(options 
        => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSignalR().AddAzureSignalR(builder
    .Configuration["ConnectionStrings:AzureSignalRConnectionString"]);

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
            policy.AllowCredentials();
            policy.SetIsOriginAllowed(_ => true);
            //policy.WithOrigins();

        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(myAllowSpecificOrigins);
app.UseRequestResponseLogging();

app.MapHub<GameHub>("/GameHub");

app.Run();

return;


void ConfigureSwaggerGen(SwaggerGenOptions options)
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OrangeEagles.WebApi",
        Version = "v1"
    });
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer"
    });
    options.OperationFilter<AuthenticationRequirementsOperationFilter>();
}

void ConfigureJwtBearer(JwtBearerOptions options)
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Query.ContainsKey("access_token"))
            {
                context.Token = context.Request.Query["access_token"];
            }
            else if (context.Request.Headers.TryGetValue("Authorization", out var value) && value.Count > 0)
            {
                context.Token = value[0]?["Bearer ".Length..];
            }
            return Task.CompletedTask;
        }
        /*
        OnTokenValidated = context =>
        {
            //var memberService = context.HttpContext.RequestServices.GetService<IMemberService>();

            //var result = context?.Principal?.Claims
            //    .FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;

            //if (!string.IsNullOrEmpty(result))
            //{   
            //    context.Fail(new Exception("ÀÓ¼ö¹Î ¸¸¼¼"));
            //}

            //return Task.CompletedTask;
        }
        */
    };

    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? string.Empty)),
        ClockSkew = TimeSpan.Zero
    };
}

AppSettings GetSetting(IConfiguration config)
    => new(config["ConnectionStrings:ServiceBus"] ?? throw new InvalidOperationException(),
        config["ConnectionStrings:TopicName"] ?? throw new InvalidOperationException(),
        config["ConnectionStrings:SignalRConnectionString"] 
        ?? throw new InvalidOperationException());