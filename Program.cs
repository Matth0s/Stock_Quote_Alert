using System;
using System.Threading.Tasks;

namespace StockQuoteAlert
{
    class Program
    {
        enum Status
        {
            Low,
            Stable,
            High,
        };

        static async Task Main(string[] args)
        {
            Status lastSatus = Status.Stable;
            MonitorArgs monitorArgs = new MonitorArgs(args);
            ConfigSMTP configSMTP = new ConfigSMTP();
            RequestQuote requestQuote = new RequestQuote(monitorArgs.StockCode);
            await requestQuote.ValidadeStockCode();
            EmailSender emailSender = new EmailSender(configSMTP, monitorArgs.StockCode);
            SleepController sleepController = new SleepController();

            Console.WriteLine("/¨¨¨¨¨¨¨¨¨¨ Iniciando Monitoramento ¨¨¨¨¨¨¨¨¨¨\\");
            Console.WriteLine(monitorArgs);
            Console.WriteLine(configSMTP);
            Console.WriteLine("\\_____________________________________________/");

            while (true)
            {
                double quote = await requestQuote.GetCurrentQuote();
                Status currentStatus;

                if (quote > monitorArgs.High)
                {
                    currentStatus = Status.High;
                }
                else if (quote < monitorArgs.Low)
                {
                    currentStatus = Status.Low;
                }
                else
                {
                    currentStatus = Status.Stable;
                }

                if (lastSatus != currentStatus)
                {
                    switch (currentStatus)
                    {
                        case Status.High:
                            emailSender.SendQuoteHigh();
                            break;
                        case Status.Low:
                            emailSender.SendQuoteLow();
                            break;
                        case Status.Stable:
                            emailSender.SendQuoteStable();
                            break;
                    }
                }

                lastSatus = currentStatus;

                sleepController.Sleep();
            }
        }
    }
}
