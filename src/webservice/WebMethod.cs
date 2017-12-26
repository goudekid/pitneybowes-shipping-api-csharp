/*
Copyright 2016 Pitney Bowes Inc.

Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
You may obtain a copy of the License in the README file or at
   https://opensource.org/licenses/MIT 
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
for the specific language governing permissions and limitations under the License.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/


using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PitneyBowes.Developer.ShippingApi
{

    public class WebMethod
    {
        private static bool Retry(HttpStatusCode status, List<ErrorDetail> errors)
        {
            foreach( var e in errors)
            {
                if (e.ErrorCode.Equals("PB-APIM-ERR-1003") || e.ErrorCode.Equals("PB-APIM-ERR-1003")) return true;
            }
            return false;
        }
        private async static Task<ShippingApiResponse<Response>> Request<Response, Request>(string uri, HttpVerb verb, Request request, bool deleteBody, ISession session = null) where Request : IShippingApiRequest
        {
            if (session == null) session = Globals.DefaultSession;
            var response = new ShippingApiResponse<Response>();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                int timeout = Globals.TimeOutMilliseconds;
                for (int retries = session.Retries; retries > 0; retries--)
                {
                    if (session.AuthToken == null || session.AuthToken.AccessToken == null ) //TODO: Check if token should have expired
                    {
                        var tokenResponse = TokenMethods.token().GetAwaiter().GetResult();
                        if (!tokenResponse.Success)
                        {
                            session.AuthToken = null;
                            if (retries == 1) // no more tries left
                            {
                                response.Errors = tokenResponse.Errors;
                                response.HttpStatus = tokenResponse.HttpStatus;
                                break;
                            }
                            if ( stopwatch.ElapsedMilliseconds > Globals.TimeOutMilliseconds)
                            {
                                response.HttpStatus = HttpStatusCode.RequestTimeout;
                                response.Errors.Add(new ErrorDetail() { ErrorCode = "Client Timeout", Message= "Client Timeout"});
                                break;
                            }
                            continue;
                        }
                        session.AuthToken = tokenResponse.APIResponse;
                    }

                    request.Authorization = new StringBuilder(session.AuthToken.AccessToken);
                    response = await session.Requester.HttpRequest<Response, Request>(uri, verb, request, deleteBody, session);
                    if (response.Success) break;
                    if (!Retry(response.HttpStatus, response.Errors)) break;
                    if (stopwatch.ElapsedMilliseconds > Globals.TimeOutMilliseconds)
                    {
                        response.HttpStatus = HttpStatusCode.RequestTimeout;
                        response.Errors.Add(new ErrorDetail() { ErrorCode = "Client Timeout", Message = "Client Timeout" });
                        break;
                    }
                }
            }
            catch(JsonSerializationException j)
            {
                response.HttpStatus = HttpStatusCode.InternalServerError;
                response.Errors.Add(new ErrorDetail(){ ErrorCode = "Deserialization error", Message = j.Message});
                if (session.ThrowExceptions)
                {
                    throw new ShippingAPIException(response, "DeserializationError", j);
                }
            }
            stopwatch.Stop();
            response.RequestTime = stopwatch.Elapsed;
            session.UpdateCounters(uri, response.Success, response.RequestTime);
            if (session.ThrowExceptions && !response.Success)
            {
                throw new ShippingAPIException(response);
            }
            return response;
        }
        public async static Task<ShippingApiResponse<Response>> Post<Response, Request>(string uri, Request request, ISession session = null) where Request : IShippingApiRequest
        {
            return await Request<Response, Request>(uri, HttpVerb.POST, request, false, session);
        }
        public async static Task<ShippingApiResponse<Response>> Put<Response, Request>(string uri, Request request, ISession session = null) where Request : IShippingApiRequest
        {
            return await Request<Response, Request>(uri, HttpVerb.PUT, request, false, session);
        }
        public async static Task<ShippingApiResponse<Response>> Get<Response, Request>(string uri, Request request, ISession session = null) where Request : IShippingApiRequest
        {
            return await Request<Response, Request>( uri, HttpVerb.GET, request, false, session); 
        }
        public async static Task<ShippingApiResponse<Response>> Delete<Response, Request>(string uri, Request request, ISession session = null) where Request : IShippingApiRequest
        {
            return await Request<Response, Request>( uri, HttpVerb.DELETE, request, false, session);
        }
        public async static Task<ShippingApiResponse<Response>> DeleteWithBody<Response, Request>(string uri, Request request, ISession session = null) where Request : IShippingApiRequest
        {
            return await Request<Response, Request>(uri, HttpVerb.DELETE, request, true, session);
        }

    }
}