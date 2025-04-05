using System;
using System.Dynamic;
using YahooFinanceApi;

namespace StockQuoteAlert
{
    class RequestQuote
    {
        public string StockCode { get; private set; }
        public double LastQuote { get; private set; }

        public RequestQuote(string stockCode)
        {
            StockCode = stockCode.ToUpper();
            if (!StockCode.EndsWith(".SA"))
            {
                StockCode += ".SA";
            }
            LastQuote = 0;
        }

        public async Task ValidadeStockCode()
        {
            try
            {
                IReadOnlyDictionary<string, Security> securities = await Yahoo
                    .Symbols(StockCode)
                    .Fields(Field.RegularMarketPrice)
                    .QueryAsync();
                Security stock = securities[StockCode];
                LastQuote = stock[Field.RegularMarketPrice];
            }
            catch (KeyNotFoundException)
            {
                throw new StockQuoteAlertException(
                    $"A ação {StockCode} não foi encontrada na B3 pelo Yahoo Finance"
                );
            }
            catch (Exception ex)
            {
                throw new StockQuoteAlertException(
                    $"Ocorreu um erro com a validação do codigo da ação: {ex.Message}"
                );
            }
        }

        public async Task<double> GetCurrentQuote()
        {
            try
            {
                IReadOnlyDictionary<string, Security> securities = await Yahoo
                    .Symbols(StockCode)
                    .Fields(Field.RegularMarketPrice)
                    .QueryAsync();
                Security stock = securities[StockCode];
                LastQuote = stock[Field.RegularMarketPrice];
            }
            catch (Exception)
            {
                Console.WriteLine($"Erro ao consultar ação em: [{DateTime.Now}]");
            }
            return LastQuote;
        }
    }
}
