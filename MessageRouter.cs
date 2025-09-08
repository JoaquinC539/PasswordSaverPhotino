

using System.Text.Json;
using System.Text.Json.Serialization;
using classes;
using controllers;
using Photino.NET;

class MessageRouter
{

    private PhotinoWindow? Window;


    private readonly Dictionary<string, Func<Request, Task<object?>>> routes;
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    public MessageRouter()
    {
        Window = null;
        var greet = new GreetController();
        var notFound = new NotFoundController();
        var masterPassword = new MasterPasswordController();
        routes = new Dictionary<string, Func<Request, Task<object?>>>
        {
            ["greet"] = req => greet.HandleAsyncTask(req),
            ["notFound"] = req => notFound.HandleAsyncTask(req),
            ["count"] = req => masterPassword.HandleAsyncTask(req),
            ["setMaster"] = req => masterPassword.SetMasterPassword(req),
            ["login"] = req => masterPassword.Login(req)
        };
    }
    private void SetWindow(PhotinoWindow window)
    {
        if (Window == null)
        {
            Window = window;
        }
    }

    public async Task HandleRouterAsync(PhotinoWindow window, string message)
    {
        SetWindow(window);
        Request? req;
        try
        {
            var doc = JsonDocument.Parse(message);
            var root = doc.RootElement;
            if (!root.TryGetProperty("type", out var t) || !root.TryGetProperty("id", out var idEl))
            {
                SendError(0, "invalid_request", "type or id missing");
                return;
            }
            req = new Request
            {
                Type = t.GetString()!,
                Id = idEl.GetInt32(),
                Payload = root.TryGetProperty("payload", out var payloadEl) ? (JsonElement?)payloadEl : null
            };
        }
        catch (Exception)
        {

            SendError(0, "invalid_request", "type or id missing");
            return;
        }
        try
        {
            var handler = routes.ContainsKey(req.Type) ? routes[req.Type] : routes["notFound"];
            var result = await handler(req);
            var resp = new ViewResponse
            {
                Id = req.Id,
                Type = req.Type,
                Payload = result,
                Success = true
            };
            DispatchWebSender(resp);
        }
        catch (Exception ex)
        {

            SendError(req.Id, req.Type, ex.Message);
        }
        

    }
    

    private void DispatchWebSender(ViewResponse resp)
    {
        Window!.SendWebMessage(JsonSerializer.Serialize(resp));
    }
    
    private void SendError( int id, string type, string error)
    {
        var resp = new ViewResponse {
            Id = id,
            Type = type,
            Success = false,
            Error = error
        };
        DispatchWebSender( resp);
    }
}