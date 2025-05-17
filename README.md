# DotnetDemo

## Rota OData Manualmente  
Em vez de usar `[EnableQuery]` diretamente no controller, este método implementa OData manualmente, permitindo maior controle sobre a consulta. Isso possibilita a personalização da lógica antes de aplicar os filtros e opções da query.  

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
