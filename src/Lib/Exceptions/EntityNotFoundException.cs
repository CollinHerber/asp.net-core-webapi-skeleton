using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Exceptions {
    public class EntityNotFoundException : NetCoreExampleException {
        public EntityNotFoundException(string message) : base(message) { }
    }
}
