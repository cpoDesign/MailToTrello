using System;
using Akka.Actor;
using Email.Entities;
using Email.Logger;

namespace Service.Core
{
    public static class Service
    {
        public static void Process()
        {
            Console.WriteLine("Starting processing");
            // Create a new actor system (a container for your actors)
            var system = ActorSystem.Create("MySystem");

            // Create your actor and get a reference to it.
            // This will be an "ActorRef", which is not a
            // reference to the actual actor instance
            // but rather a client or proxy to it.
            var greeter = system.ActorOf<EmailReaderActor.EmailProcessingActor>("EmailProcessingActor");

            // Send a message to the actor
            greeter.Tell(new EmailMessage() { Subject = "Start processing" });

            Console.WriteLine("Completed processing");
        }

        public class EmailReaderActor : UntypedActor
        {
            private const string ActorName = "EmailReaderActor";
            private const ConsoleColor MessageColor = ConsoleColor.Yellow;
            private IActorRef _greenActor;
            public class EmailProcessingActor : ReceiveActor
            {
                public EmailProcessingActor()
                {
                    Receive<EmailMessage>(greet =>
                        Console.WriteLine("Hello {0}", greet.Subject));
                }
            }

            protected override void OnReceive(object message)
            {
                base.PreStart();

                _greenActor = Context.ActorOf<GreenActor>();
            }
        }
    }
}
