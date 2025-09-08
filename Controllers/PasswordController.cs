using System.Text.Json;
using classes;
using services;

namespace controllers;

public class PasswordController : IController
{
    private PasswordService passwordService;

    public PasswordController()
    {
        passwordService = PasswordService.GetInstance();
    }
    public async Task<object?> HandleAsyncTask(Request req)
    {
        var passwords = await passwordService.GetAllPasswordsAsync();
        return passwords;
    }
    public async Task<object?> InsertPassword(Request req)
    {

        var passwordDto = JsonSerializer.Deserialize<PasswordDto>(req.Payload!.Value.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var inserted = await passwordService.AddPasswordAsync(passwordDto);

        return new GenericJsonObject
        {
            Error = inserted == null,
            Added = inserted,
            ErrorMessage = (inserted == false || inserted == null) ? "Password could not be inserted" : null
        };
    }
    public async Task<object?> GetPassword(Request req)
    {
        int id = req.Payload.Value.GetInt32();
        if (id == null)
        {
            return new GenericJsonObject
            {
                Error = true,
                Added = false,
                ErrorMessage = "There is no id"
            };
        }
        var password = await passwordService.GetSinglePasswordByIdASync(id);
        return password;
    }
    public async Task<object?> UpdatePassword(Request req)
    {
        var passwordDto = JsonSerializer.Deserialize<PasswordDto>(req.Payload!.Value.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (passwordDto!.Id == null)
        {
            return new GenericJsonObject
            {
                Error = true,
                Added = false,
                ErrorMessage = "Body doesn't have id"
            };
        }
        var inserted = await passwordService.EditPasswordAsync(passwordDto);

        return new GenericJsonObject
        {
            Error = inserted == null,
            Added = inserted,
            ErrorMessage = (inserted == false || inserted == null) ? "Password could not be inserted" : null
        };
    }
    public async Task<object?> DeletePassword(Request req)
    {
        int id = req.Payload.Value.GetInt32();
        if (id == null)
        {
            return new GenericJsonObject
            {
                Error = true,
                Added = false,
                ErrorMessage = "There is no id"
            };
        }
        var deleted = await passwordService.DeletePassword(id);
        return new GenericJsonObject
        {
            Error = deleted == null,
            Added = deleted,
            ErrorMessage = (deleted == false || deleted == null) ? "Password could not be inserted" : null
        };
    }
}