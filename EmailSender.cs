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
		private string		_acao;

		public EmailSender(ConfigSMTP configSMTP, string acao)
		{
			try
			{
				_acao = acao;
				_smtpClient = new SmtpClient(configSMTP.Host, configSMTP.Port);
				_smtpClient.EnableSsl = true;
				_smtpClient.UseDefaultCredentials = false;
				_smtpClient.Credentials = new NetworkCredential(configSMTP.Username, configSMTP.Password);
				_from = configSMTP.Username;
				_to = configSMTP.Receiver;

				_smtpClient.Send(this.CreateMessage($"Iniciando Monitoramento de {_acao}"));
			}
			catch (Exception ex)
			{
				throw new StockQuoteAlertException($"Ocorreu um erro com a configuração do SMTP: {ex.Message}");
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

		public void SendQuoteHight()
		{
			_smtpClient.Send(this.CreateMessage($"Alta de {_acao}"));
		}

		public void SendQuoteStable()
		{
			_smtpClient.Send(this.CreateMessage($"Estabilidade de {_acao}"));
		}
		public void SendQuoteLow()
		{
			_smtpClient.Send(this.CreateMessage($"Baixa de {_acao}"));
		}
	}
}
