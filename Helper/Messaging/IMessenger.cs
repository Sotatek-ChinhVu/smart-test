namespace Helper.Messaging
{
    /// <summary>The interface of the messenger. </summary>
    public interface IMessenger
    {
        void Register<T>(object receiver, Action<T> action);

        void Deregister<T>(object receiver, Action<T> action);

        void DeregisterAll();

        void Send<T>(T message);
    }
}
