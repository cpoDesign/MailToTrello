
using Akka;
using Akka.Actor;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Email.Entities;
using Email.Logger;
using OpenPop.Pop3;
using SystemContants;
using System.Timers;

namespace Email.Reader
{
    public class EmailReaderActor : UntypedActor
    {
        private const ConsoleColor MessageColor = ConsoleColor.Yellow;
        private IActorRef _emailProcessorActor;
        private IActorRef _systemLogger;

        protected override void PreStart()
        {
            base.PreStart();

            _emailProcessorActor = Context.ActorOf<EmailProcessorActor>();
            _systemLogger = Context.ActorOf<SystemLogger>();
        }

        protected override void OnReceive(object message)
        {
            if (message is string)
            {
                var msg = message as string;

                ProcessEmailInstruction(msg);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void ProcessEmailInstruction(string message)
        {
            Console.ForegroundColor = MessageColor;
            Console.WriteLine(
                "{0} on thread #{1}: {2}",
                SystemActors.EmailReaderActor,
                Thread.CurrentThread.ManagedThreadId,
                message);
            
            // add check for command
            
            ReadAllEmails();
        }

        private void ReadAllEmails()
        {

            //// The client disconnects from the server when being disposed
            //using (Pop3Client client = new Pop3Client())
            //{
            //    // Connect to the server
            //    client.Connect(EmailConfiguration.Pop3, EmailConfiguration.PopPort, true, 1800, 1800, removeCertificateCallback);

            //    // Authenticate ourselves towards the server
            //    client.Authenticate(EmailConfiguration.Email, EmailConfiguration.EmailPwd);

            //    // Get the number of messages in the inbox
            //    int messageCount = client.GetMessageCount();

            //    _systemLogger.Tell(string.Format("Total messages found: {0}", messageCount));

            //    // Most servers give the latest message the highest number
            //    for (int i = messageCount; i > 0; i--)
            //    {
            //        var msg = client.GetMessage(i);

            //        // Now return the fetched messages
            //        _emailProcessorActor.Tell(new EmailMessage(){Subject = msg.Headers.Subject, Date = msg.Headers.Date});
            //    }
            //}

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _systemLogger.Tell($"Total messages found: {1}");
            var msg = new EmailMessage
            {
                Subject = $"Message subject {DateTime.Now.ToString("s")}",
                Date = DateTime.Now.ToString()
            };

            _emailProcessorActor.Tell(msg);
        }

        private bool removeCertificateCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }
    }
}
