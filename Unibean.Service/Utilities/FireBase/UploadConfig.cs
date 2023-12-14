namespace Unibean.Service.Utilities.FireBase;

public record FireBaseFile
{
    public string URL { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string Extension { get; set; } = default!;
}
public class UploadConfig
{
    // Vulnurable Data
    public static readonly string API_KEY = "AIzaSyC5tWKQPYpWmGeViqKBTxF4--j2jwDbCHA";
    public static readonly string AuthDomain = "upload-file-2ac29.firebaseapp.com";
    public static readonly string Bucket = "upload-file-2ac29.appspot.com";
    public static readonly string AuthEmail = "admin@gmail.com";
    public static readonly string AuthPassword = "admin123";
}
