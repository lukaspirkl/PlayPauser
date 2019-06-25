namespace PlayPauser
{
    public class Options
    {
        public int ServerPort { get; set; }
        public string ClientAddress { get; set; }
        public bool IsHttpSender { get; set; }
        public bool IsHttpReceiver { get; set; }
        public bool IsNoSleepEnabled { get; set; }
    }
}
