using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Exceptions {
    public class InvalidEntityException : NetCoreExampleException {
        public InvalidEntityException(string message) : base(message) { }
    }
}
