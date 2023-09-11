using Account.MessageService;

namespace Account.ChatService
{
    [Serializable]
    public class Chat
    {
        public int User { get; }
        public string Name { get; }
        public string CustomizedName { get; set; }
        private readonly List<Message> _messages;

        public Chat(int user, string name)
        {
            User = user;
            Name = name;
            CustomizedName = name;

            _messages = new List<Message>();
        }

        public Message Message
        {
            set { _messages.Add(value); }
        }
        public List<Message> ListOfMessages
        {
            get { return _messages; }
        }
        
    }
}
