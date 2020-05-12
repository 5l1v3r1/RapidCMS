using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RapidCMS.Example.Shared.Data;
using RapidCMS.Repositories.ApiBridge;

namespace RapidCMS.Example.WebAssembly.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : RepositoryController<Person>
    {
        protected static Dictionary<string, List<Person>> _data = new Dictionary<string, List<Person>>();

        public PersonController()
        {
            if (!_data.ContainsKey("-1"))
            {
                _data["-1"] = new List<Person>();
            }
        }

        protected override Task DeleteAsync(string id)
        {
            _data["-1"].RemoveAll(x => x.Id == int.Parse(id));

            return Task.CompletedTask;
        }

        protected override Task<IEnumerable<Person>> GetAllAsync()
        {
            return Task.FromResult(_data["-1"].AsEnumerable());
        }

        protected override Task<Person> GetByIdAsync(string id)
        {
            return Task.FromResult(_data["-1"].FirstOrDefault(x => x.Id == int.Parse(id)));
        }

        protected override Task<Person> InsertAsync(Person person)
        {
            person.Id = new Random().Next(0, int.MaxValue);
            _data["-1"].Add(person);
            return Task.FromResult(person);
        }

        protected override Task<Person> NewAsync()
        {
            return Task.FromResult(new Person());
        }

        protected override Task UpdateAsync(string id, Person person)
        {
            var index = _data["-1"].FindIndex(x => x.Id == person.Id);

            _data["-1"].Insert(index, person);
            _data["-1"].RemoveAt(index + 1);

            return Task.CompletedTask;
        }
    }
}
