namespace Account
{
    internal interface IUser
    {
        void CreateChat(int userId, string name);
        void DeleteChat(int userId);
        void SendMessage(int userId, string text, DateTime date);
        void ChangeMessage(int userId, int messageId, string text);
        void DeleteMessage(int userId, int messageId);
    }
}
