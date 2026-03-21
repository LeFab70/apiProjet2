using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;

namespace ApiProjetBorrowing.OpenApi;

internal static class OpenApiOperationExtensions
{
    public static RouteHandlerBuilder WithApiDoc(this RouteHandlerBuilder builder, string summary, string description)
    {
#pragma warning disable ASPDEPR002 // WithOpenApi encore utilisé pour résumés / descriptions jusqu’à adoption des transformateurs d’opération dédiés (.NET 10).
        return builder.WithOpenApi(operation =>
        {
            operation.Summary = summary;
            operation.Description = description;
            return operation;
        });
#pragma warning restore ASPDEPR002
    }
}
