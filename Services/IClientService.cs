using BackBetha.Models;
using BackBetha.Models.SrevicesContracts;

namespace BackBetha.Services
{
    public interface IClientService
    {
        public Task<bool> AddSingleClient(Client c);
        public List<Client> GetClients();
        public Task InitialClients(int numberOfClients);
        public Task<int> HandleDeposit(DepositRequest depositRequest);
    }
}