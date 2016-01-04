using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace responseTip.Exceptions
{
    [Serializable()]
    public class InvalidTaskStatus : System.Exception
    {
        public InvalidTaskStatus() : base() { }
        public InvalidTaskStatus(string message) : base(message) { }
        public InvalidTaskStatus(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected InvalidTaskStatus(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

    [Serializable()]
    public class NotEnoughArbitersAvailable : System.Exception
    {
        public NotEnoughArbitersAvailable() : base() { }
        public NotEnoughArbitersAvailable(string message) : base(message) { }
        public NotEnoughArbitersAvailable(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected NotEnoughArbitersAvailable(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

    [Serializable()]
    public class EvenNumberOfArbiterVotes : System.Exception
    {
        public EvenNumberOfArbiterVotes() : base() { }
        public EvenNumberOfArbiterVotes(string message) : base(message) { }
        public EvenNumberOfArbiterVotes(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected EvenNumberOfArbiterVotes(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
