using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace PasswordSaver.Controllers;

public class ApiController : WebApiController
{
    public ApiController()
    {
        
    }

    public class PostMessage
    {
        public string? Message {get;set;}
        public bool? Error {get; set;}
    }

    [Route(HttpVerbs.Get, "/status")]
    public object GetStatus()
    {
        try
        {
            object res = new {Message= "ok"};
            return res;
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    [Route(HttpVerbs.Post,"/post")]
    public PostMessage PostSomething([JsonData] PostMessage message)
    {
        Console.WriteLine($"Data from Post {message.Message?.ToString()} - {message.Error?.ToString()}");
        Response.Headers["My-Header"]="HeaderRes";
        return new PostMessage {Message = "Hello rom Server", Error = false};
    }


}