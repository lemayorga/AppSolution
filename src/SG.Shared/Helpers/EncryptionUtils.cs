using BC = BCrypt.Net.BCrypt;

namespace SG.Shared.Helpers;

public static class EncryptionUtils
{
    private static readonly int WorkFactor = 16;

    public static string HashText(string plainText)
    => BC.HashPassword(plainText, WorkFactor);

    public static bool EnhancedVerify(string hastText, string plainText) 
    => BC.EnhancedVerify(plainText, hastText);

    public static bool Verify(string hastText, string plainText) 
    => BC.Verify(plainText, hastText);

    public static string EnhancedHashPassword(string plainText) 
    => BC.EnhancedHashPassword(plainText);
}
