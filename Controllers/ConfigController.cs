
using Microsoft.Extensions.Logging;
using PasswordSaver.Models;
using PasswordSaver.Services;


namespace PasswordSaver.Controllers;

public class ConfigController : IController
{
   

    ConfigService configService = ConfigService.GetInstance();
    public Task<object?> HandleAsyncTask(Request req)
    {
        bool result = configService.ChangeDBInConfig(req.Payload.HasValue ? req.Payload.Value.GetString()! : "");
        return Task.FromResult<object?>(result);
        // return Task.FromResult<object?>(Path.Combine(AppContext.BaseDirectory,".."));
    }
}