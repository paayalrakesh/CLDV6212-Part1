namespace ABC_Retailers.Models{
    public class Transaction
    {
        // Unique identifier for the transaction
        public string TransactionId { get; set; }

        // Name of the item in the transaction
        public string ItemName { get; set; }

        // Quantity of the item in the transaction
        public int Quantity { get; set; }

        // Price per unit of the item
        public decimal Price { get; set; }

        // Total price calculated from Quantity and Price
        public decimal TotalPrice => Quantity * Price;

        // Optional: Date and time when the transaction was created
        public DateTime TransactionDate { get; set; }

        // Optional: A status to indicate if the transaction is pending, completed, etc.
        public string Status { get; set; }
    }
}

