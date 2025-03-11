using DotnetDemo.API.Extensions;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DotnetDemo.API.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TModel>(IBaseService<TModel> service, ILogger logger) : Controller where TModel : BaseModel
    {
        protected readonly IBaseService<TModel> _service = service;
        protected readonly ILogger _logger = logger;

        [EnableQuery]
        [HttpGet]
        public virtual IActionResult GetAll()
        {
            return TryExecute(() =>
            {
                return Ok(_service.Get());
            });
        }

        [HttpGet("{id}")]
        public virtual IActionResult GetById(int id)
        {
            return TryExecute(() =>
            {
                return Ok(_service.Get(id).FirstOrDefault());
            });
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

        [NonAction]
        protected virtual IActionResult TryExecute(Func<IActionResult> execute)
        {
            try
            {
                return execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.InnerException);
                return BadRequest(ex);
            }
        }
    }
}
