using NUnit.Framework;
using SCD.Core.Utilities;

namespace SCD.Core.Tests.Utilities;

public class ParserTests
{
    [TestCase("https://cyberdrop.me/a/jCxmKJOD", "jCxmKJOD")]
    public void ParseAlbumIdentifierFromUrl_ReturnsCorrectIdentifier(string url, string identifier)
    {
        string result = Parser.ParseAlbumIdentifierFromUrl(url);

        Assert.That(result == identifier);
    }

    [TestCase("test<ro><ck", "testrock")]
    public void RemoveInvalidPathChars_ReturnsCorrectValue(string path, string correctValue)
    {
        string? result = Parser.ParseValidPath(path);

        Assert.That(result == correctValue);
    }

    [TestCase("test|||<rock", "testrock")]
    public void RemoveInvalidFilenameChars_ReturnsCorrectValue(string path, string correctValue)
    {
        string? result = Parser.ParseValidFilename(path);

        Assert.That(result == correctValue);
    }
}
