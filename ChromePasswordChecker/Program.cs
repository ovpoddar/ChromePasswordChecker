using ChromePasswordChecker.Models;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text.Json;

var chromeFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome");

if (!Directory.Exists(chromeFolder))
{
    Console.WriteLine("You don't have chrome installed in your computer.");
    return;
}

var StoragePath = Path.Combine(chromeFolder, @"User Data\Local State");
var ChromePath = Path.Combine(chromeFolder, @"User Data");

var secreteKey = GetSecretKey();
var folders = new[]
{
    Directory.GetDirectories(ChromePath, "Default*", SearchOption.TopDirectoryOnly).FirstOrDefault(),
    Directory.GetDirectories(ChromePath, "*Profile*", SearchOption.TopDirectoryOnly).FirstOrDefault()
};
var export = new List<object>();
foreach (var folder in folders)
{
    var folderName = Path.GetFileName(folder);

    var dir = Path.Join(folder, "Login Data");
    using (var connection = GetDbConnection(dir))
    {
        if (secreteKey != null && connection != null)
        {
            connection.Open();
            using (var cmd = new SQLiteCommand("SELECT action_url, username_value, password_value FROM logins", connection))
            {
                var reader = cmd.ExecuteReader();
                var index = 0;
                while (reader.Read())
                {
                    var url = reader.GetString(0);
                    var username = reader.GetString(1);
                    var ciphertext = (byte[])reader.GetValue(2);
                    if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(username) && ciphertext != null)
                    {
                        // Decrypt the password
                        using (var aesGcm = new AesGcmInternal(secreteKey))
                        {
                            var decryptedPassword = aesGcm.DecryptAndGetPassword(ciphertext);
                            Console.WriteLine($"Sequence: {index}");
                            Console.WriteLine($"URL: {url}\nUser Name: {username}\nPassword: {decryptedPassword}\n");
                            export.Add((url, username, decryptedPassword));
                        }

                    }
                    index++;
                }
            }
            connection.Close();
        }
    }
}

Console.WriteLine("Do you want to export the password then provide the file path or press enter to exit out the program.");
var exportPath = Console.ReadLine();
if (string.IsNullOrWhiteSpace(exportPath) || !Directory.Exists(exportPath))
    return;
export.Export(Path.Combine(exportPath, "ChromeDecrypt.csv"));
byte[]? GetSecretKey()
{
    try
    {
        var textJson = File.Open(StoragePath, FileMode.Open);
        var objectJson = JsonSerializer.Deserialize(textJson, LocalStateContext.Default.LocalState);
        if (objectJson == null
            || objectJson.OsCrypt == null
            || string.IsNullOrWhiteSpace(objectJson.OsCrypt.EncryptedKey))
            throw new Exception();

        var secretkey = Convert.FromBase64String(objectJson.OsCrypt.EncryptedKey);
        secretkey = secretkey[5..];
        secretkey = ProtectedData.Unprotect(secretkey, null, DataProtectionScope.CurrentUser);
        return secretkey;
    }
    catch
    {
        return null;
    }

}


static SQLiteConnection? GetDbConnection(string chromePathLoginDb)
{
    try
    {
        File.Copy(chromePathLoginDb, "Loginvault.db", true);
        return new SQLiteConnection("Data Source=Loginvault.db");
    }
    catch (Exception e)
    {
        Console.WriteLine($"{e.Message}");
        Console.WriteLine("[ERR] Chrome database cannot be found");
        return null;
    }
}
