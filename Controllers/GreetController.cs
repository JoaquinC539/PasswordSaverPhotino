using System.Text.Json;
using classes;

namespace controllers;


class GreetController
{
    public GreetController() { }

    public string HandleGreet(Request req)
    {
        //  Here the request could be deserialized into necesary
        Console.WriteLine( req.Payload.GetType());

        GreetMessage gmsg = new GreetMessage() { Message="Hello from .NET", Test= new TestNes(){TestText="This is a nested text"}};

        ViewResponse resp = new ViewResponse(req.Id, req.Type, gmsg);

        return JsonSerializer.Serialize(resp);
    }
}
