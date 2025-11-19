
using Microsoft.Extensions.Logging;
using PasswordSaver.Models;
using PasswordSaver.Services;


namespace PasswordSaver.Controllers;

public class ConfigController : IController
{
   

    ConfigService configService = ConfigService.GetInstance();
    public async Task<object?> HandleAsyncTask(Request req)
    {
        // var result = await configService.ChangeDBInConfig(req.Payload.HasValue ? req.Payload.Value.GetString()! : "");
        var result = await configService.ChangeDBInConfig(  );
        return result;
        // return Task.FromResult<object?>(Path.Combine(AppContext.BaseDirectory,".."));
    }
    public async Task<object?> GetPlatform(Request req)
    {
        return  configService.GetPlatform();
    }
    public async Task<object?> MakeBackup(Request req)
    {
        return await configService.CopyToDirDb();
    }
}