using System;

namespace StockQuoteAlert
{
	class Program
	{
		static void Main(string[] args)
		{
			ConfigSMTP configSMTP = new ConfigSMTP();

			Console.WriteLine(configSMTP);
		}
	}
}
