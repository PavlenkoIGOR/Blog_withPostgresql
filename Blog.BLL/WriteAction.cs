using Microsoft.AspNetCore.Hosting;

namespace Blog.Data;

public static class WriteAction
{
    /// <summary>
    /// Метод для создания папки в которой будут файлы с логами
    /// </summary>
    /// <param name="_env">IWebHostEnvironment</param>
    /// <param name="fileName">имя создаваемого файла без расширения</param>
    /// <param name="action">описываемое действие</param>
    public static async Task CreateLogFolder_File(IWebHostEnvironment _env, string fileName, string action)
    {
        string logFolder = Path.Combine(_env.ContentRootPath, "Logs");
        DirectoryInfo di = new DirectoryInfo(logFolder);
        if (!di.Exists)
        {
            di.Create();
        }
        string logFile = Path.Combine(_env.ContentRootPath, "Logs", $"{fileName}.txt");
        using (StreamWriter fs = new(logFile, true))
        {
            await fs.WriteLineAsync($"{DateTime.UtcNow} {action}");
            fs.Close();
        }
    }
}
