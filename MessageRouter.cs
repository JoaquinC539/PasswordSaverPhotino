

using System.Text.Json;
using classes;
using controllers;
using Photino.NET;

class MessageRouter
{

    private GreetController greetController;
    public MessageRouter()
    {
        greetController = new GreetController();
    }

    public void HandleRouter(PhotinoWindow window, string message)
    {
        
        var mapMsg = JsonSerializer.Deserialize<Dictionary<string, object>>(message);
        string type = mapMsg!.ContainsKey("type") ? mapMsg["type"].ToString()! : "";
       
        if (type == "" || !mapMsg.ContainsKey("id"))
        {
            window.SendWebMessage("Type or id not found in request");
            return;
        }
        int id = int.Parse(mapMsg["id"].ToString()!);
        Request req = new Request() { Type = type, Id = id, Payload = mapMsg!.ContainsKey("payload") ? mapMsg["payload"].ToString() : null };
        if (type == "greet")
        {
            string msgres = greetController.HandleGreet(req);            
            window.SendWebMessage(msgres);
        }
    }
    
}