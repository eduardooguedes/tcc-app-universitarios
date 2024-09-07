using Konscious.Security.Cryptography;
using System.Text;

namespace Dashdine.CrossCutting.Senha;

public static class SenhaUsuario
{
    public static string CriptografarSenhaUsuario(string senha)
    {
        return AplicarHashSenha(senha);
    }

    public static bool SenhaDoUsuarioEhValida(string senha, string senhaBanco) => senhaBanco.Equals(AplicarHashSenha(senha));

    private static string AplicarHashSenha(string senha)
    {
        Argon2id argon2 = new(Encoding.UTF8.GetBytes(senha))
        {
            Salt = Encoding.ASCII.GetBytes("fleet$app@.br.mga"),
            DegreeOfParallelism = 8,
            Iterations = 5,
            MemorySize = 512 * 512
        };

        byte[] bytes = argon2.GetBytes(64);

        StringBuilder retorno = new();
        foreach (byte b in bytes)
        {
            retorno.Append(b.ToString("x2"));
        }

        return retorno.ToString();
    }
}
