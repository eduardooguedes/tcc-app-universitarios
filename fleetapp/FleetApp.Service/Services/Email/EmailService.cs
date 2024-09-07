using Dashdine.Service.Interface.Email;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace Dashdine.Service.Services.Email;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly string emailRemetentePadrao = "dashdineapp@gmail.com";
    private readonly string smtpPadrao = "smtp.gmail.com";
    private readonly int portaPadrao = 587;

    public async Task Enviar(string para, string assunto, string corpoDoEmail, List<string>? copias = null, List<string>? copiasOcultas = null)
    {
        if (string.IsNullOrEmpty(para))
            throw new Exception("Favor informar um destinatário.");

        string remetente = configuration["Email:Remetente"] ?? emailRemetentePadrao;

        MailMessage mail = new(remetente, para);

        using SmtpClient client = new();
        client.EnableSsl = true;
        client.Host = configuration["Email:Host"] ?? smtpPadrao;

        client.UseDefaultCredentials = false;
        client.Credentials = new System.Net.NetworkCredential(remetente, configuration["Email:SenhaApp"]);

        client.Port = int.Parse(configuration["Email:Porta"] ?? portaPadrao.ToString());
        client.DeliveryMethod = SmtpDeliveryMethod.Network;

        mail.Subject = assunto;
        mail.Body = corpoDoEmail;
        mail.IsBodyHtml = true;

        if (copias?.Count > 0)
        {
            copias.ForEach(mail.CC.Add);
        }

        if (copiasOcultas?.Count > 0)
        {
            copiasOcultas.ForEach(mail.Bcc.Add);
        }

        client.Send(mail);
    }
}
