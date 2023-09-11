namespace Account.MessageService
{
    [Serializable]
    public class Message
    {
        public int Id { get; }
        public DateTime Date { get; }
        public string Text { get; set; }

        public Message(int id, string text, DateTime date) 
        {
            Id = id;
            Text = text;
            Date = date;
        }
    }
}
