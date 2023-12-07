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
    /// NodePool
    /// </summary>
    [DataContract(Name = "nodepools")]
    public partial class Nodepools : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Nodepools" /> class.
        /// </summary>
        /// <param name="id">The [NodePool ID](#operation/get-nodepools)..</param>
        /// <param name="dateCreated">Date of creation.</param>
        /// <param name="label">Label for nodepool.</param>
        /// <param name="tag">Tag for node pool.</param>
        /// <param name="plan">Plan used for nodepool.</param>
        /// <param name="status">Status for nodepool.</param>
        /// <param name="nodeQuantity">Number of nodes in nodepool.</param>
        /// <param name="nodes">nodes.</param>
        /// <param name="dateUpdated">Date the nodepool was updated..</param>
        /// <param name="autoScaler">Displays if the auto scaler is enabled or disabled for your cluster..</param>
        /// <param name="minNodes">Auto scaler field that displays the minimum nodes you want for your cluster..</param>
        /// <param name="maxNodes">Auto scaler field that displays the maximum nodes you want for your cluster..</param>
        public Nodepools(string id = default(string), string dateCreated = default(string), string label = default(string), string tag = default(string), string plan = default(string), string status = default(string), int nodeQuantity = default(int), List<NodepoolInstances> nodes = default(List<NodepoolInstances>), string dateUpdated = default(string), bool autoScaler = default(bool), int minNodes = default(int), int maxNodes = default(int))
        {
            this.Id = id;
            this.DateCreated = dateCreated;
            this.Label = label;
            this.Tag = tag;
            this.Plan = plan;
            this.Status = status;
            this.NodeQuantity = nodeQuantity;
            this.Nodes = nodes;
            this.DateUpdated = dateUpdated;
            this.AutoScaler = autoScaler;
            this.MinNodes = minNodes;
            this.MaxNodes = maxNodes;
        }

        /// <summary>
        /// The [NodePool ID](#operation/get-nodepools).
        /// </summary>
        /// <value>The [NodePool ID](#operation/get-nodepools).</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        /// <value>Date of creation</value>
        [DataMember(Name = "date_created", EmitDefaultValue = false)]
        public string DateCreated { get; set; }

        /// <summary>
        /// Label for nodepool
        /// </summary>
        /// <value>Label for nodepool</value>
        [DataMember(Name = "label", EmitDefaultValue = false)]
        public string Label { get; set; }

        /// <summary>
        /// Tag for node pool
        /// </summary>
        /// <value>Tag for node pool</value>
        [DataMember(Name = "tag", EmitDefaultValue = false)]
        public string Tag { get; set; }

        /// <summary>
        /// Plan used for nodepool
        /// </summary>
        /// <value>Plan used for nodepool</value>
        [DataMember(Name = "plan", EmitDefaultValue = false)]
        public string Plan { get; set; }

        /// <summary>
        /// Status for nodepool
        /// </summary>
        /// <value>Status for nodepool</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; }

        /// <summary>
        /// Number of nodes in nodepool
        /// </summary>
        /// <value>Number of nodes in nodepool</value>
        [DataMember(Name = "node_quantity", EmitDefaultValue = false)]
        public int NodeQuantity { get; set; }

        /// <summary>
        /// Gets or Sets Nodes
        /// </summary>
        [DataMember(Name = "nodes", EmitDefaultValue = false)]
        public List<NodepoolInstances> Nodes { get; set; }

        /// <summary>
        /// Date the nodepool was updated.
        /// </summary>
        /// <value>Date the nodepool was updated.</value>
        [DataMember(Name = "date_updated", EmitDefaultValue = false)]
        public string DateUpdated { get; set; }

        /// <summary>
        /// Displays if the auto scaler is enabled or disabled for your cluster.
        /// </summary>
        /// <value>Displays if the auto scaler is enabled or disabled for your cluster.</value>
        [DataMember(Name = "auto_scaler", EmitDefaultValue = true)]
        public bool AutoScaler { get; set; }

        /// <summary>
        /// Auto scaler field that displays the minimum nodes you want for your cluster.
        /// </summary>
        /// <value>Auto scaler field that displays the minimum nodes you want for your cluster.</value>
        [DataMember(Name = "min_nodes", EmitDefaultValue = false)]
        public int MinNodes { get; set; }

        /// <summary>
        /// Auto scaler field that displays the maximum nodes you want for your cluster.
        /// </summary>
        /// <value>Auto scaler field that displays the maximum nodes you want for your cluster.</value>
        [DataMember(Name = "max_nodes", EmitDefaultValue = false)]
        public int MaxNodes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class Nodepools {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  DateCreated: ").Append(DateCreated).Append("\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  Tag: ").Append(Tag).Append("\n");
            sb.Append("  Plan: ").Append(Plan).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  NodeQuantity: ").Append(NodeQuantity).Append("\n");
            sb.Append("  Nodes: ").Append(Nodes).Append("\n");
            sb.Append("  DateUpdated: ").Append(DateUpdated).Append("\n");
            sb.Append("  AutoScaler: ").Append(AutoScaler).Append("\n");
            sb.Append("  MinNodes: ").Append(MinNodes).Append("\n");
            sb.Append("  MaxNodes: ").Append(MaxNodes).Append("\n");
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
