using Imetame.Documentacao.WebApi;
using OpenIddict.Validation.AspNetCore;
using Imetame.Documentacao.WebApi.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCustomMvc()
            .Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });


//builder.Services.AddControllers();
//builder.Services.AddMvcCore()
//                .AddViews()
//                .AddRazorViewEngine();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        var identityOptions = new IdentityOptions();
        builder.Configuration.GetSection("Identity").Bind(identityOptions);

        // Note: the validation handler uses OpenID Connect discovery
        // to retrieve the address of the introspection endpoint.
        options.SetIssuer(identityOptions.Authority);
        options.AddAudiences(identityOptions.Audience);

        // Configure the validation handler to use introspection and register the client
        // credentials used when communicating with the remote introspection endpoint.
        options.UseIntrospection()
               .SetClientId(identityOptions.ClientId)
               .SetClientSecret(identityOptions.ClientSecret);


        // Register the System.Net.Http integration.
        options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

builder.Services.AddCustomAuthorization();


builder.Services.AddCors(o => o.AddPolicy("AnyOriginPolicy", builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
}));

builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.RegisterCustomServices(builder.Configuration);
builder.Services.AddCustomAutoMapper();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AnyOriginPolicy");
app.UseHttpsRedirection();

app.UseCustomStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();





app.Run();
