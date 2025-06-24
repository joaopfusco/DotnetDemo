using DotnetDemo.Keycloak.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

#nullable disable

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

        options.ClientId = builder.Configuration["Keycloak:ClientId"];
        options.ClientSecret = builder.Configuration["Keycloak:ClientSecret"];
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.CallbackPath = "/signin-oidc";

        options.SaveTokens = true;
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true
        };
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true
        };
    });

builder.Services.AddAuthorization();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    })
    .AddOData(opt => opt.Select()
        .Expand()
        .SetMaxTop(null)
        .SkipToken()
        .OrderBy()
        .Count()
        .Filter()
        .EnableQueryFeatures(1000)
    )
    .AddODataNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureServices();

builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
    policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
