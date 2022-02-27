using backBetha.Models;
using BackBetha.Models;
using BackBetha.Models.SrevicesContracts;
using BackBetha.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackBetha.Controllers
{
    [EnableCors()]
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private int _numOfAccounts = 0;
        private readonly IClientService _clientService;
        private readonly ISendingRecorderService _sendingRecorderService;

        public BankController(IClientService clientService, ISendingRecorderService sendingRecorderService)
        {
            _clientService = clientService;
            _sendingRecorderService = sendingRecorderService;
        }


        [HttpPost("Deposit")]
        public async Task<DepositResponse> Deposit([FromBody] DepositRequest request)
        {

            var response = new DepositResponse();  
            
            if (await _sendingRecorderService.AddRecordOfDepositRequestByUser(request.RequestingClientThread))
            {
                await HandleDeposit(request);
                response.ReturnStatus = ReturnStatuses.Success;
            }
            else
            {
                response.ReturnStatus = ReturnStatuses.ErrorTooManyRequests;
            }
               
            return response;

        }

        private async Task HandleDeposit(DepositRequest request)
        {
            await _clientService.HandleDeposit(request);
            _ = Task.CompletedTask;
        }

        [HttpPost("SetNumberOfClients")]
        public async Task<SetNumberOfClientsResponse> SetNumberOfClients([FromBody] SetNumberOfClientsRequest request)
        {
            //Console.WriteLine("arrived - > " + request.NumberOfClients.ToString());
            _numOfAccounts = request.NumberOfClients;

            await _clientService.InitialClients(_numOfAccounts);
            var response = new SetNumberOfClientsResponse() { ReturnStatus = ReturnStatuses.Success, TotalCount = _numOfAccounts };
            return response;
        }

        [HttpGet]
        public List<Client> GetClients()
        {

            var clients = _clientService.GetClients();
            return clients;
        }

        [HttpPost]
        public async Task<Client> AddClient([FromBody] Client c)
        {
            await _clientService.AddSingleClient(c);
            return c;
        }

        [HttpGet("IsAllive")]
        public async Task<IsAlliveResponse> IsAllive()
        {

            var response = new IsAlliveResponse() { ReturnStatus = ReturnStatuses.Allive };
            return response;
        }
    }
}
