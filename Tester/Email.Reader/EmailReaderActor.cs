
using Akka.Actor;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Email.Entities;
using Email.Logger;
using OpenPop.Pop3;

namespace Email.Reader
{
    public class EmailReaderActor : UntypedActor
    {
        private const string ActorName = "EmailReaderActor";
        private const ConsoleColor MessageColor = ConsoleColor.Yellow;
        private IActorRef _emailProcessorActor;

        protected override void PreStart()
        {
            base.PreStart();

            _emailProcessorActor = Context.ActorOf<EmailProcessorActor>();
        }

        protected override void OnReceive(object message)
        {
            if (message is string)
            {
                var msg = message as string;

                PrintMessage(msg);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void PrintMessage(string message)
        {
            Console.ForegroundColor = MessageColor;
            Console.WriteLine(
                "{0} on thread #{1}: {2}",
                ActorName,
                Thread.CurrentThread.ManagedThreadId,
                message);
            ReadAllImages();
        }

        private void ReadAllImages()
        {

            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(EmailConfiguration.Pop3, EmailConfiguration.PopPort, true, 1800, 1800, removeCertificateCallback);

                // Authenticate ourselves towards the server
                client.Authenticate(EmailConfiguration.Email, EmailConfiguration.EmailPwd);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    var msg = client.GetMessage(i);
                    
                    // Now return the fetched messages
                    _emailProcessorActor.Tell(new EmailMessage(){Subject = msg.Headers.Subject, Date = msg.Headers.Date});
                }
            }
        }

        private bool removeCertificateCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }
    }
}
