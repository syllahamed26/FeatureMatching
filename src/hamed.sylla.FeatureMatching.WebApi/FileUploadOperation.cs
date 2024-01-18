using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class FileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileUploadMime = "multipart/form-data";
        if (operation.RequestBody?.Content.Any(x => x.Key.Equals(fileUploadMime)) == true)
        {
            operation.RequestBody.Content[fileUploadMime].Schema.Properties.Clear();
            operation.RequestBody.Content[fileUploadMime].Schema.Properties.Add("files", new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema { Type = "string", Format = "binary" }
            });
        }
    }
}