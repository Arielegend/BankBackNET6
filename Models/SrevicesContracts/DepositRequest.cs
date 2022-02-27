namespace BackBetha.Models.SrevicesContracts
{
    public class DepositRequest
    {
        public int RequestingClientThread { get; set; }
        public int ClientId { get; set; }
        public int Amount { get; set; }
    }
}
