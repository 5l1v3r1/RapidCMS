using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RapidCMS.Core.Abstractions.Data;

namespace RapidCMS.Repositories.ApiBridge
{
    public abstract class RepositoryController<TEntity> : ControllerBase
        where TEntity : IEntity
    {
        // TODO: validation + authorization + authentication?
        // TODO: parentId + IQuery

        [HttpGet("entity/{id}")]
        public async Task<ActionResult<TEntity>> HttpGetByIdAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            return entity;
        }

        protected abstract Task<TEntity> GetByIdAsync(string id);

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TEntity>>> HttpGetAllAsync()
        {
            var entities = await GetAllAsync();
            return Ok(entities);
        }

        protected abstract Task<IEnumerable<TEntity>> GetAllAsync();

        // get all related

        // get all non related 

        [HttpGet("new")]
        public async Task<ActionResult<TEntity>> HttpNewAsync()
        {
            var entity = await NewAsync();
            return entity;
        }

        protected abstract Task<TEntity> NewAsync();

        [HttpPost("entity")]
        public async Task<ActionResult<TEntity>> HttpInsertAsync([FromBody]TEntity entity)
        {
            var newEntity = await InsertAsync(entity);
            return newEntity;
        }

        protected abstract Task<TEntity> InsertAsync(TEntity entity);

        [HttpPost("entity/{id}")]
        public async Task<ActionResult> HttpUpdateAsync(string id, [FromBody]TEntity entity)
        {
            await UpdateAsync(id, entity);
            return Ok();
        }

        protected abstract Task UpdateAsync(string id, TEntity entity);

        [HttpDelete("entity/{id}")]
        public async Task<ActionResult> HttpDeleteAsync(string id)
        {
            await DeleteAsync(id);
            return Ok();
        }

        protected abstract Task DeleteAsync(string id);


        // add

        // remove

        // reorder
    }
}
