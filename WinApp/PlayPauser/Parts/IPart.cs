namespace PlayPauser.Parts
{
    public interface IPart
    {
        void Start(Options options);
        void Stop();
    }
}
