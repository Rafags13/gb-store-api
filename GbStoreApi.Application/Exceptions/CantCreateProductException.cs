using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
