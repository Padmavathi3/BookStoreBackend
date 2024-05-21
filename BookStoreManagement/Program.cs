using BusinessLayer.BInterfaces;
using BusinessLayer.BServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.RInterfaces;
using RepositoryLayer.RServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//dapper
builder.Services.AddSingleton<DapperContext>();
//user
builder.Services.AddScoped<IUser,UserService>();    
builder.Services.AddScoped<IUserBl,UserServiceBl>();
//books
builder.Services.AddScoped<IBook,BookService>();
builder.Services.AddScoped<IBookBl,BookServiceBl>();
//cart
builder.Services.AddScoped<ICart,CartService>();
builder.Services.AddScoped<ICartBl,CartServiceBl>();
//Address
builder.Services.AddScoped<IAddress,AddressService>();
builder.Services.AddScoped<IAddressBl,AddressServiceBl>();
//Order
builder.Services.AddScoped<IOrder,OrderService>();
builder.Services.AddScoped<IOrderBl,OrderServiceBl>();  

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "https://localhost:7231")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowAnyOrigin();
        });
});
//---------------------------------------------------------------------------------------------------------------------------------------


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookStore Project", Version = "v1" });

    // Define the JWT bearer scheme
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

    // Require JWT tokens to be passed on requests
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDistributedMemoryCache();

//jwt

// Add JWT authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,



        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key
    };
});

//-------------------------------------------------------------------------------------------------

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
