using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using music_blog_server.Data;
using music_blog_server.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddDbContext<MusicBlogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MusicBlogConnectionString")));
builder.Services.AddDbContext<MusicBlogAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MusicBlogConnectionString")));

builder.Services.AddScoped<IGalleryImageRepository, LocalGalleryImageRepsitory>();
builder.Services.AddScoped<IArticleImageRepository, LocalArticleImageRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("MusicBlog")
    .AddEntityFrameworkStores<MusicBlogAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    }) ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images", "Gallery")),
    RequestPath = "/images/gallery"
}
);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images", "Article")),
    RequestPath = "/images/article"
}
);

app.MapControllers();

app.Run();
