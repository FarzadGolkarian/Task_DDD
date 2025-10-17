using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Task_DDD.WebApi.StartupExtensions;

public static class AuthenticationExtension
{
    public const string CustomAuthorizationHeaderName = "Authorization";
    /// <summary>
    /// JSON Web Encryption
    /// </summary>
    public static AuthenticationBuilder AddJWEBearer(this AuthenticationBuilder authenticationBuilder, WebApplicationBuilder webApplication)
    {
        // https://www.rfc-editor.org/rfc/rfc7516

        authenticationBuilder.AddJwtBearer(options =>
        {
            var secretkey = Encoding.UTF8.GetBytes(webApplication.Configuration["Jwt:Key"]);
            var encryptionkey = Encoding.UTF8.GetBytes(webApplication.Configuration["Jwt:EncryptKey"]);
            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero, // default: 5 min
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ValidateAudience = true, //default : false
                ValidAudience = webApplication.Configuration["Jwt:Audience"],

                ValidateIssuer = true, //default : false
                ValidIssuer = webApplication.Configuration["Jwt:Issuer"],

                TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
            };


            options.SaveToken = true;
            options.TokenValidationParameters = validationParameters;
            options.RequireHttpsMetadata = false;
        });
        return authenticationBuilder;
    }
}