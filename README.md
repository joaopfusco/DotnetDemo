# DotnetDemo

Rota OData manualmente
```csharp
[HttpGet("odata")]
public virtual IActionResult Get(ODataQueryOptions<TModel> queryOptions)
{
    return TryExecute(() =>
    {
        var query = _service.Get();
        var queryfilter = queryOptions.Filter?.ApplyTo(query, new ODataQuerySettings()) ?? query;

        return Ok(new
        {
            _count = queryOptions.Count?.GetEntityCount(queryfilter),
            value = queryOptions.ApplyTo(query)
        });
    });
}
```
