using System;
using Akka.Actor;

namespace Email.Logger
{
    public class SystemLogger : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is string)
            {
                Console.WriteLine(message);
            }
            else
            {
                Unhandled(message);
            }
        }
    }
}