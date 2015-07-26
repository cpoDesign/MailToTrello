
namespace Email.Entities
{
    public class EmailMessage
    {
        public int Counter { get; private set; }

        public EmailMessage(int counter)
        {
            Counter = counter;
        }
    }
}
