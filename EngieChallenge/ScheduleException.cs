using System.Runtime.Serialization;

namespace Engie
{
    [Serializable]
    internal class ScheduleException : Exception
    {
        public ScheduleException()
        {
        }

        public ScheduleException(string? message) : base(message)
        {
        }

        public ScheduleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ScheduleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}