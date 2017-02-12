using Akka.Actor;
using Email.Entities;
using System;
using SystemContants;

namespace Trello.Recorder
{
    public class EmailProcessorActor : ReceiveActor
    {
        private const ConsoleColor MessageColor = ConsoleColor.Cyan;
        private const ConsoleColor ResponseColor = ConsoleColor.DarkCyan;

        protected override void PreStart()
        {
            base.PreStart();
            Become(HandleString);
        }

        private void HandleString()
        {

            Receive<EmailMessage>(s => PrintResponse(s));
        }

        private void PrintResponse(EmailMessage message)
        {
            Console.ForegroundColor = ResponseColor;
            Console.Write("{0} on thread #: ", SystemActors.TrelloActor);
            Console.WriteLine("Receive message with counter: {0}", message.Subject);
        }
    }
}
