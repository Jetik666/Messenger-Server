namespace Account.Accessablities
{
    [Flags]
    public enum Accessibility : ushort
    {
        Banned = 0,
        Default = 1,
        Administrator = 2,
    }
}
