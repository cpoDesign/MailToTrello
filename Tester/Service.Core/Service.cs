using Akka.Actor;
using Email.Reader;
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

            var yellowActor = actorSystem.ActorOf<EmailReaderActor>();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Starting actor system on thread: {0}", Thread.CurrentThread.ManagedThreadId);

            yellowActor.Tell("Message to yellow");
            actorSystem.AwaitTermination();

        }
    }
    
    public class BlueActor : ReceiveActor
    {
        private const string ActorName = "BlueActor";
        private const ConsoleColor MessageColor = ConsoleColor.Blue;

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
                //Sender.Tell(new EmailMessage(_counter));
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
}