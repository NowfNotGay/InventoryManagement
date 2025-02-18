using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOcelot();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("example_ocelot.json", optional: false, reloadOnChange: true)
    .Build();

#region Cấu hình RSA Public Key
builder.Services.AddSingleton<RsaSecurityKey>(_ =>
{
    string publicKey = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDQqWk0VEBS8KdpBGkoIOEsCFk+
/CxKvgcZmXJicLn1Z0ZjNgoF3r3XrXwQPxk2/NrzFM9DrF07QBRnVW70BYDPFI2w
ez93M5Q3FsqgO2NkVXWkYDLAPbiM2T2XSyp83uRDb2Vhr6VFTI7uoUyws+jae76L
+DXpKPJkm4Hs9UizkQIDAQAB
-----END PUBLIC KEY-----";
    RSA rsa = RSA.Create();
    rsa.ImportFromPem(publicKey.ToCharArray());
    return new RsaSecurityKey(rsa);
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var rsaKey = builder.Services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = rsaKey
        };
    });



#endregion


#region add cor
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .SetIsOriginAllowed(_ => true);
    });
});
#endregion





var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseAuthentication();

app.UseOcelot().Wait();

app.MapControllers();
app.Run();




//// Xóa cái này e khô máu với mn
///
//"AuthenticationOptions": {
//    "AuthenticationProviderKey": "Bearer",
//        "AllowedScopes": []
//      }
