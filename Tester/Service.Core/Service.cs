using System;
using Akka.Actor;

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
            var greeter = system.ActorOf<EmailProcessingActor>("EmailProcessingActor");

            // Send a message to the actor
            greeter.Tell(new Email("Hello thread"));

            Console.WriteLine("Completed processing");
        }
    }

    public class Email
    {
        public Email(string subject)
        {
            Subject = subject;
        }

        public string Subject { get; private set; }
    }

    public class EmailProcessingActor : ReceiveActor
    {
        public EmailProcessingActor()
        {
            Receive<Email>(greet =>
               Console.WriteLine("Hello {0}", greet.Subject));
        }
    }
}
