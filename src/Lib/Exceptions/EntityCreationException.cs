using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Exceptions {
    public class EntityCreationException : NetCoreExampleException {
        public EntityCreationException(string message) : base(message) { }
    }
}
