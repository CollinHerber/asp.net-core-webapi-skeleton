using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Exceptions {
    public class NetCoreExampleException : Exception {
        public NetCoreExampleException(string message) : base(message) { }
    }
}
