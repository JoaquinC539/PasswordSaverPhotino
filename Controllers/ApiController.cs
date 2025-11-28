using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using PasswordSaver.Models;
namespace PasswordSaver.Controllers;

public class ApiController : WebApiController
{

    private OmniPasswordController omniPasswordController;

    public ApiController()
    {
        omniPasswordController = OmniPasswordController.GetInstance();
    }

    [Route(HttpVerbs.Get, "/status")]
    public object GetStatus()
    {
        try
        {
            object res = new { Message = "ok" };
            return res;
        }
        catch (System.Exception ex)
        {
            throw new Exception("An unhalnded exception occurred at ApiController: " + ex.Message);
        }

    }

    public class PostMessage
    {
        public string? Message { get; set; }
        public bool? Error { get; set; }

    }

    [Route(HttpVerbs.Post, "/post")]
    public object PostSomething([JsonData] PostMessage message)
    {
        Console.WriteLine($"Data from Post {message.Message?.ToString()} - {message.Error?.ToString()}");
        Response.Headers["Mi-Header"] = "HeaderRes";
        return new PostMessage { Message = "Hello from MAUI", Error = false };
    }

    [Route(HttpVerbs.Post,"/passwords")]
    public async Task<Response> HandlePasswordRoute()
    {
        try
        {

            string body = await HttpContext.GetRequestBodyAsStringAsync();
            return await omniPasswordController.HandleOmniControllerAsync(body);
        }
        catch (IncompleteDataException e)
        {
            Response.StatusCode = 400;
            return new Response { Id = 0, Type = "failure", Success = false, Error = "Incomplete data exception: " + e.Message };
        }
        catch (PermissionDeniedException e)
        {
            Response.StatusCode = 403;
            return new Response {Id =0, Type="failure", Success=false, Error=$"PermissionDeniedException: {e}"};
        }
        catch (NullDataException e)
        {
            Response.StatusCode = 503;
            return new Response { Id = 0, Type = "failure", Success = false, Error = "Null data exception: " + e.Message };
        }
        catch( NotImplementedException e)
        {
            Response.StatusCode = 405;
            return new Response {Id = 0, Type = "failure", Success = false, Error = "Not implemented exception: " + e.Message};
        }
        catch(IOException e)
        {
            Response.StatusCode = 403;
            return new Response {Id =0, Type="failure", Success=false, Error=$"IOException: {e}"};
        }
        catch (TaskCanceledException)
        {
            Response.StatusCode = 400;
            return new Response { Id=0,Type="failure",Success=false,Error=$"Operation cancelled"};
        }
        catch (Exception e)
        {
            Response.StatusCode = 500;
            return new Response { Id = 0, Type = "failure", Success = false, Error = e.Message };
        }

    }
}
