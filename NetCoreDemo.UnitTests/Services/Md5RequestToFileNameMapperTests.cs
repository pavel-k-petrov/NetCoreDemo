using FluentAssertions;
using HttpRequestProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetCoreDemo.UnitTests.Services
{
    [TestClass]
    public class Md5RequestToFileNameMapperTests
    {
        private Md5RequestToFileNameMapper _service;

        [TestInitialize]
        public void Initialize()
        {
            _service = new Md5RequestToFileNameMapper();
        }

        [DataTestMethod]
        [DataRow(null, "/simplePath/", null, "dbe50032d94ef9bcacae0fe06348d8e8")]
        [DataRow("POST", "/twoParts/Path", null, "35e359c56a2a1635f519a81d9f1b888d")]
        [DataRow("GET", "/Path", "?withQuery=true", "9a6316e38e971125e956b2af163a5f1d")]
        public void GetFileNameForRequest_Always_ProvidesExpectedResults(string method, string path, string queryString, string expectedFileName)
        {
            var request = new RequestModel
            {
                Method = method,
                Path = path,
                QueryString = queryString
            };

            var filename = _service.GetFileNameForRequest(request);

            filename.Should().Be(expectedFileName);
        }
    }
}
