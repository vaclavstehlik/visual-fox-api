using System;
using System.Runtime.Serialization;

namespace Web.Exceptions
{
    [Serializable]
    public class RecommenderExternalException : Exception
    {
        public RecommenderExternalException()
        {
        }

        public RecommenderExternalException(string message) : base(message)
        {
        }

        public RecommenderExternalException(string message, Exception inner) : base(message, inner)
        {
        }

        public RecommenderExternalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
