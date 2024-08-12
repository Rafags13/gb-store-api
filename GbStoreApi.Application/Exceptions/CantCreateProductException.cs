namespace GbStoreApi.Application.Exceptions
{
    public class CantCreateProductException : Exception
    {
        public CantCreateProductException(){}

        public CantCreateProductException(string message) : base(message)
        {}

        public CantCreateProductException(string message, Exception inner): base(message, inner){}
    }
}
