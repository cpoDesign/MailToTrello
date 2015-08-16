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
            // This will be an "ActorRef", which is not a reference to the actual actor instance
            // but rather a client or proxy to it.
            var greeter = system.ActorOf<EmailMarshallActor>("EmailMarshallActor");

            // Send a message to the actor to start the process
            greeter.Tell("Start processing");
            greeter.Tell("Stop processing");

            Console.WriteLine("Completed processing");
        }

    
    }
}
