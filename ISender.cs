namespace StockQuoteAlert
{
    public interface ISender
    {
        public void SendQuoteHigh();

        public void SendQuoteStable();

        public void SendQuoteLow();
    }
}
