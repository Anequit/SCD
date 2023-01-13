using NUnit.Framework;
using SCD.Core.DataModels;
using SCD.Core.Helpers;
using System;

namespace SCD.Core.Tests.Helpers;

public class PartHelperTests
{
    [TestCase(1231515125, 1024 * 512)]
    [TestCase(13123156, 1024 * 32)]
    [TestCase(1115, 1024 * 64)]
    public void BuildPartArrayTest_ReturnsCorrectAmount(int contentLength, int buffer)
    {
        FileChunk[]? result = PartHelper.BuildPartArray(contentLength, buffer);

        Assert.True(Math.Abs(result.Length - Math.Ceiling((double)contentLength / buffer)) == 0);
    }

    [TestCase(1231515125, 1024 * 512)]
    [TestCase(13123156, 1024 * 32)]
    [TestCase(1115, 1024 * 64)]
    public void BuildPartArrayTest_FinalPartCorrectEndingHeader(int contentLength, int buffer)
    {
        FileChunk[]? result = PartHelper.BuildPartArray(contentLength, buffer);

        Assert.True(result[^1].EndingHeaderRange == contentLength);
    }

    [TestCase(0, 1024 * 32)]
    public void BuildPartArrayTest_ContentLengthZero(int contentLength, int buffer)
    {
        Assert.Throws<ArgumentOutOfRangeException>(delegate
        {
            PartHelper.BuildPartArray(contentLength, buffer);
        });
    }

    [TestCase(21312490, 0)]
    public void BuildPartArrayTest_BufferZero(int contentLength, int buffer)
    {
        Assert.Throws<ArgumentOutOfRangeException>(delegate
        {
            PartHelper.BuildPartArray(contentLength, buffer);
        });
    }
}
