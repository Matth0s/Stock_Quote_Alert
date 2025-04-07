using System;
using System.Reflection;

namespace StockQuoteAlert
{
    class SleepController
    {
        private double _timeToNextClose;

        public SleepController()
        {
            DateTime now = DateTime.Now;
            _timeToNextClose = 0;

            // Se a B3 estiver em horario comercial, calcula o tempo até o proximo fechamento
            if (
                now.DayOfWeek >= DayOfWeek.Monday
                && now.DayOfWeek <= DayOfWeek.Friday
                && now.TimeOfDay >= new TimeSpan(10, 0, 0) // Horario de Abertura B3
                && now.TimeOfDay <= new TimeSpan(17, 00, 00) // Horario de Fechamento B3
            )
            {
                _timeToNextClose = (DateTime.Today.AddHours(17) - now).TotalMilliseconds;
            }
        }

        private int TimeToNextOpen()
        {
            DateTime now = DateTime.Now;
            DateTime nextOpenTime = DateTime.Today.AddHours(10);

            if (now > nextOpenTime) // Significa que a proxima abertura sera no dia seguinte
            {
                nextOpenTime = nextOpenTime.AddDays(1);
            }

            // Se o dia da proxima abertura for um fim de semana, move para a proxima segunda feira
            if (nextOpenTime.DayOfWeek == DayOfWeek.Saturday)
            {
                nextOpenTime = nextOpenTime.AddDays(2);
            }
            else if (nextOpenTime.DayOfWeek == DayOfWeek.Sunday)
            {
                nextOpenTime = nextOpenTime.AddDays(1);
            }

            return (int)(nextOpenTime - now).TotalMilliseconds;
        }

        public void Sleep()
        {
            // Se estiver em periodo de negociação (existir milissegundos antes do fechamento), pausa durante 1m
            if (_timeToNextClose > 0)
            {
                _timeToNextClose -= 60000;
                Thread.Sleep(60000);
            }
            // Caso contrario, reseta o valor de _timeToNextClose e pausa até a proxima abertura da B3
            else
            {
                _timeToNextClose = 25200000; // Milissegundos entre 10h e 17h30
                Thread.Sleep(TimeToNextOpen());
            }
        }
    }
}
