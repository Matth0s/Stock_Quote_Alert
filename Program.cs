using System;
using System.Threading.Tasks;

namespace StockQuoteAlert
{
    class Program
    {
        static async Task Main(string[] args)
        {
            RequestQuote requestQuote = new RequestQuote(args);
            await requestQuote.ValidadeStockCode();

            EmailSender emailSender = new EmailSender(requestQuote.Args.StockCode);
            requestQuote.Sender = emailSender;

            Console.WriteLine("/¨¨¨¨¨¨¨¨¨¨ Iniciando Monitoramento ¨¨¨¨¨¨¨¨¨¨\\");
            Console.WriteLine(requestQuote.Args);
            Console.WriteLine(emailSender.Configs);
            Console.WriteLine("\\_____________________________________________/");

            await requestQuote.MonitorQuote();
        }
    }
}
