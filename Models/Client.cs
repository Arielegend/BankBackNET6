using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackBetha.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public int Amount { get; set; }
    }
}