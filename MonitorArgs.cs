using System;

namespace StockQuoteAlert
{
    class MonitorArgs
    {
        public string StockCode { get; private set; }
        public double High { get; private set; }
        public double Low { get; private set; }

        public MonitorArgs(string[] args)
        {
            try
            {
                StockCode = args[0];
                double value1 = double.Parse(args[1].Replace('.', ','));
                double value2 = double.Parse(args[2].Replace('.', ','));

                if (value1 > value2)
                {
                    High = value1;
                    Low = value2;
                }
                else
                {
                    High = value2;
                    Low = value1;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new StockQuoteAlertException(
                    $"Numero de argumentos de monitoramento invalido, necessario 3"
                );
            }
            catch (FormatException)
            {
                throw new StockQuoteAlertException(
                    $"O formato dos valores limite fornecidos para monitoramento é invalido"
                );
            }
            catch (Exception ex)
            {
                throw new StockQuoteAlertException(
                    $"Ocorreu um com o processamento dos parametros passados para o programa: {ex.Message}"
                );
            }
        }

        public override string ToString()
        {
            return $"Parametros de Monitoramento:\n  - StockCode: {StockCode}\n  - High: {High:F2}\n  - Low: {Low:F2}";
        }
    }
}
