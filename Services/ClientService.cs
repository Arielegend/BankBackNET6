using BackBetha.Data;
using BackBetha.Models;
using BackBetha.Models.SrevicesContracts;

namespace BackBetha.Services
{
    public class ClientService : IClientService
    {
        private readonly DataContext _dataContext;

        public ClientService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<bool> AddSingleClient(Client c)
        {
            _dataContext.AddAsync(c);
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public List<Client> GetClients()
        {
            return _dataContext.Clients.ToList();
        }

        public async Task InitialClients(int numOfClients)
        {
            if (!CheckIfTableEmpty())
            {
                await EmptyClientsTable();
            }
            
            int[] arrayIds = Enumerable.Range(1, numOfClients).ToArray();
            foreach (int i in arrayIds)
            {
                var helper = new Client()
                {
                    ClientId = i,
                    Amount = 0

                };
                await AddSingleClient(helper);
            }
            
        }
        private async Task EmptyClientsTable()
        {
            var clients = GetClients();
            foreach (var client in clients)
            {
                _ = await RemoveClient(client);
            }
        }

        private bool CheckIfTableEmpty()
        {
            return !_dataContext.Clients.Any();
        }

        private async Task<bool> RemoveClient(Client c)
        {
            _dataContext.Clients.Remove(c);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> HandleDeposit(DepositRequest depositRequest)
        {
            try
            {
                var client = _dataContext.Clients.Find(depositRequest.ClientId);
                var newAmount = client.Amount + depositRequest.Amount;
                client.Amount = newAmount;
                await _dataContext.SaveChangesAsync();
                return newAmount;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
