/*
 * Vultr API
 *
 * # Introduction  The Vultr API v2 is a set of HTTP endpoints that adhere to RESTful design principles and CRUD actions with predictable URIs. It uses standard HTTP response codes, authentication, and verbs. The API has consistent and well-formed JSON requests and responses with cursor-based pagination to simplify list handling. Error messages are descriptive and easy to understand. All functions of the Vultr customer portal are accessible via the API, enabling you to script complex unattended scenarios with any tool fluent in HTTP.  ## Requests  Communicate with the API by making an HTTP request at the correct endpoint. The chosen method determines the action taken.  | Method | Usage | | - -- -- - | - -- -- -- -- -- -- | | DELETE | Use the DELETE method to destroy a resource in your account. If it is not found, the operation will return a 4xx error and an appropriate message. | | GET | To retrieve information about a resource, use the GET method. The data is returned as a JSON object. GET methods are read-only and do not affect any resources. | | PATCH | Some resources support partial modification with PATCH, which modifies specific attributes without updating the entire object representation. | | POST | Issue a POST method to create a new object. Include all needed attributes in the request body encoded as JSON. | | PUT | Use the PUT method to update information about a resource. PUT will set new values on the item without regard to their current values. |  **Rate Limit:** Vultr safeguards the API against bursts of incoming traffic based on the request's IP address to ensure stability for all users. If your application sends more than 30 requests per second, the API may return HTTP status code 429.  ## Response Codes  We use standard HTTP response codes to show the success or failure of requests. Response codes in the 2xx range indicate success, while codes in the 4xx range indicate an error, such as an authorization failure or a malformed request. All 4xx errors will return a JSON response object with an `error` attribute explaining the error. Codes in the 5xx range indicate a server-side problem preventing Vultr from fulfilling your request.  | Response | Description | | - -- -- - | - -- -- -- -- -- -- | | 200 OK | The response contains your requested information. | | 201 Created | Your request was accepted. The resource was created. | | 202 Accepted | Your request was accepted. The resource was created or updated. | | 204 No Content | Your request succeeded, there is no additional information returned. | | 400 Bad Request | Your request was malformed. | | 401 Unauthorized | You did not supply valid authentication credentials. | | 403 Forbidden | You are not allowed to perform that action. | | 404 Not Found | No results were found for your request. | | 429 Too Many Requests | Your request exceeded the API rate limit. | | 500 Internal Server Error | We were unable to perform the request due to server-side problems. |  ## Meta and Pagination  Many API calls will return a `meta` object with paging information.  ### Definitions  | Term | Description | | - -- -- - | - -- -- -- -- -- -- | | **List** | The items returned from the database for your request (not necessarily shown in a single response depending on the **cursor** size). | | **Page** | A subset of the full **list** of items. Choose the size of a **page** with the `per_page` parameter. | | **Total** | The `total` attribute indicates the number of items in the full **list**.| | **Cursor** | Use the `cursor` query parameter to select a next or previous **page**. | | **Next** & **Prev** | Use the `next` and `prev` attributes of the `links` meta object as `cursor` values. |  ### How to use Paging  If your result **list** total exceeds the default **cursor** size (the default depends on the route, but is usually 100 records) or the value defined by the `per_page` query param (when present) the response will be returned to you paginated.  ### Paging Example  > These examples have abbreviated attributes and sample values. Your actual `cursor` values will be encoded alphanumeric strings.  To return a **page** with the first two plans in the List:      curl \"https://api.vultr.com/v2/plans?per_page=2\" \\       -X GET \\       -H \"Authorization: Bearer ${VULTR_API_KEY}\"  The API returns an object similar to this:      {         \"plans\": [             {                 \"id\": \"vc2-1c-2gb\",                 \"vcpu_count\": 1,                 \"ram\": 2048,                 \"locations\": []             },             {                 \"id\": \"vc2-24c-97gb\",                 \"vcpu_count\": 24,                 \"ram\": 98304,                 \"locations\": []             }         ],         \"meta\": {             \"total\": 19,             \"links\": {                 \"next\": \"WxYzExampleNext\",                 \"prev\": \"WxYzExamplePrev\"             }         }     }  The object contains two plans. The `total` attribute indicates that 19 plans are available in the List. To navigate forward in the **list**, use the `next` value (`WxYzExampleNext` in this example) as your `cursor` query parameter.      curl \"https://api.vultr.com/v2/plans?per_page=2&cursor=WxYzExampleNext\" \\       -X GET       -H \"Authorization: Bearer ${VULTR_API_KEY}\"  Likewise, you can use the example `prev` value `WxYzExamplePrev` to navigate backward.  ## Parameters  You can pass information to the API with three different types of parameters.  ### Path parameters  Some API calls require variable parameters as part of the endpoint path. For example, to retrieve information about a user, supply the `user-id` in the endpoint.      curl \"https://api.vultr.com/v2/users/{user-id}\" \\       -X GET \\       -H \"Authorization: Bearer ${VULTR_API_KEY}\"  ### Query parameters  Some API calls allow filtering with query parameters. For example, the `/v2/plans` endpoint supports a `type` query parameter. Setting `type=vhf` instructs the API only to return High Frequency Compute plans in the list. You'll find more specifics about valid filters in the endpoint documentation below.      curl \"https://api.vultr.com/v2/plans?type=vhf\" \\       -X GET \\       -H \"Authorization: Bearer ${VULTR_API_KEY}\"  You can also combine filtering with paging. Use the `per_page` parameter to retreive a subset of vhf plans.      curl \"https://api.vultr.com/v2/plans?type=vhf&per_page=2\" \\       -X GET \\       -H \"Authorization: Bearer ${VULTR_API_KEY}\"  ### Request Body  PUT, POST, and PATCH methods may include an object in the request body with a content type of **application/json**. The documentation for each endpoint below has more information about the expected object.  ## API Example Conventions  The examples in this documentation use `curl`, a command-line HTTP client, to demonstrate useage. Linux and macOS computers usually have curl installed by default, and it's [available for download](https://curl.se/download.html) on all popular platforms including Windows.  Each example is split across multiple lines with the `\\` character, which is compatible with a `bash` terminal. A typical example looks like this:      curl \"https://api.vultr.com/v2/domains\" \\       -X POST \\       -H \"Authorization: Bearer ${VULTR_API_KEY}\" \\       -H \"Content-Type: application/json\" \\       - -data '{         \"domain\" : \"example.com\",         \"ip\" : \"192.0.2.123\"       }'  * The `-X` parameter sets the request method. For consistency, we show the method on all examples, even though it's not explicitly required for GET methods. * The `-H` lines set required HTTP headers. These examples are formatted to expand the VULTR\\_API\\_KEY environment variable for your convenience. * Examples that require a JSON object in the request body pass the required data via the `- -data` parameter.  All values in this guide are examples. Do not rely on the OS or Plan IDs listed in this guide; use the appropriate endpoint to retreive values before creating resources. 
 *
 * The version of the OpenAPI document: 2.0
 * Contact: support@vultr.com
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Org.OpenAPITools.Api
{

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ILoadBalancerApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Create Load Balancer
        /// </summary>
        /// <remarks>
        /// Create a new Load Balancer in a particular &#x60;region&#x60;.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>CreateLoadBalancer202Response</returns>
        CreateLoadBalancer202Response CreateLoadBalancer(CreateLoadBalancerRequest? createLoadBalancerRequest = default(CreateLoadBalancerRequest?), int operationIndex = 0);

        /// <summary>
        /// Create Load Balancer
        /// </summary>
        /// <remarks>
        /// Create a new Load Balancer in a particular &#x60;region&#x60;.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of CreateLoadBalancer202Response</returns>
        ApiResponse<CreateLoadBalancer202Response> CreateLoadBalancerWithHttpInfo(CreateLoadBalancerRequest? createLoadBalancerRequest = default(CreateLoadBalancerRequest?), int operationIndex = 0);
        /// <summary>
        /// Create Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Create a new forwarding rule for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="createLoadBalancerForwardingRulesRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void CreateLoadBalancerForwardingRules(string loadBalancerId, CreateLoadBalancerForwardingRulesRequest? createLoadBalancerForwardingRulesRequest = default(CreateLoadBalancerForwardingRulesRequest?), int operationIndex = 0);

        /// <summary>
        /// Create Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Create a new forwarding rule for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="createLoadBalancerForwardingRulesRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CreateLoadBalancerForwardingRulesWithHttpInfo(string loadBalancerId, CreateLoadBalancerForwardingRulesRequest? createLoadBalancerForwardingRulesRequest = default(CreateLoadBalancerForwardingRulesRequest?), int operationIndex = 0);
        /// <summary>
        /// Delete Load Balancer
        /// </summary>
        /// <remarks>
        /// Delete a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteLoadBalancer(string loadBalancerId, int operationIndex = 0);

        /// <summary>
        /// Delete Load Balancer
        /// </summary>
        /// <remarks>
        /// Delete a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteLoadBalancerWithHttpInfo(string loadBalancerId, int operationIndex = 0);
        /// <summary>
        /// Delete Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Delete a Forwarding Rule on a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteLoadBalancerForwardingRule(string loadBalancerId, string forwardingRuleId, int operationIndex = 0);

        /// <summary>
        /// Delete Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Delete a Forwarding Rule on a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteLoadBalancerForwardingRuleWithHttpInfo(string loadBalancerId, string forwardingRuleId, int operationIndex = 0);
        /// <summary>
        /// Delete Load Balancer SSL
        /// </summary>
        /// <remarks>
        /// Delete a Load Balancer SSL.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/delete-load-balancer-ssl).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteLoadBalancerSsl(string loadBalancerId, int operationIndex = 0);

        /// <summary>
        /// Delete Load Balancer SSL
        /// </summary>
        /// <remarks>
        /// Delete a Load Balancer SSL.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/delete-load-balancer-ssl).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteLoadBalancerSslWithHttpInfo(string loadBalancerId, int operationIndex = 0);
        /// <summary>
        /// Get Load Balancer
        /// </summary>
        /// <remarks>
        /// Get information for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>CreateLoadBalancer202Response</returns>
        CreateLoadBalancer202Response GetLoadBalancer(string loadBalancerId, int operationIndex = 0);

        /// <summary>
        /// Get Load Balancer
        /// </summary>
        /// <remarks>
        /// Get information for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of CreateLoadBalancer202Response</returns>
        ApiResponse<CreateLoadBalancer202Response> GetLoadBalancerWithHttpInfo(string loadBalancerId, int operationIndex = 0);
        /// <summary>
        /// Get Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Get information for a Forwarding Rule on a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>GetLoadBalancerForwardingRule200Response</returns>
        GetLoadBalancerForwardingRule200Response GetLoadBalancerForwardingRule(string loadBalancerId, string forwardingRuleId, int operationIndex = 0);

        /// <summary>
        /// Get Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Get information for a Forwarding Rule on a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of GetLoadBalancerForwardingRule200Response</returns>
        ApiResponse<GetLoadBalancerForwardingRule200Response> GetLoadBalancerForwardingRuleWithHttpInfo(string loadBalancerId, string forwardingRuleId, int operationIndex = 0);
        /// <summary>
        /// Get Firewall Rule
        /// </summary>
        /// <remarks>
        /// Get a firewall rule for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="firewallRuleId"></param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>LoadbalancerFirewallRule</returns>
        LoadbalancerFirewallRule GetLoadbalancerFirewallRule(string loadbalancerId, string firewallRuleId, int operationIndex = 0);

        /// <summary>
        /// Get Firewall Rule
        /// </summary>
        /// <remarks>
        /// Get a firewall rule for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="firewallRuleId"></param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of LoadbalancerFirewallRule</returns>
        ApiResponse<LoadbalancerFirewallRule> GetLoadbalancerFirewallRuleWithHttpInfo(string loadbalancerId, string firewallRuleId, int operationIndex = 0);
        /// <summary>
        /// List Forwarding Rules
        /// </summary>
        /// <remarks>
        /// List the fowarding rules for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ListLoadBalancerForwardingRules200Response</returns>
        ListLoadBalancerForwardingRules200Response ListLoadBalancerForwardingRules(string loadBalancerId, int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0);

        /// <summary>
        /// List Forwarding Rules
        /// </summary>
        /// <remarks>
        /// List the fowarding rules for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of ListLoadBalancerForwardingRules200Response</returns>
        ApiResponse<ListLoadBalancerForwardingRules200Response> ListLoadBalancerForwardingRulesWithHttpInfo(string loadBalancerId, int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0);
        /// <summary>
        /// List Load Balancers
        /// </summary>
        /// <remarks>
        /// List the Load Balancers in your account.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500.  (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ListLoadBalancers200Response</returns>
        ListLoadBalancers200Response ListLoadBalancers(int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0);

        /// <summary>
        /// List Load Balancers
        /// </summary>
        /// <remarks>
        /// List the Load Balancers in your account.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500.  (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of ListLoadBalancers200Response</returns>
        ApiResponse<ListLoadBalancers200Response> ListLoadBalancersWithHttpInfo(int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0);
        /// <summary>
        /// List Firewall Rules
        /// </summary>
        /// <remarks>
        /// List the firewall rules for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>LoadbalancerFirewallRule</returns>
        LoadbalancerFirewallRule ListLoadbalancerFirewallRules(string loadbalancerId, string? perPage = default(string?), string? cursor = default(string?), int operationIndex = 0);

        /// <summary>
        /// List Firewall Rules
        /// </summary>
        /// <remarks>
        /// List the firewall rules for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of LoadbalancerFirewallRule</returns>
        ApiResponse<LoadbalancerFirewallRule> ListLoadbalancerFirewallRulesWithHttpInfo(string loadbalancerId, string? perPage = default(string?), string? cursor = default(string?), int operationIndex = 0);
        /// <summary>
        /// Update Load Balancer
        /// </summary>
        /// <remarks>
        /// Update information for a Load Balancer. All attributes are optional. If not set, the attributes will retain their original values.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="updateLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void UpdateLoadBalancer(string loadBalancerId, UpdateLoadBalancerRequest? updateLoadBalancerRequest = default(UpdateLoadBalancerRequest?), int operationIndex = 0);

        /// <summary>
        /// Update Load Balancer
        /// </summary>
        /// <remarks>
        /// Update information for a Load Balancer. All attributes are optional. If not set, the attributes will retain their original values.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="updateLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> UpdateLoadBalancerWithHttpInfo(string loadBalancerId, UpdateLoadBalancerRequest? updateLoadBalancerRequest = default(UpdateLoadBalancerRequest?), int operationIndex = 0);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ILoadBalancerApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Create Load Balancer
        /// </summary>
        /// <remarks>
        /// Create a new Load Balancer in a particular &#x60;region&#x60;.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of CreateLoadBalancer202Response</returns>
        System.Threading.Tasks.Task<CreateLoadBalancer202Response> CreateLoadBalancerAsync(CreateLoadBalancerRequest? createLoadBalancerRequest = default(CreateLoadBalancerRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create Load Balancer
        /// </summary>
        /// <remarks>
        /// Create a new Load Balancer in a particular &#x60;region&#x60;.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (CreateLoadBalancer202Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<CreateLoadBalancer202Response>> CreateLoadBalancerWithHttpInfoAsync(CreateLoadBalancerRequest? createLoadBalancerRequest = default(CreateLoadBalancerRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Create a new forwarding rule for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="createLoadBalancerForwardingRulesRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CreateLoadBalancerForwardingRulesAsync(string loadBalancerId, CreateLoadBalancerForwardingRulesRequest? createLoadBalancerForwardingRulesRequest = default(CreateLoadBalancerForwardingRulesRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Create a new forwarding rule for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="createLoadBalancerForwardingRulesRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CreateLoadBalancerForwardingRulesWithHttpInfoAsync(string loadBalancerId, CreateLoadBalancerForwardingRulesRequest? createLoadBalancerForwardingRulesRequest = default(CreateLoadBalancerForwardingRulesRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete Load Balancer
        /// </summary>
        /// <remarks>
        /// Delete a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteLoadBalancerAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Load Balancer
        /// </summary>
        /// <remarks>
        /// Delete a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteLoadBalancerWithHttpInfoAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Delete a Forwarding Rule on a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteLoadBalancerForwardingRuleAsync(string loadBalancerId, string forwardingRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Delete a Forwarding Rule on a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteLoadBalancerForwardingRuleWithHttpInfoAsync(string loadBalancerId, string forwardingRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete Load Balancer SSL
        /// </summary>
        /// <remarks>
        /// Delete a Load Balancer SSL.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/delete-load-balancer-ssl).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteLoadBalancerSslAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete Load Balancer SSL
        /// </summary>
        /// <remarks>
        /// Delete a Load Balancer SSL.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/delete-load-balancer-ssl).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteLoadBalancerSslWithHttpInfoAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Load Balancer
        /// </summary>
        /// <remarks>
        /// Get information for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of CreateLoadBalancer202Response</returns>
        System.Threading.Tasks.Task<CreateLoadBalancer202Response> GetLoadBalancerAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Load Balancer
        /// </summary>
        /// <remarks>
        /// Get information for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (CreateLoadBalancer202Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<CreateLoadBalancer202Response>> GetLoadBalancerWithHttpInfoAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Get information for a Forwarding Rule on a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of GetLoadBalancerForwardingRule200Response</returns>
        System.Threading.Tasks.Task<GetLoadBalancerForwardingRule200Response> GetLoadBalancerForwardingRuleAsync(string loadBalancerId, string forwardingRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Forwarding Rule
        /// </summary>
        /// <remarks>
        /// Get information for a Forwarding Rule on a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (GetLoadBalancerForwardingRule200Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<GetLoadBalancerForwardingRule200Response>> GetLoadBalancerForwardingRuleWithHttpInfoAsync(string loadBalancerId, string forwardingRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get Firewall Rule
        /// </summary>
        /// <remarks>
        /// Get a firewall rule for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="firewallRuleId"></param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of LoadbalancerFirewallRule</returns>
        System.Threading.Tasks.Task<LoadbalancerFirewallRule> GetLoadbalancerFirewallRuleAsync(string loadbalancerId, string firewallRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get Firewall Rule
        /// </summary>
        /// <remarks>
        /// Get a firewall rule for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="firewallRuleId"></param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (LoadbalancerFirewallRule)</returns>
        System.Threading.Tasks.Task<ApiResponse<LoadbalancerFirewallRule>> GetLoadbalancerFirewallRuleWithHttpInfoAsync(string loadbalancerId, string firewallRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// List Forwarding Rules
        /// </summary>
        /// <remarks>
        /// List the fowarding rules for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ListLoadBalancerForwardingRules200Response</returns>
        System.Threading.Tasks.Task<ListLoadBalancerForwardingRules200Response> ListLoadBalancerForwardingRulesAsync(string loadBalancerId, int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// List Forwarding Rules
        /// </summary>
        /// <remarks>
        /// List the fowarding rules for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ListLoadBalancerForwardingRules200Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<ListLoadBalancerForwardingRules200Response>> ListLoadBalancerForwardingRulesWithHttpInfoAsync(string loadBalancerId, int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// List Load Balancers
        /// </summary>
        /// <remarks>
        /// List the Load Balancers in your account.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500.  (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ListLoadBalancers200Response</returns>
        System.Threading.Tasks.Task<ListLoadBalancers200Response> ListLoadBalancersAsync(int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// List Load Balancers
        /// </summary>
        /// <remarks>
        /// List the Load Balancers in your account.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500.  (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ListLoadBalancers200Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<ListLoadBalancers200Response>> ListLoadBalancersWithHttpInfoAsync(int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// List Firewall Rules
        /// </summary>
        /// <remarks>
        /// List the firewall rules for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of LoadbalancerFirewallRule</returns>
        System.Threading.Tasks.Task<LoadbalancerFirewallRule> ListLoadbalancerFirewallRulesAsync(string loadbalancerId, string? perPage = default(string?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// List Firewall Rules
        /// </summary>
        /// <remarks>
        /// List the firewall rules for a Load Balancer.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (LoadbalancerFirewallRule)</returns>
        System.Threading.Tasks.Task<ApiResponse<LoadbalancerFirewallRule>> ListLoadbalancerFirewallRulesWithHttpInfoAsync(string loadbalancerId, string? perPage = default(string?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Update Load Balancer
        /// </summary>
        /// <remarks>
        /// Update information for a Load Balancer. All attributes are optional. If not set, the attributes will retain their original values.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="updateLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task UpdateLoadBalancerAsync(string loadBalancerId, UpdateLoadBalancerRequest? updateLoadBalancerRequest = default(UpdateLoadBalancerRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Update Load Balancer
        /// </summary>
        /// <remarks>
        /// Update information for a Load Balancer. All attributes are optional. If not set, the attributes will retain their original values.
        /// </remarks>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="updateLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> UpdateLoadBalancerWithHttpInfoAsync(string loadBalancerId, UpdateLoadBalancerRequest? updateLoadBalancerRequest = default(UpdateLoadBalancerRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ILoadBalancerApi : ILoadBalancerApiSync, ILoadBalancerApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class LoadBalancerApi : ILoadBalancerApi
    {
        private Org.OpenAPITools.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancerApi"/> class.
        /// </summary>
        /// <returns></returns>
        public LoadBalancerApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancerApi"/> class.
        /// </summary>
        /// <returns></returns>
        public LoadBalancerApi(string basePath)
        {
            this.Configuration = Org.OpenAPITools.Client.Configuration.MergeConfigurations(
                Org.OpenAPITools.Client.GlobalConfiguration.Instance,
                new Org.OpenAPITools.Client.Configuration { BasePath = basePath }
            );
            this.Client = new Org.OpenAPITools.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new Org.OpenAPITools.Client.ApiClient(this.Configuration.BasePath);
            this.ExceptionFactory = Org.OpenAPITools.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancerApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public LoadBalancerApi(Org.OpenAPITools.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = Org.OpenAPITools.Client.Configuration.MergeConfigurations(
                Org.OpenAPITools.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.Client = new Org.OpenAPITools.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new Org.OpenAPITools.Client.ApiClient(this.Configuration.BasePath);
            ExceptionFactory = Org.OpenAPITools.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancerApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public LoadBalancerApi(Org.OpenAPITools.Client.ISynchronousClient client, Org.OpenAPITools.Client.IAsynchronousClient asyncClient, Org.OpenAPITools.Client.IReadableConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (asyncClient == null) throw new ArgumentNullException("asyncClient");
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
            this.ExceptionFactory = Org.OpenAPITools.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public Org.OpenAPITools.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public Org.OpenAPITools.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Org.OpenAPITools.Client.IReadableConfiguration Configuration { get; set; }

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public Org.OpenAPITools.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Create Load Balancer Create a new Load Balancer in a particular &#x60;region&#x60;.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>CreateLoadBalancer202Response</returns>
        public CreateLoadBalancer202Response CreateLoadBalancer(CreateLoadBalancerRequest? createLoadBalancerRequest = default(CreateLoadBalancerRequest?), int operationIndex = 0)
        {
            Org.OpenAPITools.Client.ApiResponse<CreateLoadBalancer202Response> localVarResponse = CreateLoadBalancerWithHttpInfo(createLoadBalancerRequest);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create Load Balancer Create a new Load Balancer in a particular &#x60;region&#x60;.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of CreateLoadBalancer202Response</returns>
        public Org.OpenAPITools.Client.ApiResponse<CreateLoadBalancer202Response> CreateLoadBalancerWithHttpInfo(CreateLoadBalancerRequest? createLoadBalancerRequest = default(CreateLoadBalancerRequest?), int operationIndex = 0)
        {
            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.Data = createLoadBalancerRequest;

            localVarRequestOptions.Operation = "LoadBalancerApi.CreateLoadBalancer";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<CreateLoadBalancer202Response>("/load-balancers", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("CreateLoadBalancer", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create Load Balancer Create a new Load Balancer in a particular &#x60;region&#x60;.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of CreateLoadBalancer202Response</returns>
        public async System.Threading.Tasks.Task<CreateLoadBalancer202Response> CreateLoadBalancerAsync(CreateLoadBalancerRequest? createLoadBalancerRequest = default(CreateLoadBalancerRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Org.OpenAPITools.Client.ApiResponse<CreateLoadBalancer202Response> localVarResponse = await CreateLoadBalancerWithHttpInfoAsync(createLoadBalancerRequest, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create Load Balancer Create a new Load Balancer in a particular &#x60;region&#x60;.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (CreateLoadBalancer202Response)</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<CreateLoadBalancer202Response>> CreateLoadBalancerWithHttpInfoAsync(CreateLoadBalancerRequest? createLoadBalancerRequest = default(CreateLoadBalancerRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.Data = createLoadBalancerRequest;

            localVarRequestOptions.Operation = "LoadBalancerApi.CreateLoadBalancer";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PostAsync<CreateLoadBalancer202Response>("/load-balancers", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("CreateLoadBalancer", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create Forwarding Rule Create a new forwarding rule for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="createLoadBalancerForwardingRulesRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void CreateLoadBalancerForwardingRules(string loadBalancerId, CreateLoadBalancerForwardingRulesRequest? createLoadBalancerForwardingRulesRequest = default(CreateLoadBalancerForwardingRulesRequest?), int operationIndex = 0)
        {
            CreateLoadBalancerForwardingRulesWithHttpInfo(loadBalancerId, createLoadBalancerForwardingRulesRequest);
        }

        /// <summary>
        /// Create Forwarding Rule Create a new forwarding rule for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="createLoadBalancerForwardingRulesRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public Org.OpenAPITools.Client.ApiResponse<Object> CreateLoadBalancerForwardingRulesWithHttpInfo(string loadBalancerId, CreateLoadBalancerForwardingRulesRequest? createLoadBalancerForwardingRulesRequest = default(CreateLoadBalancerForwardingRulesRequest?), int operationIndex = 0)
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->CreateLoadBalancerForwardingRules");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            localVarRequestOptions.Data = createLoadBalancerForwardingRulesRequest;

            localVarRequestOptions.Operation = "LoadBalancerApi.CreateLoadBalancerForwardingRules";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/load-balancers/{load-balancer-id}/forwarding-rules", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("CreateLoadBalancerForwardingRules", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create Forwarding Rule Create a new forwarding rule for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="createLoadBalancerForwardingRulesRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CreateLoadBalancerForwardingRulesAsync(string loadBalancerId, CreateLoadBalancerForwardingRulesRequest? createLoadBalancerForwardingRulesRequest = default(CreateLoadBalancerForwardingRulesRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await CreateLoadBalancerForwardingRulesWithHttpInfoAsync(loadBalancerId, createLoadBalancerForwardingRulesRequest, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Create Forwarding Rule Create a new forwarding rule for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="createLoadBalancerForwardingRulesRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<Object>> CreateLoadBalancerForwardingRulesWithHttpInfoAsync(string loadBalancerId, CreateLoadBalancerForwardingRulesRequest? createLoadBalancerForwardingRulesRequest = default(CreateLoadBalancerForwardingRulesRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->CreateLoadBalancerForwardingRules");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            localVarRequestOptions.Data = createLoadBalancerForwardingRulesRequest;

            localVarRequestOptions.Operation = "LoadBalancerApi.CreateLoadBalancerForwardingRules";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PostAsync<Object>("/load-balancers/{load-balancer-id}/forwarding-rules", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("CreateLoadBalancerForwardingRules", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete Load Balancer Delete a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteLoadBalancer(string loadBalancerId, int operationIndex = 0)
        {
            DeleteLoadBalancerWithHttpInfo(loadBalancerId);
        }

        /// <summary>
        /// Delete Load Balancer Delete a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public Org.OpenAPITools.Client.ApiResponse<Object> DeleteLoadBalancerWithHttpInfo(string loadBalancerId, int operationIndex = 0)
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->DeleteLoadBalancer");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.DeleteLoadBalancer";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<Object>("/load-balancers/{load-balancer-id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteLoadBalancer", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete Load Balancer Delete a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteLoadBalancerAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteLoadBalancerWithHttpInfoAsync(loadBalancerId, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete Load Balancer Delete a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<Object>> DeleteLoadBalancerWithHttpInfoAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->DeleteLoadBalancer");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.DeleteLoadBalancer";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/load-balancers/{load-balancer-id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteLoadBalancer", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete Forwarding Rule Delete a Forwarding Rule on a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteLoadBalancerForwardingRule(string loadBalancerId, string forwardingRuleId, int operationIndex = 0)
        {
            DeleteLoadBalancerForwardingRuleWithHttpInfo(loadBalancerId, forwardingRuleId);
        }

        /// <summary>
        /// Delete Forwarding Rule Delete a Forwarding Rule on a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public Org.OpenAPITools.Client.ApiResponse<Object> DeleteLoadBalancerForwardingRuleWithHttpInfo(string loadBalancerId, string forwardingRuleId, int operationIndex = 0)
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->DeleteLoadBalancerForwardingRule");
            }

            // verify the required parameter 'forwardingRuleId' is set
            if (forwardingRuleId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'forwardingRuleId' when calling LoadBalancerApi->DeleteLoadBalancerForwardingRule");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            localVarRequestOptions.PathParameters.Add("forwarding-rule-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(forwardingRuleId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.DeleteLoadBalancerForwardingRule";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<Object>("/load-balancers/{load-balancer-id}/forwarding-rules/{forwarding-rule-id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteLoadBalancerForwardingRule", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete Forwarding Rule Delete a Forwarding Rule on a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteLoadBalancerForwardingRuleAsync(string loadBalancerId, string forwardingRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteLoadBalancerForwardingRuleWithHttpInfoAsync(loadBalancerId, forwardingRuleId, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete Forwarding Rule Delete a Forwarding Rule on a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<Object>> DeleteLoadBalancerForwardingRuleWithHttpInfoAsync(string loadBalancerId, string forwardingRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->DeleteLoadBalancerForwardingRule");
            }

            // verify the required parameter 'forwardingRuleId' is set
            if (forwardingRuleId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'forwardingRuleId' when calling LoadBalancerApi->DeleteLoadBalancerForwardingRule");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            localVarRequestOptions.PathParameters.Add("forwarding-rule-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(forwardingRuleId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.DeleteLoadBalancerForwardingRule";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/load-balancers/{load-balancer-id}/forwarding-rules/{forwarding-rule-id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteLoadBalancerForwardingRule", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete Load Balancer SSL Delete a Load Balancer SSL.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/delete-load-balancer-ssl).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteLoadBalancerSsl(string loadBalancerId, int operationIndex = 0)
        {
            DeleteLoadBalancerSslWithHttpInfo(loadBalancerId);
        }

        /// <summary>
        /// Delete Load Balancer SSL Delete a Load Balancer SSL.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/delete-load-balancer-ssl).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public Org.OpenAPITools.Client.ApiResponse<Object> DeleteLoadBalancerSslWithHttpInfo(string loadBalancerId, int operationIndex = 0)
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->DeleteLoadBalancerSsl");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.DeleteLoadBalancerSsl";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<Object>("/load-balancers/{load-balancer-id}/ssl", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteLoadBalancerSsl", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete Load Balancer SSL Delete a Load Balancer SSL.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/delete-load-balancer-ssl).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteLoadBalancerSslAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteLoadBalancerSslWithHttpInfoAsync(loadBalancerId, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete Load Balancer SSL Delete a Load Balancer SSL.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/delete-load-balancer-ssl).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<Object>> DeleteLoadBalancerSslWithHttpInfoAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->DeleteLoadBalancerSsl");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.DeleteLoadBalancerSsl";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/load-balancers/{load-balancer-id}/ssl", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteLoadBalancerSsl", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get Load Balancer Get information for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>CreateLoadBalancer202Response</returns>
        public CreateLoadBalancer202Response GetLoadBalancer(string loadBalancerId, int operationIndex = 0)
        {
            Org.OpenAPITools.Client.ApiResponse<CreateLoadBalancer202Response> localVarResponse = GetLoadBalancerWithHttpInfo(loadBalancerId);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get Load Balancer Get information for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of CreateLoadBalancer202Response</returns>
        public Org.OpenAPITools.Client.ApiResponse<CreateLoadBalancer202Response> GetLoadBalancerWithHttpInfo(string loadBalancerId, int operationIndex = 0)
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->GetLoadBalancer");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.GetLoadBalancer";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<CreateLoadBalancer202Response>("/load-balancers/{load-balancer-id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetLoadBalancer", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get Load Balancer Get information for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of CreateLoadBalancer202Response</returns>
        public async System.Threading.Tasks.Task<CreateLoadBalancer202Response> GetLoadBalancerAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Org.OpenAPITools.Client.ApiResponse<CreateLoadBalancer202Response> localVarResponse = await GetLoadBalancerWithHttpInfoAsync(loadBalancerId, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get Load Balancer Get information for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (CreateLoadBalancer202Response)</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<CreateLoadBalancer202Response>> GetLoadBalancerWithHttpInfoAsync(string loadBalancerId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->GetLoadBalancer");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.GetLoadBalancer";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<CreateLoadBalancer202Response>("/load-balancers/{load-balancer-id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetLoadBalancer", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get Forwarding Rule Get information for a Forwarding Rule on a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>GetLoadBalancerForwardingRule200Response</returns>
        public GetLoadBalancerForwardingRule200Response GetLoadBalancerForwardingRule(string loadBalancerId, string forwardingRuleId, int operationIndex = 0)
        {
            Org.OpenAPITools.Client.ApiResponse<GetLoadBalancerForwardingRule200Response> localVarResponse = GetLoadBalancerForwardingRuleWithHttpInfo(loadBalancerId, forwardingRuleId);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get Forwarding Rule Get information for a Forwarding Rule on a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of GetLoadBalancerForwardingRule200Response</returns>
        public Org.OpenAPITools.Client.ApiResponse<GetLoadBalancerForwardingRule200Response> GetLoadBalancerForwardingRuleWithHttpInfo(string loadBalancerId, string forwardingRuleId, int operationIndex = 0)
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->GetLoadBalancerForwardingRule");
            }

            // verify the required parameter 'forwardingRuleId' is set
            if (forwardingRuleId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'forwardingRuleId' when calling LoadBalancerApi->GetLoadBalancerForwardingRule");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            localVarRequestOptions.PathParameters.Add("forwarding-rule-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(forwardingRuleId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.GetLoadBalancerForwardingRule";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<GetLoadBalancerForwardingRule200Response>("/load-balancers/{load-balancer-id}/forwarding-rules/{forwarding-rule-id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetLoadBalancerForwardingRule", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get Forwarding Rule Get information for a Forwarding Rule on a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of GetLoadBalancerForwardingRule200Response</returns>
        public async System.Threading.Tasks.Task<GetLoadBalancerForwardingRule200Response> GetLoadBalancerForwardingRuleAsync(string loadBalancerId, string forwardingRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Org.OpenAPITools.Client.ApiResponse<GetLoadBalancerForwardingRule200Response> localVarResponse = await GetLoadBalancerForwardingRuleWithHttpInfoAsync(loadBalancerId, forwardingRuleId, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get Forwarding Rule Get information for a Forwarding Rule on a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="forwardingRuleId">The [Forwarding Rule id](#operation/list-load-balancer-forwarding-rules).</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (GetLoadBalancerForwardingRule200Response)</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<GetLoadBalancerForwardingRule200Response>> GetLoadBalancerForwardingRuleWithHttpInfoAsync(string loadBalancerId, string forwardingRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->GetLoadBalancerForwardingRule");
            }

            // verify the required parameter 'forwardingRuleId' is set
            if (forwardingRuleId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'forwardingRuleId' when calling LoadBalancerApi->GetLoadBalancerForwardingRule");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            localVarRequestOptions.PathParameters.Add("forwarding-rule-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(forwardingRuleId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.GetLoadBalancerForwardingRule";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<GetLoadBalancerForwardingRule200Response>("/load-balancers/{load-balancer-id}/forwarding-rules/{forwarding-rule-id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetLoadBalancerForwardingRule", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get Firewall Rule Get a firewall rule for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="firewallRuleId"></param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>LoadbalancerFirewallRule</returns>
        public LoadbalancerFirewallRule GetLoadbalancerFirewallRule(string loadbalancerId, string firewallRuleId, int operationIndex = 0)
        {
            Org.OpenAPITools.Client.ApiResponse<LoadbalancerFirewallRule> localVarResponse = GetLoadbalancerFirewallRuleWithHttpInfo(loadbalancerId, firewallRuleId);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get Firewall Rule Get a firewall rule for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="firewallRuleId"></param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of LoadbalancerFirewallRule</returns>
        public Org.OpenAPITools.Client.ApiResponse<LoadbalancerFirewallRule> GetLoadbalancerFirewallRuleWithHttpInfo(string loadbalancerId, string firewallRuleId, int operationIndex = 0)
        {
            // verify the required parameter 'loadbalancerId' is set
            if (loadbalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadbalancerId' when calling LoadBalancerApi->GetLoadbalancerFirewallRule");
            }

            // verify the required parameter 'firewallRuleId' is set
            if (firewallRuleId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'firewallRuleId' when calling LoadBalancerApi->GetLoadbalancerFirewallRule");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("loadbalancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadbalancerId)); // path parameter
            localVarRequestOptions.PathParameters.Add("firewall-rule-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(firewallRuleId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.GetLoadbalancerFirewallRule";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<LoadbalancerFirewallRule>("/load-balancers/{loadbalancer-id}/firewall-rules/{firewall-rule-id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetLoadbalancerFirewallRule", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get Firewall Rule Get a firewall rule for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="firewallRuleId"></param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of LoadbalancerFirewallRule</returns>
        public async System.Threading.Tasks.Task<LoadbalancerFirewallRule> GetLoadbalancerFirewallRuleAsync(string loadbalancerId, string firewallRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Org.OpenAPITools.Client.ApiResponse<LoadbalancerFirewallRule> localVarResponse = await GetLoadbalancerFirewallRuleWithHttpInfoAsync(loadbalancerId, firewallRuleId, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get Firewall Rule Get a firewall rule for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="firewallRuleId"></param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (LoadbalancerFirewallRule)</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<LoadbalancerFirewallRule>> GetLoadbalancerFirewallRuleWithHttpInfoAsync(string loadbalancerId, string firewallRuleId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadbalancerId' is set
            if (loadbalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadbalancerId' when calling LoadBalancerApi->GetLoadbalancerFirewallRule");
            }

            // verify the required parameter 'firewallRuleId' is set
            if (firewallRuleId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'firewallRuleId' when calling LoadBalancerApi->GetLoadbalancerFirewallRule");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("loadbalancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadbalancerId)); // path parameter
            localVarRequestOptions.PathParameters.Add("firewall-rule-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(firewallRuleId)); // path parameter

            localVarRequestOptions.Operation = "LoadBalancerApi.GetLoadbalancerFirewallRule";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<LoadbalancerFirewallRule>("/load-balancers/{loadbalancer-id}/firewall-rules/{firewall-rule-id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetLoadbalancerFirewallRule", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// List Forwarding Rules List the fowarding rules for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ListLoadBalancerForwardingRules200Response</returns>
        public ListLoadBalancerForwardingRules200Response ListLoadBalancerForwardingRules(string loadBalancerId, int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0)
        {
            Org.OpenAPITools.Client.ApiResponse<ListLoadBalancerForwardingRules200Response> localVarResponse = ListLoadBalancerForwardingRulesWithHttpInfo(loadBalancerId, perPage, cursor);
            return localVarResponse.Data;
        }

        /// <summary>
        /// List Forwarding Rules List the fowarding rules for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of ListLoadBalancerForwardingRules200Response</returns>
        public Org.OpenAPITools.Client.ApiResponse<ListLoadBalancerForwardingRules200Response> ListLoadBalancerForwardingRulesWithHttpInfo(string loadBalancerId, int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->ListLoadBalancerForwardingRules");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            if (perPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "per_page", perPage));
            }
            if (cursor != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "cursor", cursor));
            }

            localVarRequestOptions.Operation = "LoadBalancerApi.ListLoadBalancerForwardingRules";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<ListLoadBalancerForwardingRules200Response>("/load-balancers/{load-balancer-id}/forwarding-rules", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListLoadBalancerForwardingRules", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// List Forwarding Rules List the fowarding rules for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ListLoadBalancerForwardingRules200Response</returns>
        public async System.Threading.Tasks.Task<ListLoadBalancerForwardingRules200Response> ListLoadBalancerForwardingRulesAsync(string loadBalancerId, int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Org.OpenAPITools.Client.ApiResponse<ListLoadBalancerForwardingRules200Response> localVarResponse = await ListLoadBalancerForwardingRulesWithHttpInfoAsync(loadBalancerId, perPage, cursor, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// List Forwarding Rules List the fowarding rules for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ListLoadBalancerForwardingRules200Response)</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<ListLoadBalancerForwardingRules200Response>> ListLoadBalancerForwardingRulesWithHttpInfoAsync(string loadBalancerId, int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->ListLoadBalancerForwardingRules");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            if (perPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "per_page", perPage));
            }
            if (cursor != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "cursor", cursor));
            }

            localVarRequestOptions.Operation = "LoadBalancerApi.ListLoadBalancerForwardingRules";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<ListLoadBalancerForwardingRules200Response>("/load-balancers/{load-balancer-id}/forwarding-rules", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListLoadBalancerForwardingRules", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// List Load Balancers List the Load Balancers in your account.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500.  (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ListLoadBalancers200Response</returns>
        public ListLoadBalancers200Response ListLoadBalancers(int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0)
        {
            Org.OpenAPITools.Client.ApiResponse<ListLoadBalancers200Response> localVarResponse = ListLoadBalancersWithHttpInfo(perPage, cursor);
            return localVarResponse.Data;
        }

        /// <summary>
        /// List Load Balancers List the Load Balancers in your account.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500.  (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of ListLoadBalancers200Response</returns>
        public Org.OpenAPITools.Client.ApiResponse<ListLoadBalancers200Response> ListLoadBalancersWithHttpInfo(int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0)
        {
            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            if (perPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "per_page", perPage));
            }
            if (cursor != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "cursor", cursor));
            }

            localVarRequestOptions.Operation = "LoadBalancerApi.ListLoadBalancers";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<ListLoadBalancers200Response>("/load-balancers", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListLoadBalancers", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// List Load Balancers List the Load Balancers in your account.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500.  (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ListLoadBalancers200Response</returns>
        public async System.Threading.Tasks.Task<ListLoadBalancers200Response> ListLoadBalancersAsync(int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Org.OpenAPITools.Client.ApiResponse<ListLoadBalancers200Response> localVarResponse = await ListLoadBalancersWithHttpInfoAsync(perPage, cursor, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// List Load Balancers List the Load Balancers in your account.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500.  (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (ListLoadBalancers200Response)</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<ListLoadBalancers200Response>> ListLoadBalancersWithHttpInfoAsync(int? perPage = default(int?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            if (perPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "per_page", perPage));
            }
            if (cursor != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "cursor", cursor));
            }

            localVarRequestOptions.Operation = "LoadBalancerApi.ListLoadBalancers";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<ListLoadBalancers200Response>("/load-balancers", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListLoadBalancers", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// List Firewall Rules List the firewall rules for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>LoadbalancerFirewallRule</returns>
        public LoadbalancerFirewallRule ListLoadbalancerFirewallRules(string loadbalancerId, string? perPage = default(string?), string? cursor = default(string?), int operationIndex = 0)
        {
            Org.OpenAPITools.Client.ApiResponse<LoadbalancerFirewallRule> localVarResponse = ListLoadbalancerFirewallRulesWithHttpInfo(loadbalancerId, perPage, cursor);
            return localVarResponse.Data;
        }

        /// <summary>
        /// List Firewall Rules List the firewall rules for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of LoadbalancerFirewallRule</returns>
        public Org.OpenAPITools.Client.ApiResponse<LoadbalancerFirewallRule> ListLoadbalancerFirewallRulesWithHttpInfo(string loadbalancerId, string? perPage = default(string?), string? cursor = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'loadbalancerId' is set
            if (loadbalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadbalancerId' when calling LoadBalancerApi->ListLoadbalancerFirewallRules");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("loadbalancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadbalancerId)); // path parameter
            if (perPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "per_page", perPage));
            }
            if (cursor != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "cursor", cursor));
            }

            localVarRequestOptions.Operation = "LoadBalancerApi.ListLoadbalancerFirewallRules";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<LoadbalancerFirewallRule>("/load-balancers/{loadbalancer-id}/firewall-rules", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListLoadbalancerFirewallRules", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// List Firewall Rules List the firewall rules for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of LoadbalancerFirewallRule</returns>
        public async System.Threading.Tasks.Task<LoadbalancerFirewallRule> ListLoadbalancerFirewallRulesAsync(string loadbalancerId, string? perPage = default(string?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            Org.OpenAPITools.Client.ApiResponse<LoadbalancerFirewallRule> localVarResponse = await ListLoadbalancerFirewallRulesWithHttpInfoAsync(loadbalancerId, perPage, cursor, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// List Firewall Rules List the firewall rules for a Load Balancer.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadbalancerId"></param>
        /// <param name="perPage">Number of items requested per page. Default is 100 and Max is 500. (optional)</param>
        /// <param name="cursor">Cursor for paging. See [Meta and Pagination](#section/Introduction/Meta-and-Pagination). (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (LoadbalancerFirewallRule)</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<LoadbalancerFirewallRule>> ListLoadbalancerFirewallRulesWithHttpInfoAsync(string loadbalancerId, string? perPage = default(string?), string? cursor = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadbalancerId' is set
            if (loadbalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadbalancerId' when calling LoadBalancerApi->ListLoadbalancerFirewallRules");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("loadbalancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadbalancerId)); // path parameter
            if (perPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "per_page", perPage));
            }
            if (cursor != null)
            {
                localVarRequestOptions.QueryParameters.Add(Org.OpenAPITools.Client.ClientUtils.ParameterToMultiMap("", "cursor", cursor));
            }

            localVarRequestOptions.Operation = "LoadBalancerApi.ListLoadbalancerFirewallRules";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<LoadbalancerFirewallRule>("/load-balancers/{loadbalancer-id}/firewall-rules", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListLoadbalancerFirewallRules", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update Load Balancer Update information for a Load Balancer. All attributes are optional. If not set, the attributes will retain their original values.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="updateLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void UpdateLoadBalancer(string loadBalancerId, UpdateLoadBalancerRequest? updateLoadBalancerRequest = default(UpdateLoadBalancerRequest?), int operationIndex = 0)
        {
            UpdateLoadBalancerWithHttpInfo(loadBalancerId, updateLoadBalancerRequest);
        }

        /// <summary>
        /// Update Load Balancer Update information for a Load Balancer. All attributes are optional. If not set, the attributes will retain their original values.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="updateLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public Org.OpenAPITools.Client.ApiResponse<Object> UpdateLoadBalancerWithHttpInfo(string loadBalancerId, UpdateLoadBalancerRequest? updateLoadBalancerRequest = default(UpdateLoadBalancerRequest?), int operationIndex = 0)
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->UpdateLoadBalancer");
            }

            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            localVarRequestOptions.Data = updateLoadBalancerRequest;

            localVarRequestOptions.Operation = "LoadBalancerApi.UpdateLoadBalancer";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Patch<Object>("/load-balancers/{load-balancer-id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("UpdateLoadBalancer", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update Load Balancer Update information for a Load Balancer. All attributes are optional. If not set, the attributes will retain their original values.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="updateLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task UpdateLoadBalancerAsync(string loadBalancerId, UpdateLoadBalancerRequest? updateLoadBalancerRequest = default(UpdateLoadBalancerRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await UpdateLoadBalancerWithHttpInfoAsync(loadBalancerId, updateLoadBalancerRequest, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update Load Balancer Update information for a Load Balancer. All attributes are optional. If not set, the attributes will retain their original values.
        /// </summary>
        /// <exception cref="Org.OpenAPITools.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loadBalancerId">The [Load Balancer id](#operation/list-load-balancers).</param>
        /// <param name="updateLoadBalancerRequest">Include a JSON object in the request body with a content type of **application/json**. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<Org.OpenAPITools.Client.ApiResponse<Object>> UpdateLoadBalancerWithHttpInfoAsync(string loadBalancerId, UpdateLoadBalancerRequest? updateLoadBalancerRequest = default(UpdateLoadBalancerRequest?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'loadBalancerId' is set
            if (loadBalancerId == null)
            {
                throw new Org.OpenAPITools.Client.ApiException(400, "Missing required parameter 'loadBalancerId' when calling LoadBalancerApi->UpdateLoadBalancer");
            }


            Org.OpenAPITools.Client.RequestOptions localVarRequestOptions = new Org.OpenAPITools.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
            };

            var localVarContentType = Org.OpenAPITools.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = Org.OpenAPITools.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("load-balancer-id", Org.OpenAPITools.Client.ClientUtils.ParameterToString(loadBalancerId)); // path parameter
            localVarRequestOptions.Data = updateLoadBalancerRequest;

            localVarRequestOptions.Operation = "LoadBalancerApi.UpdateLoadBalancer";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (API Key) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PatchAsync<Object>("/load-balancers/{load-balancer-id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("UpdateLoadBalancer", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

    }
}
