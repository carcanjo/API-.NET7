using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SeuTempo.API.Utils;
using SeuTempo.Application.Interfaces;
using SeuTempo.Application.Services;
using SeuTempo.Core.DTOs;
using SeuTempo.Core.Entities.UsuarioAPI;
using SeuTempo.Core.Interfaces;
using SeuTempo.Infrastructure.Context;
using SeuTempo.Infrastructure.Repositories;
using System.Text;

namespace SeuTempo.API.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITokenRepository, TokenRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<SeuTempoDbContext>();
            services.AddSingleton(AddAutoMapper().CreateMapper());
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            var secretKey = Environment.GetEnvironmentVariable("SECRET_JWT");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
                x.Events = new JwtBearerEvents
                {
                    OnForbidden = async (context) =>
                    {
                        context.HttpContext.Response.StatusCode = 403;
                        await context.HttpContext.Response.WriteAsJsonAsync(Responses.ForbiddenErrorMessage());
                    },
                    OnAuthenticationFailed = async (context) =>
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        await context.HttpContext.Response.WriteAsJsonAsync(Responses.UnauthorizeErrorMessage());
                    }
                };
            });
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Seu Tempo",
                    Description = "API Responsável por toda regra de negócio da aplicação SEU TEMPO"
                });
                x.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization Header - utilizando Bearer Authentication \r\n\r\n" +
                    "Digite 'Bearer' [espaço] e então seu token no campo abaixo. \r\n\r\n" +
                    "Exemplo (informar sem as aspas): 'Bearer dsjgh~dlyrwotéorhkgfkhrptohrt==' \r\n\r\n" +
                    "Autenticação deve ser feita com login e senha",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });
                //x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory , $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        private static MapperConfiguration AddAutoMapper()
        {
            return new MapperConfiguration(x =>
            {
                x.CreateMap<UsuarioApi, UsuarioApiDTO>().ReverseMap();
            });
        }
    }
}

