using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HttpRequestProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NetCoreDemo.UnitTests.Services
{
    [TestClass]
    public class FailFastRequestProcessorDecoratorTests
    {
        private Mock<IRequestProcessor> _requestProcessorMock;
        private Mock<IMessageBusHealthChecker> _messageBusHealthChecker;
        private FailFastRequestProcessorDecorator _service;

        [TestInitialize]
        public void Initialize()
        {
            _requestProcessorMock = new Mock<IRequestProcessor>();
            _messageBusHealthChecker = new Mock<IMessageBusHealthChecker>();
            _service = new FailFastRequestProcessorDecorator(_requestProcessorMock.Object, _messageBusHealthChecker.Object);
        }

        [TestMethod]
        public async Task Process_MessageBusOnline_RequestProcessorCalled()
        {
            _messageBusHealthChecker.Setup(x => x.GetStatus()).Returns(MessageBusHealthStatus.Online);
            var request = new RequestModel();
            var expectedResponse = new ResponseModel();
            _requestProcessorMock.Setup(x => x.Process(request, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            var response = await _service.Process(request, CancellationToken.None);

            _requestProcessorMock.Verify(x => x.Process(request, It.IsAny<CancellationToken>()), Times.Once);
            response.Should().Be(expectedResponse);
        }

        [TestMethod]
        public async Task Process_MessageBusIsNotOnline_RequestProcessorIsNotCalled()
        {
            _messageBusHealthChecker.Setup(x => x.GetStatus()).Returns(MessageBusHealthStatus.Offline);

            await _service.Process(new RequestModel(), CancellationToken.None);

            _requestProcessorMock.Verify(x => x.Process(It.IsAny<RequestModel>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task Process_MessageBusIsNotOnline_ResponseStatusCode503()
        {
            _messageBusHealthChecker.Setup(x => x.GetStatus()).Returns(MessageBusHealthStatus.Offline);

            var response = await _service.Process(new RequestModel(), CancellationToken.None);

            response.StatusCode.Should().Be(503);
        }
    }
}
