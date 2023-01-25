using NUnit.Framework;
using OpenAI.GPT3.Extensions;
using System;

namespace OpenAI.Tests
{
    public class ExtensionTests
    {
        [Test]
        public void Extension_RemoveIfStartWith()
        {
            var text = "data: text";
            var result = text.RemoveIfStartWith("data: ");
            Console.WriteLine(result);
            Assert.That(result, Is.EqualTo("text"));
        }
    }
}