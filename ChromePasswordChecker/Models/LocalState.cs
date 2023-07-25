using System.Text.Json.Serialization;

namespace ChromePasswordChecker.Models;
public class LocalState
{
	[JsonPropertyName("os_crypt")]
	public OsCrypt OsCrypt { get; set; }
}

public class OsCrypt
{
	[JsonPropertyName("encrypted_key")]
	public string EncryptedKey { get; set; }
}