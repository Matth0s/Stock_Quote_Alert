using System;
using System.IO;

namespace StockQuoteAlert
{
    class ConfigSMTP
    {
        private const string _filePath = "config.txt";
        public string Receiver { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public ConfigSMTP()
        {
            try
            {
                string[] fileLines = File.ReadAllLines(_filePath);

                Receiver = fileLines[0];
                Host = fileLines[1];
                Port = int.Parse(fileLines[2]);
                Username = fileLines[3];
                Password = fileLines[4];
            }
            catch (FileNotFoundException)
            {
                throw new StockQuoteAlertException($"O arquivo \"{_filePath}\" não foi encontrado");
            }
            catch (IndexOutOfRangeException)
            {
                throw new StockQuoteAlertException(
                    $"Numero de argumentos em \"{_filePath}\" invalido, necessario 5"
                );
            }
            catch (FormatException)
            {
                throw new StockQuoteAlertException($"O formato da porta fornecida é invalido");
            }
            catch (Exception ex)
            {
                throw new StockQuoteAlertException($"Ocorreu um erro: {ex.Message}");
            }
        }

        public override string ToString()
        {
            return $"Receiver: {Receiver}\nHost: {Host}\nPort: {Port}\nUsername: {Username}";
        }
    }
}
