namespace services;

public class PasswordService
{
    private static PasswordService instance = null;


    private PasswordService() { }

    public static PasswordService GetInstance()
    {
        if (instance == null)
        {
            instance = new PasswordService();
        }
        return instance;
    }
}