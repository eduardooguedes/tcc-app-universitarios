using Dashdine.CrossCutting.Enums.Usuario;
using Dashdine.Service.Interface.Usuario;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dashdine.Service.Services.Usuario.Autenticacao;

public class TokenService : ITokenService
{
    protected readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(Guid id, string nome, string email, string celular, EnumTipoDeUsuario tipoDeUsuario, int situacao, bool autorizadoRecuperarSenha = false)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_configuration["JWT:key"]!);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString(), ClaimValueTypes.String),
                    new Claim(ClaimTypes.Name, nome, ClaimValueTypes.String),
                    new Claim(ClaimTypes.Email, email, ClaimValueTypes.Email),
                    new Claim(ClaimTypes.MobilePhone, celular, ClaimValueTypes.String),
                    new Claim(ClaimTypes.Role, tipoDeUsuario.ToString(), ClaimValueTypes.String),
                    new Claim("Situacao", situacao.ToString(), ClaimValueTypes.Integer),
                    new Claim("RecuperarSenha", autorizadoRecuperarSenha.ToString(), ClaimValueTypes.Boolean)
                }),
            Expires = autorizadoRecuperarSenha ? DateTime.UtcNow.AddMinutes(10) : DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
