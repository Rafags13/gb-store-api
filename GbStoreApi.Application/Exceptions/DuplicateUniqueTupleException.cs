namespace GbStoreApi.Application.Exceptions
{
    internal class DuplicateUniqueTupleException : Exception
    {
        public DuplicateUniqueTupleException()
        {
            
        }

        public DuplicateUniqueTupleException(string message) : base(message)
        {
            
        }

        public DuplicateUniqueTupleException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
