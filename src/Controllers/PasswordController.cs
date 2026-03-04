namespace PasswordSaver.Controllers;

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PasswordSaver.Models;
using PasswordSaver.Services;
using PasswordSaver.Utils;

public class PasswordController : IController
{
    private PasswordService passwordService;

    public PasswordController()
    {
        passwordService = ServiceLocator.ServiceProvider.GetService<PasswordService>();
        // passwordService = PasswordService.GetInstance();
    }

    public async Task<object?> HandleAsyncTask(Request req)
    {
        var passwords = await passwordService.GetAllPasswordsAsync();
        return passwords;
    }

    public async Task<object?> InsertPassword(Request req)
    {
        var passwordDto = JsonSerializer.Deserialize<PasswordDto>(req.Payload.Value.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var inserted = await passwordService.AddPasswordAsync(passwordDto);
        return inserted;
    }

    public async Task<object?> GetPassword(Request req)
    {
        int id = req.Payload != null ? req.Payload.Value.GetInt32():0;
        if (id == 0)
        {
            return null;
        }
        
        var password = await passwordService.GetSinglePasswordByIdASync(id);
        return password;
    }

    public async Task<object?> UpdatePassword(Request req)
    {
        var passwordDto = JsonSerializer.Deserialize<PasswordDto>(req.Payload!.Value.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (passwordDto!.Id == null)
        {
            return null;
        }
        var inserted = await passwordService.EditPasswordAsync(passwordDto);

        return inserted;
    }

    public async Task<object?> DeletePassword(Request req)
    {
        int id = req.Payload != null ? req.Payload.Value.GetInt32():0;
        if (id == 0)
        {
            return null;
        }
        var deleted = await passwordService.DeletePassword(id);
        return deleted;
    }
}