using Projetc.TechChallenge.FIAP.Interfaces;

namespace Projetc.TechChallenge.FIAP.Services
{
    public class ResponseService : IResponseService
    {
        public async Task WriteResponseAsync(HttpContext context, int statusCode, object response)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var responseString = System.Text.Json.JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseString);
        }
    }
    
    
}
