using NetCoreExample.Server.Redis.Interfaces;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreExample.Server.Redis.Repositories {
    public class JwtRepository : IJwtRepository, IDisposable {
        private readonly IRedisClient _client;

        public JwtRepository(IRedisClientsManager clientsManager) {
#if !DEBUG
            _client = clientsManager.GetClient();
#endif
        }

        public void Add(string jwt, long userId) {
            //Remove all previous JWT since user just logged in and this should be only valid JWT
            //This will only allow a user to be signed in at one place at a time
            //Remove this to allow multiple tokens that will only be cleared on logout or expiration
            //RemoveByUserId(userId);
            _client.AddItemToSet(userId.ToString(), jwt);
        }

        public void RemoveByUserId(long userId) {
            var nativeClient = (IRedisNativeClient)_client;
#if !DEBUG
            nativeClient.Del(userId.ToString());
#endif
        }

        public bool JwtExists(long userId, string jwt) {
            var tokens = _client.GetAllItemsFromSet(userId.ToString());
            return tokens == null || tokens.Contains(jwt);
        }

        public void Dispose() {
#if !DEBUG
            _client.Dispose();
#endif
        }
    }
}
