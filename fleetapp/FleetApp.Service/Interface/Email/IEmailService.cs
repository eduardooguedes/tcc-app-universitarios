namespace Dashdine.Service.Interface.Email;

public interface IEmailService
{
    Task Enviar(string para, string assunto, string corpoDoEmail, List<string>? copias = null, List<string>? copiasOcultas = null);
}
