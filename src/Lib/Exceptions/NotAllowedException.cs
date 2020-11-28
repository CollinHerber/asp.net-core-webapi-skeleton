using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Exceptions {
    public class NotAllowedException : NetCoreExampleException {
        public NotAllowedException(string message) : base(message) { }
    }
}
