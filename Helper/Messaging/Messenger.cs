using Helper.Constants;
using static System.Net.Mime.MediaTypeNames;

namespace Helper.Messaging
{
    public class Messenger : IMessenger
    {
        private readonly List<MessageRegistration> _actions = new List<MessageRegistration>();
        
        private static Messenger? _instance;
        public static Messenger Instance => _instance ?? (_instance = new Messenger());

        private Messenger()
        {
        }

        public void Register<T>(object receiver, Action<T> action)
        {
            _actions.Add(new MessageRegistration(receiver, typeof(T), action));
        }

        public void DeregisterAll()
        {
            _actions.Clear();
        }

        public void Deregister<T>(object receiver, Action<T> action)
        {
            var listAction = _actions.Where(a => a.Receiver == receiver && a.Action as Action<T> == action).ToList();

            foreach (var a in listAction)
                _actions.Remove(a);
        }

        private void PerformAction<T>(object action, T message)
        {
            if (action is Action<T> actionT)
            {
                actionT.Invoke(message);
            }
            else
            {
                ((Delegate)action).DynamicInvoke(message);
            }
        }

        public virtual void Send<T>(T message)
        {
            if (message == null) return;
            
            var type = message.GetType();
            List<MessageRegistration> listAction = _actions.Where(a => a.Type == type || a.Type == null).ToList();

            foreach (var a in listAction)
            {
                PerformAction<T>(a.Action, message);
            }
        }

        public Task<CallbackMessageResult<T>> SendAsync<T>(CallbackMessage<T> message)
        {
            SendMessage(message);

            return message.Task;
        }

        private T SendMessage<T>(T message)
        {
            var type = message!.GetType();
            List<MessageRegistration> listAction = _actions.Where(a => a.Type == type || a.Type == null).ToList();

            foreach (var a in listAction)
            {
                PerformAction<T>(a.Action, message);

            }
            return message;
        }
    }

    internal class MessageRegistration
    {
        public object Receiver { get; private set; }

        public Type Type { get; private set; }
        
        public object Action { get; private set; }


        public MessageRegistration(object receiver, Type type, object action)
        {
            Receiver = receiver;
            Type = type;
            Action = action;
        }
    }
}
