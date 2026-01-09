using System;

namespace DelimiterSeparatedValueWrapperFramework.Exceptions
{
    public class MalformedLineException : Exception
    {
        public MalformedLineException(string message) : base(message)
        {
        }
    }
}