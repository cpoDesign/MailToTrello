using System;
using Akka.Actor;
using Email.Reader;
using SystemContants;

namespace Service.Core
{
    public class EmailMarshallActor : UntypedActor
    {
   
        private const ConsoleColor MessageColor = ConsoleColor.Yellow;
        private IActorRef _emailProcessorActor;
        public class EmailProcessingActor : ReceiveActor
        {
            public EmailProcessingActor()
            {
                Console.ForegroundColor = MessageColor;
                Console.WriteLine("Starting {0}", SystemActors.EmailMarshallActor);
            }
        }

        protected override void PreStart()
        {
            base.PreStart();
            
            Console.WriteLine("PreStart {0}", SystemActors.EmailMarshallActor);
            
            _emailProcessorActor = Context.ActorOf<EmailReaderActor>();
        }

        protected override void OnReceive(object message)
        {
            Console.WriteLine("PreStart {0}", SystemActors.EmailMarshallActor);
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