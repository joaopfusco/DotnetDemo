using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DotnetDemo.API.Controllers
{
    public class CrudController<TModel>(IBaseService<TModel> service, ILogger logger) : BaseController(logger) where TModel : BaseModel
    {
        protected readonly IBaseService<TModel> _service = service;
        protected readonly ILogger _logger = logger;

        [EnableQuery]
        [HttpGet]
        public virtual IActionResult Get()
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
        public virtual async Task<IActionResult> Post([FromBody] TModel model)
        {
            return await TryExecuteAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.Insert(model);
                return Ok(model);
            });
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(int id, [FromBody] TModel model)
        {
            return await TryExecuteAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                model.Id = id;
                await _service.Update(model);
                return Ok(model);
            });
        }

        [HttpPatch("{id}")]
        public virtual IActionResult Patch(int id, [FromBody] JsonPatchDocument<TModel> patchDoc)
        {
            return TryExecute(() =>
            {
                if (patchDoc == null)
                    return BadRequest();

                var model = _service.Get(id).FirstOrDefault();
                if (model == null)
                    return NotFound();

                patchDoc.ApplyTo(model, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(model);
            });
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            return await TryExecuteAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.Delete(id);
                return Ok(id);
            });
        }
    }
}
