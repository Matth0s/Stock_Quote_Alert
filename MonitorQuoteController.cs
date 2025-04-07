using System;
using System.Dynamic;
using YahooFinanceApi;

namespace StockQuoteAlert
{
    enum Status
    {
        Low,
        Stable,
        High,
    };

    class MonitorQuoteController
    {
        public MonitorArgs Args { get; private set; }
        public Yahoo YahooStock { get; private set; }
        public double Quote { get; private set; }
        public ISender? Sender { get; set; }

        public MonitorQuoteController(string[] args)
        {
            Args = new MonitorArgs(args);
            if (Args.StockCode.EndsWith(".SA"))
            {
                YahooStock = Yahoo.Symbols(Args.StockCode).Fields(Field.RegularMarketPrice);
            }
            else
            {
                YahooStock = Yahoo.Symbols(Args.StockCode + ".SA").Fields(Field.RegularMarketPrice);
            }
            Quote = 0;
            Sender = null;
        }

        public async Task ValidadeStockCode()
        {
            try
            {
                IReadOnlyDictionary<string, Security> securities = await YahooStock.QueryAsync();
                Quote = securities.Values.First()[Field.RegularMarketPrice];
            }
            catch (KeyNotFoundException)
            {
                throw new StockQuoteAlertException(
                    $"A ação {Args.StockCode} não foi encontrada na B3 pelo Yahoo Finance"
                );
            }
            catch (Exception ex)
            {
                throw new StockQuoteAlertException(
                    $"Ocorreu um erro com a validação do codigo da ação: {ex.Message}"
                );
            }
        }

        public async Task<Status> GetCurrentStatusQuote()
        {
            try
            {
                IReadOnlyDictionary<string, Security> securities = await YahooStock.QueryAsync();
                Quote = securities.Values.First()[Field.RegularMarketPrice];
            }
            catch (Exception)
            {
                Console.WriteLine($"Erro ao consultar ação em: [{DateTime.Now}]");
            }

            if (Quote > Args.High)
            {
                return Status.High;
            }
            else if (Quote < Args.Low)
            {
                return Status.Low;
            }
            else
            {
                return Status.Stable;
            }
        }

        public async Task MonitorQuote()
        {
            Status lastSatus = Status.Stable;
            SleepController sleepController = new SleepController();

            while (true)
            {
                Status currentStatus = await GetCurrentStatusQuote();

                if (lastSatus != currentStatus && Sender != null)
                {
                    switch (currentStatus)
                    {
                        case Status.High:
                            Sender.SendQuoteHigh();
                            break;
                        case Status.Low:
                            Sender.SendQuoteLow();
                            break;
                        case Status.Stable:
                            Sender.SendQuoteStable();
                            break;
                    }
                }

                lastSatus = currentStatus;
                sleepController.Sleep();
            }
        }
    }
}
