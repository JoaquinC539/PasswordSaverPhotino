using Microsoft.Extensions.DependencyInjection;
using PasswordSaver.Models;
using PasswordSaver.Services;
using PasswordSaver.Utils;

namespace PasswordSaver.Controllers;

public class MasterPasswordController : IController
{
    private MasterPasswordService masterPasswordService;

    public MasterPasswordController()
    {
       masterPasswordService = ServiceLocator.ServiceProvider.GetService<MasterPasswordService>();
        if(masterPasswordService == null)
        {
            throw new NotImplementedException("Master Password Service was not initialized");
        }

    }  
  public async Task<object?> HandleAsyncTask(Request req)
    {
        int count = (int)await masterPasswordService.GetMasterPasswordCountAsync();

        return count;
    }

    public async Task<object?> SetMasterPassword(Request req)
    {

        string password = req.Payload!.Value.GetString()!;
        bool? inserted = await masterPasswordService.InsertMasterPasswordAsync(password);
        return inserted;
    }
    public async Task<object?> Login(Request req)
    {
        string password = req.Payload!.Value.GetString()!;
        bool? same = await masterPasswordService.MakeLoginComparisonAsync(password);
        return same;
    }

    public async Task<object?> Logout(Request req)
    {
        masterPasswordService.MakeLogoutAsync();
        return true;
    }
}