using System;
using Akka.Actor;
using Email.Logger;
using Email.Reader;

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
            }
        }

        protected override void PreStart()
        {
            base.PreStart();
            
            Console.WriteLine("PreStart {0}", ActorName);
            
            _emailProcessorActor = Context.ActorOf<EmailReaderActor>();
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
                    Console.WriteLine("Recieved command for start processing {0}", command);
                    _emailProcessorActor.Tell("Read emails");
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