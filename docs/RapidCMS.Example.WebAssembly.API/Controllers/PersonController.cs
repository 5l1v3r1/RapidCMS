using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidCMS.Core.Abstractions.Repositories;
using RapidCMS.Core.Abstractions.Services;
using RapidCMS.Example.Shared.Data;
using RapidCMS.Repositories;
using RapidCMS.Repositories.ApiBridge;

namespace RapidCMS.Example.WebAssembly.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : RepositoryController<Person, JsonRepository<Person>>
    {
        public PersonController(
            IAuthorizationService authorizationService, 
            IHttpContextAccessor httpContextAccessor, 
            IParentService parentService, 
            JsonRepository<Person> repository) : base(authorizationService, httpContextAccessor, parentService, repository)
        {
        }
    }
}
