
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using UniBill.Data;
using UniBill.DTOs;
using UniBill.Helper;
using UniBill.Models;
using UniBill.Services;
using UniBill.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowedAngular", configurePolicy: policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
    // options.AddPolicy(name: "AllowOnlySomeOrigins", configurePolicy: policy =>
    // {
    //     policy.WithOrigins("http://localhost:4200");
    // });
});

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetValue<string>("AppSettings:Audience"),
        ValidIssuer = builder.Configuration.GetValue<string>("AppSettings:Issuer"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("AppSettings:Key")!)),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(CustomResult<string>.Fail("You are not Authenticated.", ["Token is missing or invalid."]));
            return context.Response.WriteAsync(result);
        },

        // Not currently using roles � this won't trigger yet
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(CustomResult<string>.Fail("Access denied.", ["You are not authorised."]));
            return context.Response.WriteAsync(result);
        }
    };
});


builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConString")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<HelperClass>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}"
);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("AllowedAngular");

app.MapControllers();

app.Run();
