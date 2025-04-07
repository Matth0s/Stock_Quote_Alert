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
        public string YahooStockCode { get; private set; }
        public double Quote { get; private set; }
        public ISender? Sender { get; set; }

        public MonitorQuoteController(string[] args)
        {
            Args = new MonitorArgs(args);
            YahooStockCode = Args.StockCode;
            if (!YahooStockCode.EndsWith(".SA"))
            {
                YahooStockCode += ".SA";
            }
            Quote = 0;
            Sender = null;
        }

        public async Task ValidadeStockCode()
        {
            try
            {
                IReadOnlyDictionary<string, Security> securities = await Yahoo
                    .Symbols(YahooStockCode)
                    .Fields(Field.RegularMarketPrice)
                    .QueryAsync();
                Security stock = securities[YahooStockCode];
                Quote = stock[Field.RegularMarketPrice];
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
                IReadOnlyDictionary<string, Security> securities = await Yahoo
                    .Symbols(YahooStockCode)
                    .Fields(Field.RegularMarketPrice)
                    .QueryAsync();
                Security stock = securities[YahooStockCode];
                Quote = stock[Field.RegularMarketPrice];
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
