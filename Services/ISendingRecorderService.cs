namespace BackBetha.Services
{
    public interface ISendingRecorderService
    {
        public Task<bool> AddRecordOfDepositRequestByUser(int clientId);
    }
}