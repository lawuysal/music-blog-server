using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using music_blog_server.Data;
using music_blog_server.Repositories;

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

builder.Services.AddScoped<IGalleryImageRepsitory, LocalGalleryImageRepsitory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images", "Gallery")),
    RequestPath = "/images/gallery"
});

app.MapControllers();

app.Run();
