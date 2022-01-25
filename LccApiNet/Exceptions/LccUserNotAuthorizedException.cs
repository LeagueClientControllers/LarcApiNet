using System;

namespace LccApiNet.Exceptions
{
    public class LccUserNotAuthorizedException : Exception
    {
        public LccUserNotAuthorizedException() 
            : base("Attempted to access method that requires authorization without being authorized") {}
    }
}
