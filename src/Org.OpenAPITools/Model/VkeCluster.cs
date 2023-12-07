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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Org.OpenAPITools.Client.OpenAPIDateConverter;

namespace Org.OpenAPITools.Model
{
    /// <summary>
    /// VKE Cluster
    /// </summary>
    [DataContract(Name = "vke-cluster")]
    public partial class VkeCluster : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VkeCluster" /> class.
        /// </summary>
        /// <param name="id">ID for the VKE cluster.</param>
        /// <param name="firewallGroupId">The [Firewall Group id](#operation/list-firewall-groups) linked to this cluster..</param>
        /// <param name="label">Label for your cluster.</param>
        /// <param name="dateCreated">Date of creation.</param>
        /// <param name="clusterSubnet">IP range that your pods will run on in this cluster.</param>
        /// <param name="serviceSubnet">IP range that services will run on this cluster.</param>
        /// <param name="ip">IP for your Kubernetes Clusters Control Plane.</param>
        /// <param name="endpoint">Domain for your Kubernetes Clusters Control Plane.</param>
        /// <param name="varVersion">Version of Kubernetes this cluster is running on.</param>
        /// <param name="region">Region this Kubernetes Cluster is running in.</param>
        /// <param name="status">Status for VKE cluster.</param>
        /// <param name="haControlplanes">Whether a highly available control planes configuration has been deployed * true * false (default).</param>
        /// <param name="nodePools">NodePools in this cluster.</param>
        public VkeCluster(string id = default(string), string firewallGroupId = default(string), string label = default(string), string dateCreated = default(string), string clusterSubnet = default(string), string serviceSubnet = default(string), string ip = default(string), string endpoint = default(string), string varVersion = default(string), string region = default(string), string status = default(string), bool haControlplanes = default(bool), List<Nodepools> nodePools = default(List<Nodepools>))
        {
            this.Id = id;
            this.FirewallGroupId = firewallGroupId;
            this.Label = label;
            this.DateCreated = dateCreated;
            this.ClusterSubnet = clusterSubnet;
            this.ServiceSubnet = serviceSubnet;
            this.Ip = ip;
            this.Endpoint = endpoint;
            this.VarVersion = varVersion;
            this.Region = region;
            this.Status = status;
            this.HaControlplanes = haControlplanes;
            this.NodePools = nodePools;
        }

        /// <summary>
        /// ID for the VKE cluster
        /// </summary>
        /// <value>ID for the VKE cluster</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// The [Firewall Group id](#operation/list-firewall-groups) linked to this cluster.
        /// </summary>
        /// <value>The [Firewall Group id](#operation/list-firewall-groups) linked to this cluster.</value>
        [DataMember(Name = "firewall_group_id", EmitDefaultValue = false)]
        public string FirewallGroupId { get; set; }

        /// <summary>
        /// Label for your cluster
        /// </summary>
        /// <value>Label for your cluster</value>
        [DataMember(Name = "label", EmitDefaultValue = false)]
        public string Label { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        /// <value>Date of creation</value>
        [DataMember(Name = "date_created", EmitDefaultValue = false)]
        public string DateCreated { get; set; }

        /// <summary>
        /// IP range that your pods will run on in this cluster
        /// </summary>
        /// <value>IP range that your pods will run on in this cluster</value>
        [DataMember(Name = "cluster_subnet", EmitDefaultValue = false)]
        public string ClusterSubnet { get; set; }

        /// <summary>
        /// IP range that services will run on this cluster
        /// </summary>
        /// <value>IP range that services will run on this cluster</value>
        [DataMember(Name = "service_subnet", EmitDefaultValue = false)]
        public string ServiceSubnet { get; set; }

        /// <summary>
        /// IP for your Kubernetes Clusters Control Plane
        /// </summary>
        /// <value>IP for your Kubernetes Clusters Control Plane</value>
        [DataMember(Name = "ip", EmitDefaultValue = false)]
        public string Ip { get; set; }

        /// <summary>
        /// Domain for your Kubernetes Clusters Control Plane
        /// </summary>
        /// <value>Domain for your Kubernetes Clusters Control Plane</value>
        [DataMember(Name = "endpoint", EmitDefaultValue = false)]
        public string Endpoint { get; set; }

        /// <summary>
        /// Version of Kubernetes this cluster is running on
        /// </summary>
        /// <value>Version of Kubernetes this cluster is running on</value>
        [DataMember(Name = "version", EmitDefaultValue = false)]
        public string VarVersion { get; set; }

        /// <summary>
        /// Region this Kubernetes Cluster is running in
        /// </summary>
        /// <value>Region this Kubernetes Cluster is running in</value>
        [DataMember(Name = "region", EmitDefaultValue = false)]
        public string Region { get; set; }

        /// <summary>
        /// Status for VKE cluster
        /// </summary>
        /// <value>Status for VKE cluster</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; }

        /// <summary>
        /// Whether a highly available control planes configuration has been deployed * true * false (default)
        /// </summary>
        /// <value>Whether a highly available control planes configuration has been deployed * true * false (default)</value>
        [DataMember(Name = "ha_controlplanes", EmitDefaultValue = true)]
        public bool HaControlplanes { get; set; }

        /// <summary>
        /// NodePools in this cluster
        /// </summary>
        /// <value>NodePools in this cluster</value>
        [DataMember(Name = "node_pools", EmitDefaultValue = false)]
        public List<Nodepools> NodePools { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class VkeCluster {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  FirewallGroupId: ").Append(FirewallGroupId).Append("\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  DateCreated: ").Append(DateCreated).Append("\n");
            sb.Append("  ClusterSubnet: ").Append(ClusterSubnet).Append("\n");
            sb.Append("  ServiceSubnet: ").Append(ServiceSubnet).Append("\n");
            sb.Append("  Ip: ").Append(Ip).Append("\n");
            sb.Append("  Endpoint: ").Append(Endpoint).Append("\n");
            sb.Append("  VarVersion: ").Append(VarVersion).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  HaControlplanes: ").Append(HaControlplanes).Append("\n");
            sb.Append("  NodePools: ").Append(NodePools).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
