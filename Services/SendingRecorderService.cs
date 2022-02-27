using BackBetha.Data;
using BackBetha.Models;

namespace BackBetha.Services
{
    public class SendingRecorderService: ISendingRecorderService
    {
        private readonly DataContext _dataContext;
        private const int ALLOWED_MESSAGES = 5;
        private const int TIME_FRAME = 5;

        public SendingRecorderService(DataContext dataContext)
        {
            _dataContext= dataContext; 
        }

        public async Task<bool> AddRecordOfDepositRequestByUser(int clientId)
        {
            var clientSendingRecords = await GetClientsRecords(clientId);

            //clean array of old records
            var currentRecords = clientSendingRecords.ClientRecords.ToList();
            currentRecords.RemoveAll(s => RemoveIfEndTimeIsExpired(s));

            var allowdRemaining = ALLOWED_MESSAGES - currentRecords.Count;
            if (allowdRemaining > 0)
            {    
                var newRecord = new Record() { AllowedRemaning = allowdRemaining - 1, EndTime = DateTime.Now.AddSeconds(TIME_FRAME).ToString() };
                currentRecords.Add(newRecord);

                clientSendingRecords.ClientRecords = currentRecords.ToArray();

                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
    
        }

        private bool RemoveIfEndTimeIsExpired(Record s)
        {
            var now = DateTime.Now;
            return DateTime.Parse(s.EndTime) < now;
        }

        private async Task<ClientSendingRecord> GetClientsRecords(int clientId)
        {
            var clientSendingRecord =  await _dataContext.ClientSendingRecords.FindAsync(clientId);
            if (clientSendingRecord == null)
            {
                var arrayHelper = Array.Empty<Record>();
                return new ClientSendingRecord() { Id = clientId, ClientRecords = arrayHelper };
            }
            return clientSendingRecord;
        }
    }
}
