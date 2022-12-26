using System;
using System.Linq;
using Xunit;

namespace MeyerCorp.HateoasBuilder.Test
{
    public class AddLinkTests : ExtensionTest
    {
        [Theory(DisplayName = "HttpContext.AddLink (pass)")]
        [InlineData("https://foo.bar/dingle/ball?value1=1&value2=2", "dingle/ball?value1=1&value2=2")]
        [InlineData("https://foo.bar/dingle", "dingle")]
        public void AddLinkHttpContextPassTest(string result, string relativeUrl)
        {
            var links = GetHttpContext()
                .AddLink(rel, relativeUrl)
                .Build();

            Assert.Equal(result, links.First().Href);
            Assert.Equal(rel, links.First().Rel);
        }

        [Theory(DisplayName = "String.AddLink (pass)")]
        [InlineData("https://foo.bar/dingle/ball?value1=1&value2=2", "dingle/ball?value1=1&value2=2")]
        [InlineData("https://foo.bar/dingle", "dingle")]
        public void AddLinkStringPassTest(string result, string relativeUrl)
        {
            var links = baseUrl
                .AddLink(rel, relativeUrl)
                .Build();

            Assert.Equal(result, links.First().Href);
            Assert.Equal(rel, links.First().Rel);
        }

        [Theory(DisplayName = "HttpContext.AddLink (fail)")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relLabel')", null, "dingle/ball?value1=1&value2=2")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relLabel')", "", "dingle")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relLabel')", "\t", "dingle")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relativeUrl')", rel, null)]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relativeUrl')", rel, "")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relativeUrl')", rel, "\t")]
        public void AddLinkHttpContextFailTest(string result, string rel, string relativeUrl)
        {
            var ex = Assert.Throws<ArgumentException>(() => GetHttpContext().AddLink(rel, relativeUrl));

            Assert.Equal(result, ex.Message);
        }

        [Theory(DisplayName = "String.AddLink (fail)")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'baseUrl')", null, rel, "dingle/ball?value1=1&value2=2")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'baseUrl')", "", rel, "dingle")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'baseUrl')", "\t", rel, "dingle")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relLabel')", baseUrl, null, "dingle/ball?value1=1&value2=2")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relLabel')", baseUrl, "", "dingle")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relLabel')", baseUrl, "\t", "dingle")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relativeUrl')", baseUrl, rel, null)]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relativeUrl')", baseUrl, rel, "")]
        [InlineData("Parameter cannot be null, empty or whitespace. (Parameter 'relativeUrl')", baseUrl, rel, "\t")]
        public void AddLinkStringFailTest(string result, string baseUrl, string label, string relativeUrl)
        {
            var ex = Assert.ThrowsAny<ArgumentException>(() => baseUrl.AddLink(label, relativeUrl));

            Assert.Equal(result, ex.Message);
        }
    }
}