using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StockQuoteAlert
{
	class EmailSender
	{
		private SmtpClient	_smtpClient;
		private string		_from;
		private string		_to;

		public EmailSender(ConfigSMTP configSMTP)
		{
			_smtpClient = new SmtpClient(configSMTP.Host, configSMTP.Port);
			_smtpClient.EnableSsl = true;
			_smtpClient.UseDefaultCredentials = false;
			_smtpClient.Credentials = new NetworkCredential(configSMTP.Username, configSMTP.Password);
			_from = configSMTP.Username;
			_to = configSMTP.Receiver;
		}

		private MailMessage CreateMessage(string subject, string body)
		{
			MailMessage mailMessage = new MailMessage(_from, _to);

			mailMessage.Priority = MailPriority.Normal;
			mailMessage.SubjectEncoding = Encoding.GetEncoding("UTF-8");
			mailMessage.Subject = subject;
			mailMessage.BodyEncoding = Encoding.GetEncoding("UTF-8");
			mailMessage.Body = body;

			return mailMessage;
		}

		public void SendQuoteHight()
		{
			_smtpClient.Send(this.CreateMessage("Alta da Ação", "Ultrapassou o limite estabelecido"));
		}

		public void SendQuoteStable()
		{
			_smtpClient.Send(this.CreateMessage("Estabilidade da Ação", "Voltou ao intervalo estabelecido"));
		}
		public void SendQuoteLow()
		{
			_smtpClient.Send(this.CreateMessage("Baixa da Ação", "Desceu abaixo do limite estabelecido"));
		}
	}
}
