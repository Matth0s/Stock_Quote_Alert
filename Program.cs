using System;

namespace StockQuoteAlert
{
	class Program
	{
		static void Main(string[] args)
		{
			ConfigSMTP configSMTP = new ConfigSMTP();

			Console.WriteLine(configSMTP);

			EmailSender emailSender = new EmailSender(configSMTP);

			emailSender.SendQuoteLow();
			emailSender.SendQuoteStable();
			emailSender.SendQuoteHight();
		}
	}
}
