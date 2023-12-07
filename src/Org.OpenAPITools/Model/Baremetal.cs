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
    /// Bare Metal information.
    /// </summary>
    [DataContract(Name = "baremetal")]
    public partial class Baremetal : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Baremetal" /> class.
        /// </summary>
        /// <param name="id">A unique ID for the Bare Metal instance..</param>
        /// <param name="os">The [Operating System name](#operation/list-os)..</param>
        /// <param name="ram">Text description of the instances&#39; RAM..</param>
        /// <param name="disk">Text description of the instances&#39; disk configuration..</param>
        /// <param name="mainIp">The main IPv4 address..</param>
        /// <param name="cpuCount">Number of CPUs..</param>
        /// <param name="region">The [Region id](#operation/list-regions) where the instance is located..</param>
        /// <param name="defaultPassword">The default password assigned at deployment. Only available for ten minutes after deployment..</param>
        /// <param name="dateCreated">The date this instance was created..</param>
        /// <param name="status">The current status.  * active * pending * suspended.</param>
        /// <param name="netmaskV4">The IPv4 netmask in dot-decimal notation..</param>
        /// <param name="gatewayV4">The IPv4 gateway address..</param>
        /// <param name="plan">The [Bare Metal Plan id](#operation/list-metal-plans) used by this instance..</param>
        /// <param name="label">The user-supplied label for this instance..</param>
        /// <param name="tag">Use &#x60;tags&#x60; instead. The user-supplied tag for this instance..</param>
        /// <param name="osId">The [Operating System id](#operation/list-os)..</param>
        /// <param name="appId">The [Application id](#operation/list-applications)..</param>
        /// <param name="imageId">The [Application image_id](#operation/list-applications)..</param>
        /// <param name="v6Network">The IPv6 network size in bits..</param>
        /// <param name="v6MainIp">The main IPv6 network address..</param>
        /// <param name="v6NetworkSize">The IPv6 subnet..</param>
        /// <param name="macAddress">The MAC address for a Bare Metal server..</param>
        /// <param name="tags">Tags to apply to the instance..</param>
        /// <param name="userScheme">The user scheme.  * root * limited.</param>
        public Baremetal(string id = default(string), string os = default(string), string ram = default(string), string disk = default(string), string mainIp = default(string), int cpuCount = default(int), string region = default(string), string defaultPassword = default(string), string dateCreated = default(string), string status = default(string), string netmaskV4 = default(string), string gatewayV4 = default(string), string plan = default(string), string label = default(string), string tag = default(string), int osId = default(int), int appId = default(int), string imageId = default(string), string v6Network = default(string), string v6MainIp = default(string), int v6NetworkSize = default(int), int macAddress = default(int), List<string> tags = default(List<string>), string userScheme = default(string))
        {
            this.Id = id;
            this.Os = os;
            this.Ram = ram;
            this.Disk = disk;
            this.MainIp = mainIp;
            this.CpuCount = cpuCount;
            this.Region = region;
            this.DefaultPassword = defaultPassword;
            this.DateCreated = dateCreated;
            this.Status = status;
            this.NetmaskV4 = netmaskV4;
            this.GatewayV4 = gatewayV4;
            this.Plan = plan;
            this.Label = label;
            this.Tag = tag;
            this.OsId = osId;
            this.AppId = appId;
            this.ImageId = imageId;
            this.V6Network = v6Network;
            this.V6MainIp = v6MainIp;
            this.V6NetworkSize = v6NetworkSize;
            this.MacAddress = macAddress;
            this.Tags = tags;
            this.UserScheme = userScheme;
        }

        /// <summary>
        /// A unique ID for the Bare Metal instance.
        /// </summary>
        /// <value>A unique ID for the Bare Metal instance.</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// The [Operating System name](#operation/list-os).
        /// </summary>
        /// <value>The [Operating System name](#operation/list-os).</value>
        [DataMember(Name = "os", EmitDefaultValue = false)]
        public string Os { get; set; }

        /// <summary>
        /// Text description of the instances&#39; RAM.
        /// </summary>
        /// <value>Text description of the instances&#39; RAM.</value>
        [DataMember(Name = "ram", EmitDefaultValue = false)]
        public string Ram { get; set; }

        /// <summary>
        /// Text description of the instances&#39; disk configuration.
        /// </summary>
        /// <value>Text description of the instances&#39; disk configuration.</value>
        [DataMember(Name = "disk", EmitDefaultValue = false)]
        public string Disk { get; set; }

        /// <summary>
        /// The main IPv4 address.
        /// </summary>
        /// <value>The main IPv4 address.</value>
        [DataMember(Name = "main_ip", EmitDefaultValue = false)]
        public string MainIp { get; set; }

        /// <summary>
        /// Number of CPUs.
        /// </summary>
        /// <value>Number of CPUs.</value>
        [DataMember(Name = "cpu_count", EmitDefaultValue = false)]
        public int CpuCount { get; set; }

        /// <summary>
        /// The [Region id](#operation/list-regions) where the instance is located.
        /// </summary>
        /// <value>The [Region id](#operation/list-regions) where the instance is located.</value>
        [DataMember(Name = "region", EmitDefaultValue = false)]
        public string Region { get; set; }

        /// <summary>
        /// The default password assigned at deployment. Only available for ten minutes after deployment.
        /// </summary>
        /// <value>The default password assigned at deployment. Only available for ten minutes after deployment.</value>
        [DataMember(Name = "default_password", EmitDefaultValue = false)]
        public string DefaultPassword { get; set; }

        /// <summary>
        /// The date this instance was created.
        /// </summary>
        /// <value>The date this instance was created.</value>
        [DataMember(Name = "date_created", EmitDefaultValue = false)]
        public string DateCreated { get; set; }

        /// <summary>
        /// The current status.  * active * pending * suspended
        /// </summary>
        /// <value>The current status.  * active * pending * suspended</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; }

        /// <summary>
        /// The IPv4 netmask in dot-decimal notation.
        /// </summary>
        /// <value>The IPv4 netmask in dot-decimal notation.</value>
        [DataMember(Name = "netmask_v4", EmitDefaultValue = false)]
        public string NetmaskV4 { get; set; }

        /// <summary>
        /// The IPv4 gateway address.
        /// </summary>
        /// <value>The IPv4 gateway address.</value>
        [DataMember(Name = "gateway_v4", EmitDefaultValue = false)]
        public string GatewayV4 { get; set; }

        /// <summary>
        /// The [Bare Metal Plan id](#operation/list-metal-plans) used by this instance.
        /// </summary>
        /// <value>The [Bare Metal Plan id](#operation/list-metal-plans) used by this instance.</value>
        [DataMember(Name = "plan", EmitDefaultValue = false)]
        public string Plan { get; set; }

        /// <summary>
        /// The user-supplied label for this instance.
        /// </summary>
        /// <value>The user-supplied label for this instance.</value>
        [DataMember(Name = "label", EmitDefaultValue = false)]
        public string Label { get; set; }

        /// <summary>
        /// Use &#x60;tags&#x60; instead. The user-supplied tag for this instance.
        /// </summary>
        /// <value>Use &#x60;tags&#x60; instead. The user-supplied tag for this instance.</value>
        [DataMember(Name = "tag", EmitDefaultValue = false)]
        [Obsolete]
        public string Tag { get; set; }

        /// <summary>
        /// The [Operating System id](#operation/list-os).
        /// </summary>
        /// <value>The [Operating System id](#operation/list-os).</value>
        [DataMember(Name = "os_id", EmitDefaultValue = false)]
        public int OsId { get; set; }

        /// <summary>
        /// The [Application id](#operation/list-applications).
        /// </summary>
        /// <value>The [Application id](#operation/list-applications).</value>
        [DataMember(Name = "app_id", EmitDefaultValue = false)]
        public int AppId { get; set; }

        /// <summary>
        /// The [Application image_id](#operation/list-applications).
        /// </summary>
        /// <value>The [Application image_id](#operation/list-applications).</value>
        [DataMember(Name = "image_id", EmitDefaultValue = false)]
        public string ImageId { get; set; }

        /// <summary>
        /// The IPv6 network size in bits.
        /// </summary>
        /// <value>The IPv6 network size in bits.</value>
        [DataMember(Name = "v6_network", EmitDefaultValue = false)]
        public string V6Network { get; set; }

        /// <summary>
        /// The main IPv6 network address.
        /// </summary>
        /// <value>The main IPv6 network address.</value>
        [DataMember(Name = "v6_main_ip", EmitDefaultValue = false)]
        public string V6MainIp { get; set; }

        /// <summary>
        /// The IPv6 subnet.
        /// </summary>
        /// <value>The IPv6 subnet.</value>
        [DataMember(Name = "v6_network_size", EmitDefaultValue = false)]
        public int V6NetworkSize { get; set; }

        /// <summary>
        /// The MAC address for a Bare Metal server.
        /// </summary>
        /// <value>The MAC address for a Bare Metal server.</value>
        [DataMember(Name = "mac_address", EmitDefaultValue = false)]
        public int MacAddress { get; set; }

        /// <summary>
        /// Tags to apply to the instance.
        /// </summary>
        /// <value>Tags to apply to the instance.</value>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// The user scheme.  * root * limited
        /// </summary>
        /// <value>The user scheme.  * root * limited</value>
        [DataMember(Name = "user_scheme", EmitDefaultValue = false)]
        public string UserScheme { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class Baremetal {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Os: ").Append(Os).Append("\n");
            sb.Append("  Ram: ").Append(Ram).Append("\n");
            sb.Append("  Disk: ").Append(Disk).Append("\n");
            sb.Append("  MainIp: ").Append(MainIp).Append("\n");
            sb.Append("  CpuCount: ").Append(CpuCount).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  DefaultPassword: ").Append(DefaultPassword).Append("\n");
            sb.Append("  DateCreated: ").Append(DateCreated).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  NetmaskV4: ").Append(NetmaskV4).Append("\n");
            sb.Append("  GatewayV4: ").Append(GatewayV4).Append("\n");
            sb.Append("  Plan: ").Append(Plan).Append("\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  Tag: ").Append(Tag).Append("\n");
            sb.Append("  OsId: ").Append(OsId).Append("\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  ImageId: ").Append(ImageId).Append("\n");
            sb.Append("  V6Network: ").Append(V6Network).Append("\n");
            sb.Append("  V6MainIp: ").Append(V6MainIp).Append("\n");
            sb.Append("  V6NetworkSize: ").Append(V6NetworkSize).Append("\n");
            sb.Append("  MacAddress: ").Append(MacAddress).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  UserScheme: ").Append(UserScheme).Append("\n");
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
