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
}
