using OpenAI.Tokenizer.GPT3;

namespace OpenAI.Tests;

public class TokenizerGpt3Tests
{
    [Fact]
    public void Encode_EmptyString_ReturnsEmptyResult()
    {
        // Arrange
        var text = string.Empty;

        // Act
        var result = TokenizerGpt3.Encode(text);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Encode_SimpleString_ReturnsExpectedTokens()
    {
        // Arrange
        var text = "Hello, world!";
        int[] expectedTokens = { 15496, 11, 995, 0 };

        // Act
        var result = TokenizerGpt3.Encode(text);

        // Assert
        Assert.Equal(expectedTokens, result);
    }

    [Fact]
    public void TokenCount_EmptyString_ReturnsZero()
    {
        // Arrange
        var text = string.Empty;

        // Act
        var result = TokenizerGpt3.TokenCount(text);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void TokenCount_SimpleString_ReturnsExpectedCount()
    {
        // Arrange
        var text = "Hello, world!";
        var expectedCount = 4;

        // Act
        var result = TokenizerGpt3.TokenCount(text);

        // Assert
        Assert.Equal(expectedCount, result);
    }

    [Fact]
    public void Encode_StringWithCREOL_CleanUpCREOL_ReturnsExpectedTokens()
    {
        // Arrange
        var text = "Hello, world!\r\n";
        int[] expectedTokens = { 15496, 11, 995, 0, 198 };

        // Act
        var result = TokenizerGpt3.Encode(text, true);

        // Assert
        Assert.Equal(expectedTokens, result);
    }

    [Fact]
    public void TokenCount_StringWithCREOL_CleanUpCREOL_ReturnsExpectedCount()
    {
        // Arrange
        var text = "Hello, world!\r\n";
        var expectedCount = 5;

        // Act
        var result = TokenizerGpt3.TokenCount(text, true);

        // Assert
        Assert.Equal(expectedCount, result);
    }
}