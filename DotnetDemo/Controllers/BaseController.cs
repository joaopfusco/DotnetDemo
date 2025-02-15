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
    public class BaseController<TModel>(IBaseService<TModel> service, ILogger logger) : Controller where TModel : BaseModel
    {
        [HttpGet]
        public virtual IActionResult GetAll()
        {
            return TryExecute(() =>
            {
                return Ok(service.Get());
            });
        }

        [HttpGet("{id}")]
        public virtual IActionResult GetById(int id)
        {
            return TryExecute(() =>
            {
                return Ok(service.Get(id).FirstOrDefault());
            });
        }

        [HttpGet("odata")]
        public virtual IActionResult Get(ODataQueryOptions<TModel> queryOptions)
        {
            return TryExecute(() =>
            {
                var query = service.Get();
                var queryfilter = queryOptions.Filter?.ApplyTo(query, new ODataQuerySettings()) ?? query;

                return Ok(new
                {
                    _count = queryOptions.Count?.GetEntityCount(queryfilter),
                    value = queryOptions.ApplyTo(query)
                });
            });
        }


        [HttpPost]
        public virtual IActionResult Post([FromBody] TModel model)
        {
            return TryExecute(() =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                service.Insert(model);
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
                service.Update(model);
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

                service.Delete(id);
                return Ok(id);
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
                logger.LogError(ex, ex.Message, ex.InnerException);
                return BadRequest(ex);
            }
        }
    }
}
