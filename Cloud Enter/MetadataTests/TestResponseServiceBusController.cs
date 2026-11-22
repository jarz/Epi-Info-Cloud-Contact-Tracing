using Epi.Cloud.DataConsistencyServices.Controllers;
using Epi.Cloud.DataConsistencyServices.Proxy;
using Epi.Cloud.ServiceBus;
using Epi.MetadataAccessService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;

namespace MetadataTests
{
    [TestClass]
    public class TestResponseServiceBusController
    {
        [TestMethod]
        public void GetUsesInjectedServiceBusService()
        {
            var service = new LocalResponseInfoServiceBus(new MessagePayload { Body = "local-message" });
            var controller = new ResponseServiceBusController(service);

            IHttpActionResult result = controller.Get();

            Assert.AreEqual(1, service.CallCount);
            Assert.IsInstanceOfType(result, typeof(ServiceResult<MessagePayload>));
        }

        private sealed class LocalResponseInfoServiceBus : IResponseInfoServiceBus
        {
            private readonly MessagePayload _payload;

            public LocalResponseInfoServiceBus(MessagePayload payload)
            {
                _payload = payload;
            }

            public int CallCount { get; private set; }

            public MessagePayload GetResponseInfoMessageFromServiceBus()
            {
                CallCount++;
                return _payload;
            }
        }
    }
}
