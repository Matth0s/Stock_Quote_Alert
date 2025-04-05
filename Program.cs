using System;
using System.Threading.Tasks;

namespace StockQuoteAlert
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MonitorArgs monitorArgs = new MonitorArgs(args);

            ConfigSMTP configSMTP = new ConfigSMTP();

            RequestQuote requestQuote = new RequestQuote(monitorArgs.StockCode);

            await requestQuote.ValidadeStockCode();

            double quote = await requestQuote.GetCurrentQuote();

            EmailSender emailSender = new EmailSender(configSMTP, "Ação");

            emailSender.SendQuoteLow();
            emailSender.SendQuoteHight();
            emailSender.SendQuoteStable();

            Console.WriteLine(monitorArgs);
            Console.WriteLine(configSMTP);
            Console.WriteLine($"O valor atual da ação em é: {quote}");

        }
    }
}
