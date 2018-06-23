using System;

namespace NoteTakingApp.Core.Exceptions
{
    public class DomainException: Exception
    {        
        public DomainException(string message = null, Exception innerException = default(Exception))
            :base(message, innerException) { }        
    }
}
