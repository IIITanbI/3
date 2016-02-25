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

    [CommandManager(typeof(ApiManagerConfig), "Api", Description = "Manager for Api")]
    public class ApiManager : CommandManagerBase
    {
        private class LocalContainer
        {
            public ApiManagerConfig Config;
        }

        ThreadLocal<LocalContainer> _container;

        public ApiManager(ApiManagerConfig config)
            : base(config)
        {
            _container = new ThreadLocal<LocalContainer>(() =>
            {
                var localContainer = new LocalContainer();
                localContainer.Config = config;
                return localContainer;
            });
        }

        [Command("Request", Description = "Perform request")]
        public Response PerformRequest(Request request, ILogger log)
        {
            try
            {
                log?.DEBUG($"Perform {request.Method.ToString()} request");
                var rep = new Response();
                var req = (HttpWebRequest)WebRequest.Create(_container.Value.Config.EndPoint + request.PostData);
                req.Credentials = new NetworkCredential(_container.Value.Config.Username, _container.Value.Config.Password);
                req.Method = request.Method.ToString();
                req.ContentType = request.ContentType;
                var resp = (HttpWebResponse)req.GetResponse();
                rep.Content = resp.StatusCode.ToString();
                log?.DEBUG($"Performing {request.Method.ToString()} request completed");
                return rep;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during performing {request.Method.ToString()} request");
                throw new CommandAbortException($"Error occurred during performing {request.Method.ToString()} request", ex);
            }
        }

        public string PostRequest(string parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(_container.Value.Config.EndPoint + parameters);
            request.Method = "POST";
            request.ContentLength = 0;
            request.ContentType = "text/json";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseValue = string.Empty;

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
