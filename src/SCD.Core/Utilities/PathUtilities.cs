namespace SCD.Core.Utilities;

public static class PathUtilities
{
    public static string NormalizePath(string path)
    {
        string invalidChars = new string(Path.GetInvalidPathChars());

        foreach(char invalidChar in invalidChars)
            path.Replace($"{invalidChar}", string.Empty);

        return path;
    }
}
