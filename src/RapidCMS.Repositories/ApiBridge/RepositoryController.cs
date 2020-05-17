using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Repositories;
using RapidCMS.Core.Abstractions.Services;
using RapidCMS.Core.Authorization;
using RapidCMS.Core.Forms;
using RapidCMS.Core.Models.Data;
using RapidCMS.Core.Models.Request;
using RapidCMS.Core.Models.Response;
using RapidCMS.Core.Models.State;
using RapidCMS.Repositories.ApiBridge.Models;

namespace RapidCMS.Repositories.ApiBridge
{
    public abstract class RepositoryController<TEntity, TRepository> : ControllerBase
        where TEntity : IEntity
        where TRepository : IRepository
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository _repository;

        protected RepositoryController(
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            TRepository repository)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        private async Task<bool> IsAuthenticatedAsync(OperationAuthorizationRequirement operation, IEntity entity)
        {
            var authorizationChallenge = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor.HttpContext.User,
                entity,
                operation);

            return authorizationChallenge.Succeeded;
        }

        // TODO: validation?
        // TODO: parentId + IQuery + variant

        [HttpPost("entity/{id}")]
        public async Task<ActionResult<TEntity>> HttpGetByIdAsync(string id, [FromBody]QueryModel query)
        {
            var entity = await _repository.GetByIdAsync(id, query.Parent);
            if (!(entity is TEntity typedEntity))
            {
                return NotFound();
            }

            if (!await IsAuthenticatedAsync(Operations.Read, typedEntity))
            {
                return Forbid();
            }

            return typedEntity;
        }

        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<TEntity>>> HttpGetAllAsync([FromBody]QueryModel query)
        {
            var protoEntity = await _repository.NewAsync(query.Parent, query.VariantType);
            if (!await IsAuthenticatedAsync(Operations.Read, protoEntity))
            {
                return Forbid();
            }

            var entities = await _repository.GetAllAsync(query.Parent, query.Query);
            return Ok(entities);
        }

        // get all related

        // get all non related 

        [HttpGet("new")]
        public async Task<ActionResult<TEntity>> HttpNewAsync()
        {
            var entity = await _repository.NewAsync(default, default);
            if (!await IsAuthenticatedAsync(Operations.Create, entity))
            {
                return Forbid();
            }
            return (TEntity)entity;
        }

        [HttpPost("entity")]
        public async Task<ActionResult<TEntity>> HttpInsertAsync([FromBody]TEntity entity)
        {
            if (!await IsAuthenticatedAsync(Operations.Create, entity))
            {
                return Forbid();
            }
            var newEntity = await _repository.InsertAsync(default);
            return (TEntity)newEntity;
        }

        [HttpPost("entity/{id}")]
        public async Task<ActionResult> HttpUpdateAsync(string id, [FromBody]TEntity entity)
        {
            if (!await IsAuthenticatedAsync(Operations.Update, entity))
            {
                return Forbid();
            }
            await _repository.UpdateAsync(default);
            return Ok();
        }

        [HttpDelete("entity/{id}")]
        public async Task<ActionResult> HttpDeleteAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id, default);
            if (entity == null)
            {
                return NotFound();
            }

            if (!await IsAuthenticatedAsync(Operations.Delete, entity))
            {
                return Forbid();
            }

            await _repository.DeleteAsync(id, default);
            return Ok();
        }


        // add

        // remove

        // reorder
    }
}
