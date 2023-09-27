using System.Text;

namespace ChromePasswordChecker.Models;
public static class ExportHelper
{
    public static void Export(this List<object> list, string path)
    {
        using (var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            foreach (var item in list)
            {
                var type = item.GetType().GetFields();
                var writeItem = string.Empty;
                foreach (var field in type)
                {
                    var value = (string?)field.GetValue(item);
                    writeItem += value + ",\t";
                }
                stream.Write(Encoding.UTF8.GetBytes(writeItem));
                stream.Write(Encoding.UTF8.GetBytes("\n"));
            }
        }
    }
}
