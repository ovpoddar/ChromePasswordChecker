using ChromePasswordChecker.Models;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text.Json;

var StoragePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Local State");
var ChromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data");

var secreteKey = GetSecretKey();
var folders = new[]
{
	Directory.GetDirectories(ChromePath, "Default*", SearchOption.TopDirectoryOnly).FirstOrDefault(),
	Directory.GetDirectories(ChromePath, "*Profile*", SearchOption.TopDirectoryOnly).FirstOrDefault()
};
foreach (var folder in folders)
{
	var folderName = Path.GetFileName(folder);

	var dir = Path.Join(folder, "Login Data");
	var connection = GetDbConnection(dir);
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
					}

				}
				index++;
			}
		}

		// Close database connection
		connection.Close();
		connection.Dispose();
		File.Delete("Loginvault.db");
	}
}

byte[] GetSecretKey()
{
	try
	{
		var textJson = File.Open(StoragePath, FileMode.Open);
		var objectJson = JsonSerializer.Deserialize<LocalState>(textJson);
		var secretkey = Convert.FromBase64String(objectJson!.OsCrypt.EncryptedKey);
		secretkey = secretkey[5..];
		secretkey = ProtectedData.Unprotect(secretkey, null, DataProtectionScope.CurrentUser);
		return secretkey;
	}
	catch
	{
		throw new Exception();
	}

}


static SQLiteConnection GetDbConnection(string chromePathLoginDb)
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
