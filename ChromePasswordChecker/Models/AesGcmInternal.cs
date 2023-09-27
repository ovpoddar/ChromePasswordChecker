// Ignore Spelling: Aes Gcm

using System.Security.Cryptography;
using System.Text;

namespace ChromePasswordChecker.Models;
public readonly struct AesGcmInternal : IDisposable
{
    private readonly AesGcm _aes;
    public AesGcmInternal(byte[] secreteKey)
    {
        _aes = new AesGcm(secreteKey);
    }

    public readonly string DecryptAndGetPassword(byte[] cipherText)
    {
        try
        {
            var tag = cipherText[(cipherText.Length - 16)..];
            var nonce = cipherText[3..15];
            cipherText = cipherText[15..(cipherText.Length - 16)];
            var result = new byte[cipherText.Length];
            _aes.Decrypt(nonce, cipherText, tag, result);
            return Encoding.UTF8.GetString(result);
        }
        catch
        {
            return string.Empty;
        }
    }

    public void Dispose()
    {
        _aes.Dispose();
    }
}
