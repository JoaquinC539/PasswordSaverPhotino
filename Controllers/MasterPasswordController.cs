namespace controllers;
using classes;
using services;

public class MasterPasswordController : IController
{
    private MasterPasswordService masterPasswordService;
    public MasterPasswordController()
    {
        masterPasswordService = MasterPasswordService.GetInstance();
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
}