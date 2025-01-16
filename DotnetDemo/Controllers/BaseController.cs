using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DotnetDemo.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TModel> : Controller where TModel : BaseModel
    {
        protected TModel _model;
        protected readonly IBaseService<TModel> _service;
        protected readonly ILogger _logger;

        public BaseController(IBaseService<TModel> service, ILogger loger)
        {
            _service = service;
            _logger = loger;
        }

        [HttpGet]
        public virtual IActionResult GetAll()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        public virtual IActionResult GetById(int id)
        {
            return Ok(_service.Get(id).FirstOrDefault());
        }

        [HttpGet("odata")]
        public virtual IActionResult Get(ODataQueryOptions<TModel> queryOptions)
        {
            var query = _service.Get();

            var queryfilter = queryOptions.Filter?.ApplyTo(query, new ODataQuerySettings()) ?? query;

            return Ok(new
            {
                _count = queryOptions.Count?.GetEntityCount(queryfilter),
                value = queryOptions.ApplyTo(query)
            });
        }

        [NonAction]
        protected virtual IActionResult TryExecute(Func<IActionResult> execute)
        {
            try
            {
                return execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody] TModel model)
        {
            return TryExecute(() =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _service.Insert(model);
                return Ok(model);
            });
        }

        [HttpPut("{id}")]
        public virtual IActionResult Put(int id, TModel model)
        {
            return TryExecute(() =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                model.Id = id;
                _service.Update(model);
                return Ok(model);
            });
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(int id)
        {
            return TryExecute(() =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _service.Delete(id);
                return Ok(id);
            });
        }
    }
}
