using classes;
using Microsoft.Extensions.Logging;
using services;
using utils;

namespace controllers;

public class ConfigController : IController
{
    ILogger logger = LoggerUtils.Factory.CreateLogger("ConfigController");

    ConfigService configService = ConfigService.GetInstance();
    public Task<object?> HandleAsyncTask(Request req)
    {
        bool result = configService.ChangeDBInConfig(req.Payload.HasValue ? req.Payload.Value.GetString()! : "");
        return Task.FromResult<object?>(result);
        // return Task.FromResult<object?>(Path.Combine(AppContext.BaseDirectory,".."));
    }
}