using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SeuTempo.Application.InputModel;
using SeuTempo.Application.Interfaces;
using SeuTempo.Application.ViewModel;
using SeuTempo.Core.Exceptions;
using SeuTempo.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
#pragma warning disable

namespace SeuTempo.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(ILogger<TokenService> logger, IMapper mapper, ITokenRepository tokenRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _tokenRepository = tokenRepository;
        }

        public async Task<TokenViewModel> GetTokenAsync(TokenInputModel tokenInputModel)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET_JWT"));

                await ValidaUsiarioApiAsync(tokenInputModel.ClienteId, tokenInputModel.Senha);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Client", tokenInputModel.ClienteId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwt = tokenHandler.WriteToken(token);

                return new TokenViewModel()
                {
                    Token = jwt,
                    TipoToken = "Bearer",
                    Expira = (int)TimeSpan.FromMinutes(30).TotalMinutes
                };
            }
            catch (DomainException ex)
            {
                throw new DomainException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<bool> ValidaUsiarioApiAsync(Guid login, string senha)
        {
            try
            {
                var result = await _tokenRepository.ListarUsuarioApiAsync(login, senha);

                if (result is null || !result.Status)
                    throw new DomainException("Usuario sem acesso");

                return true;
            }
            catch (DomainException ex)
            {
                throw new DomainException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}