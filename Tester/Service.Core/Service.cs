using Akka.Actor;
using System;
using System.Threading;

namespace Service.Core
{
    public static class Service
    {
        public static void Process()
        {
            //Console.WriteLine("Starting processing");
            //// Create a new actor system (a container for your actors)
            //var system = ActorSystem.Create("MySystem");

            //// Create your actor and get a reference to it.
            //// This will be an "ActorRef", which is not a
            //// reference to the actual actor instance
            //// but rather a client or proxy to it.
            //var emailProcessor = system.ActorOf<EmailReaderActor>("EmailReaderActor");

            //// Send a message to the actor
            //emailProcessor.Tell(new Email("Hello thread"));

            //system.AwaitTermination();

            //Console.WriteLine("Completed processing");

            var actorSystem = ActorSystem.Create("myActorSystem");

            var yellowActor = actorSystem.ActorOf<YellowActor>();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Starting actor system on thread: {0}", Thread.CurrentThread.ManagedThreadId);

            yellowActor.Tell("Message to yellow");
            actorSystem.AwaitTermination();
        
        }
    }

    public class MessageReceived
    {
        public int Counter { get; private set; }

        public MessageReceived(int counter)
        {
            Counter = counter;
        }
    }

    public class BlueActor : ReceiveActor
    {
        private const string ActorName = "BlueActor";
        private const ConsoleColor MessageColor = ConsoleColor.Blue;

        private int _counter = 0;

        protected override void PreStart()
        {
            base.PreStart();
            Become(HandleString);
        }

        private void HandleString()
        {
            Receive<string>(s =>
            {
                PrintMessage(s);
                _counter++;
                Sender.Tell(new MessageReceived(_counter));
            });
        }

        private void PrintMessage(string message)
        {
            Console.ForegroundColor = MessageColor;
            Console.WriteLine(
                "{0} on thread #{1}: {2}",
                ActorName,
                Thread.CurrentThread.ManagedThreadId,
                message);
        }
    }


    public class GreenActor : ReceiveActor
    {
        private const string ActorName = "GreenActor";
        private const ConsoleColor MessageColor = ConsoleColor.Green;
        private const ConsoleColor ResponseColor = ConsoleColor.DarkGreen;
        private IActorRef _blueActor;

        protected override void PreStart()
        {
            base.PreStart();

            var lastActorProps = Props.Create<BlueActor>();
            _blueActor = Context.ActorOf(lastActorProps);

            Become(HandleString);
        }

        private void HandleString()
        {
            Receive<string>(s =>
            {
                PrintMessage(s);
                _blueActor.Tell(s);
            });

            Receive<MessageReceived>(m => PrintResponse(m));
        }

        private void PrintResponse(MessageReceived message)
        {
            Console.ForegroundColor = ResponseColor;
            Console.Write("{0} on thread #{1}: ",
                ActorName,
                Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Receive message with counter: {0}",
                message.Counter);
        }

        private void PrintMessage(string message)
        {
            Console.ForegroundColor = MessageColor;
            Console.WriteLine(
                "{0} on thread #{1}: {2}",
                ActorName,
                Thread.CurrentThread.ManagedThreadId,
                message);
        }
    }

    public class YellowActor : UntypedActor
    {
        private const string ActorName = "YellowActor";
        private const ConsoleColor MessageColor = ConsoleColor.Yellow;

        private IActorRef _greenActor;

        protected override void PreStart()
        {
            base.PreStart();

            _greenActor = Context.ActorOf<GreenActor>();
        }
        
        protected override void OnReceive(object message)
        {
            if (message is string)
            {
                var msg = message as string;

                PrintMessage(msg);
                _greenActor.Tell(msg);
            }
        }

        private void PrintMessage(string message)
        {
            Console.ForegroundColor = MessageColor;
            Console.WriteLine(
                "{0} on thread #{1}: {2}",
                ActorName,
                Thread.CurrentThread.ManagedThreadId,
                message);
        }
    }

}
