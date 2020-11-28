using NetCoreExample.Server.Models.Abstracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreExample.Server.Data.Interfaces.Base {
    public interface IBaseRepository<Entity> where Entity : BaseEntity {
        Task<List<Entity>> AllAsync();
        Task<long> AddAsync(Entity entity);
        Task AddMultipleAsync(List<Entity> entities);
        Task UpdateAsync(long id, Entity entity, long? adminId = null);
        Task<bool> DeleteAsync(long id);
        Task<Entity> GetByIdAsync(long id);
        Task<List<Entity>> GetByIdsAsync(long[] ids);
        Task<Entity> SearchAsync(string expression);
        Task<List<Entity>> SearchAllAsync(string expression, string orderByExpression = "");
        Task BulkUpdate(List<Entity> listOfEntitesToUpdateToo);
    }
}
