using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace AosAdjutant.Api.Common;

// Camel-cases query parameter names in the generated OpenAPI document so the spec matches the
// camelCase JSON convention. Runtime model binding is case-insensitive, so this is purely cosmetic.
public sealed class CamelCaseQueryParametersTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        if (operation.Parameters is null)
            return Task.CompletedTask;

        foreach (var parameter in operation.Parameters)
        {
            if (
                parameter is OpenApiParameter mutable
                && mutable.In == ParameterLocation.Query
                && !string.IsNullOrEmpty(mutable.Name)
            )
            {
                mutable.Name = char.ToLowerInvariant(mutable.Name[0]) + mutable.Name[1..];
            }
        }

        return Task.CompletedTask;
    }
}
