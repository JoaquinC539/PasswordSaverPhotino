namespace PasswordSaver.Utils;
public static class Copier
{
    public static void Copy(string sourceFilePath,string destFilePath)
    {
        try
        {
            File.Copy(sourceFilePath,destFilePath);
        }
        catch (System.Exception ex) 
        {
            
            throw new Exception($"An error ocurred at files copier: {ex.Message}");
        }
    }
    public static void WriteOver(string filePath,string content)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath,content);
                Console.WriteLine($"File rewritten: {filePath}");
            }
            else
            {
                throw new IOException("Configuration file not found");
            }
        }
        catch (System.Exception ex)
        {
            
            throw new Exception($"Error ocurred while writing over: {ex.Message}");
        }
        
    }
}