using System;

namespace NoteTakingApp.Core.Exceptions
{
    public class DomainException: Exception
    {
        public int Code { get; set; } = 0;
    }
}
