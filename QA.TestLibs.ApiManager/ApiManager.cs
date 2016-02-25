namespace QA.TestLibs.ApiManager
{
    using Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager(typeof(ApiConfig), "Api", Description = "Manager for Api")]
    public class ApiManager : CommandManagerBase
    {
        private class LocalContainer
        {
            public ApiConfig Config;
            public Request Request;
            public Response Response;
        }

        ThreadLocal<LocalContainer> _container;

        public ApiManager(ApiConfig config, Request request, Response response)
            : base(config)
        {
            _container = new ThreadLocal<LocalContainer>(() =>
            {
                var localContainer = new LocalContainer();
                localContainer.Config = config;
                localContainer.Request = request;
                localContainer.Response = response;
                return localContainer;
            });
        }

        public string Request(string parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(_container.Value.Config.EndPoint + parameters);
            request.Method = _container.Value.Request.Method.ToString();
            request.ContentLength = 0;
            request.ContentType = _container.Value.Request.ContentType;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseValue = string.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = string.Format("Fail: Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }

                return responseValue;
            }
        }
    }
}
