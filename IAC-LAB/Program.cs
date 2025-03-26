using IAC_LAB.Domain.Configurations;
using IAC_LAB.Services.AwsServices;
using IAC_LAB.Services.OpenstackServices;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "wwwroot"
});

builder.Services.AddControllers();
builder.Services.Configure<OpenstackAuthOptions>(
    builder.Configuration.GetSection("Openstack:Auth"));


builder.Services.AddScoped<IAwsServiceTerraform, AwsServiceTerraform>();
builder.Services.AddScoped<IOpenstackServiceTerraform, OpenstackServiceTerraform>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapGet("/", () => "Hello World!");
app.MapControllers();


app.Run();
