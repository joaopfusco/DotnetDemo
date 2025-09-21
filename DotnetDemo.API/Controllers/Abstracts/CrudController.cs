using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace DotnetDemo.API.Controllers.Abstracts
{
    public class CrudController<TModel>(IBaseService<TModel> service, IValidator<TModel> validator, ILogger logger) : BaseController(logger) where TModel : BaseModel
    {
        protected readonly IBaseService<TModel> _service = service;
        protected readonly IValidator<TModel> _validator = validator;
        protected readonly ILogger _logger = logger;

        [NonAction]
        protected async Task<IActionResult?> ValidateModelAsync(TModel model)
        {
            var result = await _validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }
            return null;
        }

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
        public virtual IActionResult GetById(Guid id)
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
                var validationError = await ValidateModelAsync(model);
                if (validationError != null) return validationError;

                await _service.Insert(model);
                return Ok(model);
            });
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(Guid id, [FromBody] TModel model)
        {
            return await TryExecuteAsync(async () =>
            {
                var validationError = await ValidateModelAsync(model);
                if (validationError != null) return validationError;

                model.Id = id;
                await _service.Update(model);
                return Ok(model);
            });
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            return await TryExecuteAsync(async () =>
            {
                await _service.Delete(id);
                return Ok(id);
            });
        }
    }
}
