using System.Diagnostics;

using Account.Accessablities;
using Account.ChatService;
using Account.MessageService;

namespace Account
{
    [Serializable]
    public class User : IUser
    {
        public int Id { get; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public List<Chat> Chats { get; }
        public Accessibility Type { get; set; }

        public User(int id, string login, string password, string userName)
        {
            Id = id;
            Login = login;
            Password = password;
            Type = Accessibility.Default;
            UserName = userName;
            Chats = new List<Chat>();
        }



        public void CreateChat(int userId, string name)
        {
            Chats.Add(new Chat(userId, name));
        }
        public void DeleteChat(int userId)
        {
            Chat? chat = Chats.Find(c => c.User == userId);
            if (chat != null)
            {
                Chats.Remove(chat);
                Debug.WriteLine($"Chat {userId} was deleted.");
            }
            else
            {
                Debug.WriteLine("The chat was not found.");
            }
        }

        public void SendMessage(int userId, string text, DateTime date)
        {
            int index = Chats.FindIndex(chat => chat.User == userId);
            if (index != -1)
            {
                Chats[index].Message = new Message(Chats[index].ListOfMessages.Count + 1, text, date);
                Debug.WriteLine($"Chat {userId} got new message.");
            }
            else
            {
                Debug.WriteLine("The chat was not found.");
            }
        }
        public void ChangeMessage(int userId, int messageId, string text)
        {
            int index = Chats.FindIndex(chat => chat.User == userId);
            if (index != -1)
            {
                Chats[index].ListOfMessages[messageId].Text = text;
                Debug.WriteLine($"Chat {userId}: {messageId} was changed.");
            }
            else
            {
                Debug.WriteLine("The chat was not found.");
            }
        }
        public void DeleteMessage(int userId, int messageId)
        {
            int index = Chats.FindIndex(chat => chat.User == userId);
            if (index != -1)
            {
                int messageIndex = Chats[index].ListOfMessages.FindIndex(message => message.Id == messageId);
                if (messageIndex != -1)
                {
                    Chats[index].ListOfMessages.RemoveAt(messageIndex);
                    Debug.WriteLine($"Chat {userId}: {messageId} was changed.");
                }
                else
                {
                    Debug.WriteLine("The message was not found.");
                    return;
                }
            }
            else
            {
                Debug.WriteLine("The chat was not found.");
            }
        }
    }
}
