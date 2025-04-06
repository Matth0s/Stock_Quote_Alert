using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StockQuoteAlert
{
    class EmailSender : ISender
    {
        public ConfigSMTP Configs { get; private set; }
        private SmtpClient _smtpClient;
        private readonly string _from;
        private readonly string _to;
        private readonly string _stockCode;

        public EmailSender(string stockCode)
        {
            Configs = new ConfigSMTP();

            try
            {
                _stockCode = stockCode;
                _smtpClient = new SmtpClient(Configs.Host, Configs.Port);
                _smtpClient.EnableSsl = true;
                _smtpClient.UseDefaultCredentials = false;
                _smtpClient.Credentials = new NetworkCredential(Configs.Username, Configs.Password);
                _from = Configs.Username;
                _to = Configs.Receiver;

                _smtpClient.Send(
                    this.CreateMessage(
                        $"Iniciando Monitoramento de {_stockCode} em [{DateTime.Now}]"
                    )
                );
            }
            catch (Exception ex)
            {
                throw new StockQuoteAlertException(
                    $"Ocorreu um erro com a configuração do SMTP: {ex.Message}"
                );
            }
        }

        private MailMessage CreateMessage(string subject)
        {
            MailMessage mailMessage = new MailMessage(_from, _to);

            mailMessage.Priority = MailPriority.Normal;
            mailMessage.SubjectEncoding = Encoding.GetEncoding("UTF-8");
            mailMessage.Subject = subject;

            return mailMessage;
        }

        public void SendQuoteHigh()
        {
            try
            {
                _smtpClient.Send(this.CreateMessage($"Alta de {_stockCode} em [{DateTime.Now}]"));
            }
            catch (Exception)
            {
                Console.WriteLine($"Erro ao enviar email em: [{DateTime.Now}]");
            }
        }

        public void SendQuoteStable()
        {
            try
            {
                _smtpClient.Send(
                    this.CreateMessage($"Estabilidade de {_stockCode} em [{DateTime.Now}]")
                );
            }
            catch (Exception)
            {
                Console.WriteLine($"Erro ao enviar email em: [{DateTime.Now}]");
            }
        }

        public void SendQuoteLow()
        {
            try
            {
                _smtpClient.Send(this.CreateMessage($"Baixa de {_stockCode} em [{DateTime.Now}]"));
            }
            catch (Exception)
            {
                Console.WriteLine($"Erro ao enviar email em: [{DateTime.Now}]");
            }
        }
    }
}
