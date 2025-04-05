using System;

namespace StockQuoteAlert
{
	class Program
	{
		static void Main(string[] args)
		{
			ConfigSMTP configSMTP = new ConfigSMTP();

			EmailSender emailSender = new EmailSender(configSMTP, "Ação");

			emailSender.SendQuoteLow();
			emailSender.SendQuoteHight();
			emailSender.SendQuoteStable();

			Console.WriteLine(configSMTP);
		}
	}
}
