using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GitIgnore;

public class GraphQLRequest
{
    [JsonPropertyName("query")]
    public required ParsedGraphQLOperation Query { get; set; }

    [JsonPropertyName("operationName")]
    public string? OperationName { get; set; }

    [JsonPropertyName("variables")]
    public Dictionary<string, object>? Variables { get; set; }

    public static GraphQLRequest? FromJsonString(string requestBody)
    {
        return JsonSerializer.Deserialize(
            requestBody,
            GraphQLServerJsonSerializerContext.Default.GraphQLRequest
        );
    }

    public string ToJsonString()
    {
        return JsonSerializer.Serialize(
            this,
            GraphQLServerJsonSerializerContext.Default.GraphQLRequest
        );
    }
}

[JsonSerializable(typeof(GraphQLRequest))]
public partial class GraphQLServerJsonSerializerContext : JsonSerializerContext;

public class ParsedGraphQLOperation
{
    public required GraphQLOperation Operation { get; set; }
    public List<GraphQLFragment> Fragments { get; set; } = new();
}

public partial class GraphQLServer
{
    public string ProcessGraphQLRequest(string request)
    {
        var parsedRequest = GraphQLRequest.FromJsonString(request);

        JsonNode? jsonNode = null;
        var errors = new JsonArray();

        if (jsonNode is null)
        {
            errors.Add("No result");
        }

        var result = new JsonObject();
        result["errors"] = errors;
        result["data"] = jsonNode;

        var resultJson = result?.ToJsonString();
        return resultJson;
    }
}
