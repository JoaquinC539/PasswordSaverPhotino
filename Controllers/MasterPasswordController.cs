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
}