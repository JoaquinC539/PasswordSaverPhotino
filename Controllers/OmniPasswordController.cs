
using System.Text.Json;
using System.Text.Json.Serialization;
using PasswordSaver.Models;
using Swan;

namespace PasswordSaver.Controllers;

public class OmniPasswordController
{
    private static OmniPasswordController? instance = null;
    
    private readonly Dictionary<string, Func<Request, Task<object?>>> routes;

    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private OmniPasswordController()
    {
        var greet = new GreetController();
        var notFound = new NotFoundController();

        routes = new Dictionary<string, Func<Request, Task<object?>>>
        {
            ["greet"] = req => greet.HandleAsyncTask(req),
            ["notFound"] = req => notFound.HandleAsyncTask(req)
        };

    }


    public static OmniPasswordController GetInstance()
    {
        if (instance == null)
        {
            instance = new OmniPasswordController();
        }
        return instance;
    }
    public async Task<Response> HandleOmniControllerAsync(string jsonString)
    {
        Request? req;
        try
        {
            var doc = JsonDocument.Parse(jsonString);
            var root = doc.RootElement;
            if (!root.TryGetProperty("type", out var t) || !root.TryGetProperty("id", out var idEl))
            {
                throw new IncompleteDataException("Type or Id value not present");
            }

            req = new Request
            {
                Type = t.GetString()!,
                Id = idEl.GetInt32(),
                Payload = root.TryGetProperty("payload", out var payloadEl) ? payloadEl : null
            };
           
        }
        catch (System.Exception e)
        {
            throw new Exception($"An excpetion ocurred during internal json casting handling {e.Message}");
        }
        try
        {
            var handler = routes.ContainsKey(req.Type) ? routes[req.Type] : routes["notFound"];
            var result = await handler(req);
            if (result == null)
            {
                throw new NullDataException($"Handler returned a null for type {req.Type}");
            }
            return new Response
            {
                Id = req.Id,
                Type = req.Type,
                Payload = result,
                Success = true
            };
        }
        catch (System.Exception e)
        {
            
            throw new Exception($"An excepcion ocurred during internal handling of the request {e.Message}");
        }
    }
    
    private Response SendError( int id, string type, string error)
    {
        var resp = new Response {
            Id = id,
            Type = type,
            Success = false,
            Error = error
        };
        return resp;
    }
}