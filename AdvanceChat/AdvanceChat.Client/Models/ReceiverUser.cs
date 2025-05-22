namespace AdvanceChat.Client.Models
{
    public class ReceiverUser
    {
        public string ReceiverId { get; set; } = string.Empty;
        public string FulllName { get; set; } = string.Empty;

        public void SetState(string Receiverid,string fullname)
        {
            ReceiverId = Receiverid;
            FulllName = fullname;
        }
    }
}
