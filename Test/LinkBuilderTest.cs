using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MeyerCorp.HateoasBuilder.Test
{
    public class LinkBuilderTest : ExtensionTest
    {
        [Theory(DisplayName = "Constructor (fail).")]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData(null)]
        public void ConstructorFailTest(string baseUrl)
        {
            var caught = Assert.Throws<ArgumentException>(() => new LinkBuilder(baseUrl));

            Assert.Equal("Parameter cannot be null, empty, or whitespace. (Parameter 'baseUrl')", caught.Message);
        }

        [Theory(DisplayName = "Check null/empty parameters.")]
        [InlineData(null, "asdf", new object[] { "test" }, "Parameter cannot be null, empty, or whitespace. (Parameter 'relLabel')")]
        [InlineData("asdf", null, new object[] { "test" }, "Parameter cannot be null, empty, or whitespace. (Parameter 'relativeUrlFormat')")]
        [InlineData("", "asdf", new object[] { "test" }, "Parameter cannot be null, empty, or whitespace. (Parameter 'relLabel')")]
        [InlineData("asdf", "", new object[] { "test" }, "Parameter cannot be null, empty, or whitespace. (Parameter 'relativeUrlFormat')")]
        [InlineData("asdf", null, new object[] { }, "Parameter cannot be null, empty, or whitespace. (Parameter 'relativeUrlFormat')")]
        [InlineData("asdf", "", new object[] { }, "Parameter cannot be null, empty, or whitespace. (Parameter 'relativeUrlFormat')")]
        public void LinkBuilder1Test(string? relLabel, string? relativeUrlFormat, IEnumerable<object> formatItems, string message)
        {
            var test = new LinkBuilder("https:meyerus.com");

            var caught = Assert.Throws<ArgumentException>(() => test.AddFormattedLink(relLabel, relativeUrlFormat, formatItems));

            Assert.Equal(message, caught.Message);
        }

        [Theory(DisplayName = "Check single link.")]
        [InlineData("http://foo.bar/asdfuno", "asdf{0}", new string[] { "uno" })]
        [InlineData("http://foo.bar/asdfunodos", "asdf{0}{1}", new string[] { "uno", "dos" })]
        [InlineData("http://foo.bar/asdftestdostres", "asdf{0}{1}{2}", new string[] { "test", "dos", "tres" })]
        public void LinkBuilder2Test(string result, string format, string[] items)
        {
            const string rel = "rel";
            var test = new LinkBuilder("http://foo.bar");

            var links = test.AddFormattedLink(rel, format, items.ToArray()).Build();
            Assert.Single(links);
            Assert.Equal(new Link(rel, result), links.First());
        }

        [Theory(DisplayName = "AddQueryLink (pass)")]
        [InlineData("http://foo.bar/baseUrl?value1=asdf&value2=0", "rel1", "baseUrl", "value1", "asdf", "value2", 0)]
        [InlineData("http://foo.bar/?value1=0&value2=2", "rel1", "", "value1", 0, "value2", "2")]
        [InlineData("http://foo.bar/?value1=0&value2=2", "rel1", null, "value1", 0, "value2", "2")]
        [InlineData("http://foo.bar/?value1=0&value2=2", "rel1", "\t", "value1", 0, "value2", "2")]
        [InlineData("http://foo.bar/base?value1=&value2=2", "rel1", "base", "value1", "", "value2", "2")]
        [InlineData("http://foo.bar/base?value1=&value2=2", "rel1", "base", "value1", null, "value2", "2")]
        [InlineData("http://foo.bar/base?value1=&value2=2", "rel1", "base", "value1", "\t", "value2", "2")]
        [InlineData("http://foo.bar/base?value2=2&value1=", "rel1", "base", "value2", "2", "value1", "")]
        [InlineData("http://foo.bar/base?value2=2&value1=", "rel1", "base", "value2", "2", "value1", null)]
        [InlineData("http://foo.bar/base?value2=2&value1=", "rel1", "base", "value2", "2", "value1", "\t")]
        public void LinkBuilder3Test(string result, string relLabel, string relativeUrl, string name1, object value1, string name2, object value2)
        {
            var test = new LinkBuilder("http://foo.bar");

            var links = test
                .AddQueryLink(relLabel, relativeUrl, name1, value1, name2, value2)
                .Build();

            Assert.Equal(relLabel, links.First().Rel);
            Assert.Equal(result, links.First().Href);
        }

        [Theory(DisplayName = "AddLink (pass)")]
        [InlineData("http://foo.bar", "base", "")]
        [InlineData("http://foo.bar", "base", null)]
        [InlineData("http://foo.bar", "base", "\t")]
        [InlineData("http://foo.bar/value1", "base", "value1")]
        public void LinkBuilder4Test(string result, string relLabel, string rawRelativeUrl)
        {
            var test = new LinkBuilder("http://foo.bar");

            var links = test.AddLink(relLabel, rawRelativeUrl).Build();

            Assert.Equal(relLabel, links.First().Rel);
            Assert.Equal(result, links.First().Href);
        }
  
        [Theory(DisplayName = "AddLink (pass)")]
        [InlineData("http://foo.bar", "base", "")]
        [InlineData("http://foo.bar", "base", null)]
        [InlineData("http://foo.bar", "base", "\t")]
        [InlineData("http://foo.bar/value1", "base", "value1")]
        public void LinkBuilder5Test(string result, string relLabel, string rawRelativeUrl)
        {
            var test = new LinkBuilder("http://foo.bar");

            var links = test.AddLink(relLabel, rawRelativeUrl).Build();

            Assert.Equal(relLabel, links.First().Rel);
            Assert.Equal(result, links.First().Href);
        }
  }
}