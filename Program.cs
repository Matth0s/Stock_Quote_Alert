using System;

namespace StockQuoteAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            MonitorArgs monitorArgs = new MonitorArgs(args);

            ConfigSMTP configSMTP = new ConfigSMTP();

            // EmailSender emailSender = new EmailSender(configSMTP, "Ação");

            // emailSender.SendQuoteLow();
            // emailSender.SendQuoteHight();
            // emailSender.SendQuoteStable();

            Console.WriteLine(monitorArgs);
            Console.WriteLine(configSMTP);
        }
    }
}
