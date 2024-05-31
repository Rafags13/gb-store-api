using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Exceptions
{
    public class UserNotValidException : Exception
    {
        public UserNotValidException(string message) : base(message)
        {
            
        }
        public UserNotValidException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
        public UserNotValidException()
        {
            
        }
    }
}
