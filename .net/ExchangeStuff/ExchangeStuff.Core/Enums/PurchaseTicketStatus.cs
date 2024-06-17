namespace ExchangeStuff.Core.Enums
{
    public enum PurchaseTicketStatus
    {
        Processing, //status after creating a ticket and the buyer's wallet is deducted
        Cancelled,  //status of the buyer canceling the ticket
        Confirmed,  //delivery status and transfer money to the seller's wallet
        Expired    //status canceled due to overdue
    }
}
