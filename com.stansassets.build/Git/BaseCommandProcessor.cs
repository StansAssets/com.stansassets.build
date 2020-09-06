using System;
using UnityEngine.Assertions;

namespace StansAssets.Git
{
    abstract class BaseCommandProcessor
    {
        readonly Func<string, string> m_RequestsProcessor;

        protected BaseCommandProcessor(Func<string, string> requestsProcessor)
        {
            Assert.IsNotNull(requestsProcessor);
            m_RequestsProcessor = requestsProcessor;
        }

        protected string SendRequest(string request) => m_RequestsProcessor(request); //[TODO] not string type for the request, use a "Request" type
    }
}
