using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;
using RapidCMS.Core.Abstractions.Repositories;
using RapidCMS.Core.Abstractions.Services;
using RapidCMS.Core.Authorization;
using RapidCMS.Core.Models.Data;
using RapidCMS.Repositories.ApiBridge.Models;

namespace RapidCMS.Repositories.ApiBridge
{
    public abstract class RepositoryController<TEntity, TRepository> : ControllerBase
        where TEntity : IEntity
        where TRepository : IRepository
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IParentService _parentService;
        private readonly IRepository _repository;

        protected RepositoryController(
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            IParentService parentService,
            TRepository repository)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _parentService = parentService;
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

        private Task<IParent?> GetParentAsync(ParentPath? parentPath)
        {
            return _parentService.GetParentAsync(parentPath);
        }

        private async Task<IEditContext<IEntity>> GetEditContextAsync(EditContextModel<TEntity> editContextModel)
        {

        }

        // TODO: validation?
        // TODO: parentId + IQuery + variant

        [HttpPost("entity/{id}")]
        public async Task<ActionResult<TEntity>> HttpGetByIdAsync(string id, [FromBody] QueryModel query)
        {
            var parent = await GetParentAsync(query.Parent);
            var entity = await _repository.GetByIdAsync(id, parent);
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
        public async Task<ActionResult<IEnumerable<TEntity>>> HttpGetAllAsync([FromBody] QueryModel query)
        {
            var parent = await GetParentAsync(query.Parent);
            var protoEntity = await _repository.NewAsync(parent, query.VariantType);
            if (!await IsAuthenticatedAsync(Operations.Read, protoEntity))
            {
                return Forbid();
            }

            var entities = await _repository.GetAllAsync(parent, query.Query);
            return Ok(entities);
        }

        // get all related

        // get all non related 

        [HttpPost("new")]
        public async Task<ActionResult<TEntity>> HttpNewAsync([FromBody] QueryModel query)
        {
            var parent = await GetParentAsync(query.Parent);
            var entity = await _repository.NewAsync(parent, query.VariantType);
            if (!await IsAuthenticatedAsync(Operations.Create, entity))
            {
                return Forbid();
            }
            return (TEntity)entity;
        }

        [HttpPost("entity")]
        public async Task<ActionResult<TEntity>> HttpInsertAsync([FromBody] EditContextModel<TEntity> editContextModel)
        {
            if (!await IsAuthenticatedAsync(Operations.Create, editContextModel.Entity))
            {
                return Forbid();
            }
            var editContext = await GetEditContextAsync(editContextModel);
            var newEntity = await _repository.InsertAsync(editContext);
            return (TEntity)newEntity;
        }

        [HttpPut("entity/{id}")]
        public async Task<ActionResult> HttpUpdateAsync(string id, [FromBody] EditContextModel<TEntity> editContextModel)
        {
            if (!await IsAuthenticatedAsync(Operations.Update, editContextModel.Entity))
            {
                return Forbid();
            }
            var editContext = await GetEditContextAsync(editContextModel);
            await _repository.UpdateAsync(editContext);
            return Ok();
        }

        [HttpDelete("entity/{id}")]
        public async Task<ActionResult> HttpDeleteAsync(string id, [FromBody] QueryModel query)
        {
            var parent = await GetParentAsync(query.Parent);
            var entity = await _repository.GetByIdAsync(id, parent);
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
