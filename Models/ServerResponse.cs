namespace ReceiveMaker.Models
{
    public class ServerResponse
    {
        public string OrderId { get; set; }
        public string error { get; set; }

        public ServerResponse(string OrderId, string error)
        {
            this.OrderId = OrderId;
            this.error = error;
        }
    }
}