using System;
using Akka.Actor;
using Email.Entities;
using Email.Logger;

namespace Service.Core
{
    public class EmailMarshallActor : UntypedActor
    {
        private const string ActorName = "EmailMarshallActor";
        private const ConsoleColor MessageColor = ConsoleColor.Yellow;
        private IActorRef _emailProcessorActor;
        public class EmailProcessingActor : ReceiveActor
        {
            public EmailProcessingActor()
            {
                Console.ForegroundColor = MessageColor;
                Console.WriteLine("Starting {0}", ActorName);
                    
                //Receive<EmailMessage>(greet =>
                //    Console.WriteLine("Hello {0}", greet.Subject));
            }
        }

        protected override void PreStart()
        {
            base.PreStart();
            
            Console.WriteLine("PreStart {0}", ActorName);
            
            _emailProcessorActor = Context.ActorOf<EmailProcessorActor>();
        }

        protected override void OnReceive(object message)
        {
            Console.WriteLine("PreStart {0}", ActorName);
            if (message is string)
            {
                ProcessMessage(message as string);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void ProcessMessage(string command)
        {
            switch (command)
            {
                case "Start processing":
                {
                    
                    break;
                }
                default:
                {
                    Console.WriteLine("Default command handling {0}", command);
                    Unhandled(command);   
                    break;
                }
            }
        }
    }
}