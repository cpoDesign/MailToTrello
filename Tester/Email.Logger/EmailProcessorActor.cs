﻿using Akka.Actor;
using Email.Entities;
using System;
using System.Threading;
using SystemContants;

namespace Email.Logger
{
    public class EmailProcessorActor : ReceiveActor
    {
        private const ConsoleColor MessageColor = ConsoleColor.Green;
        private const ConsoleColor ResponseColor = ConsoleColor.DarkGreen;

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
            Console.Write("{0} on thread #{1}: ",
                SystemActors.EmailProcessorActor,
                Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Receive message with counter: {0}",
                message.Subject);
        }
    }
}
