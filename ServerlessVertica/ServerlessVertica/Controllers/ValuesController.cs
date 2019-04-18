using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ServerlessVertica.Repositories;

namespace ServerlessVertica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValuesRepository valuesRepository;

        public ValuesController(IValuesRepository valuesRepository)
        {
            this.valuesRepository = valuesRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(valuesRepository.GetValues());
        }
    }
}
