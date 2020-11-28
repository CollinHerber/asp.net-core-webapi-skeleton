using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreExample.Server.Redis.Interfaces {
    public interface IJwtRepository {
        void Add(string jwt, long userId);
        void RemoveByUserId(long userId);
        bool JwtExists(long userId, string jwt);
    }
}
