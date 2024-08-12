namespace GbStoreApi.Application.Exceptions
{
    internal class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(){}
        public InvalidPasswordException(string message) : base(message){}
        public InvalidPasswordException(string message, Exception innerException) : base(message, innerException) {}
    }
}
