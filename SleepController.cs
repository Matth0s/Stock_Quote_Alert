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

        private double TimeToNextOpen()
        {
            int days;
            DateTime now = DateTime.Now;

            // Verifica se a proxima abertura da B3 será no dia seguinte
            if (
                now.DayOfWeek >= DayOfWeek.Monday
                && now.DayOfWeek <= DayOfWeek.Thursday
                && now.TimeOfDay >= new TimeSpan(17, 00, 0)
                && now.TimeOfDay <= new TimeSpan(23, 59, 59)
            )
            {
                days = 1;
            }
            // Verifica se a proxima abertura da B3 será ainda naquele dia pela manhã
            else if (
                now.DayOfWeek >= DayOfWeek.Tuesday
                && now.DayOfWeek <= DayOfWeek.Friday
                && now.TimeOfDay >= new TimeSpan(0, 0, 0)
                && now.TimeOfDay <= new TimeSpan(9, 59, 59)
            )
            {
                days = 0;
            }
            // Verifica se a abertura da B3 será somente na proxima segunda depois do fim de semana
            else
            {
                days = (7 + DayOfWeek.Monday - now.DayOfWeek) % 7;
            }

            return (DateTime.Today.AddDays(days).AddHours(10) - now).TotalMilliseconds;
        }

        public void Sleep()
        {
            // Se estiver em periodo de negociação (existir milissegundos antes do fechamento), pausa durante 1m
            if (_timeToNextClose > 0)
            {
                _timeToNextClose -= 60000;
                Thread.Sleep(59000); // Compensando o tempo de excecução de 1s do algoritmo na Main
            }
            // Caso contrario, reseta o valor de _timeToNextClose e pausa até a proxima abertura da B3
            else
            {
                _timeToNextClose = 25200000; // Milissegundos entre 10h e 17h30
                Thread.Sleep((int)TimeToNextOpen());
            }
        }
    }
}
