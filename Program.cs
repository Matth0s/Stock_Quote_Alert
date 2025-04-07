using System;
using System.Threading.Tasks;

namespace StockQuoteAlert
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MonitorQuoteController monitorQuoteController = new MonitorQuoteController(args);
            await monitorQuoteController.ValidadeStockCode();

            EmailSender emailSender = new EmailSender(monitorQuoteController.Args.StockCode);
            monitorQuoteController.Sender = emailSender;

            Console.WriteLine("/¨¨¨¨¨¨¨¨¨¨ Iniciando Monitoramento ¨¨¨¨¨¨¨¨¨¨\\");
            Console.WriteLine(monitorQuoteController.Args);
            Console.WriteLine(emailSender.Configs);
            Console.WriteLine("\\_____________________________________________/");

            await monitorQuoteController.MonitorQuote();
        }
    }
}
