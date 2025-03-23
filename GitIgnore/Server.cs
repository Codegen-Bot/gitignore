using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GitIgnore;

public partial class Mutation
{
    public JsonNode Resolve(
        IReadOnlyDictionary<string, object?> variables,
        IEnumerable<IGraphQLSelection> selections
    )
    {
        var jsonNode = new JsonObject();

        foreach (var selection in selections)
        {
            if (selection is GraphQLFieldSelection fieldSelection)
            {
                if (fieldSelection.Name == "addIgnorePattern")
                {
                    string? folder = null;
                    string? pattern = null;

                    foreach (var arg in fieldSelection.Arguments)
                    {
                        if (arg.Name == "folder")
                        {
                            if (arg.Value is GraphQLStringValue graphqlString)
                            {
                                folder = graphqlString.Value;
                            }
                            else if (arg.Value is GraphQLNullValue)
                            {
                                folder = null;
                            }
                            else if (arg.Value is GraphQLVariableValue graphqlVariable)
                            {
                                if (!variables.TryGetValue(graphqlVariable.Name, out var result))
                                {
                                    throw new ArgumentException(
                                        $"{graphqlVariable.Name} is not specified"
                                    );
                                }
                                folder = (string?)result;
                            }
                        }
                        if (arg.Name == "pattern")
                        {
                            if (arg.Value is GraphQLStringValue graphqlString)
                            {
                                pattern = graphqlString.Value;
                            }
                            else if (arg.Value is GraphQLVariableValue graphqlVariable)
                            {
                                if (!variables.TryGetValue(graphqlVariable.Name, out var result))
                                {
                                    throw new ArgumentException(
                                        $"{graphqlVariable.Name} is not specified"
                                    );
                                }
                                if (result is null)
                                {
                                    throw new ArgumentNullException("pattern is null");
                                }
                                pattern = (string)result;
                            }
                        }
                    }
                    jsonNode[fieldSelection.Alias ?? fieldSelection.Name] = GetAddIgnorePattern(
                        folder,
                        pattern
                    );
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        return jsonNode;
    }

    public partial string GetAddIgnorePattern(string? folder, string pattern);
}

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
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
        Justification = "<Pending>"
    )]
    public string ProcessGraphQLRequest(string request)
    {
        var parsedRequest = GraphQLRequest.FromJsonString(request);

        JsonNode? jsonNode = null;
        var errors = new JsonArray();

        if (parsedRequest.Query.Operation.OperationType == GraphQLOperationType.Mutation)
        {
            var obj = new Mutation();
            jsonNode = obj.Resolve(
                parsedRequest.Variables,
                parsedRequest.Query.Operation.Selections
            );
        }

        if (jsonNode is null)
        {
            errors.Add("No result");
        }

        var result = new JsonObject();
        result["errors"] = errors;
        result["data"] = jsonNode;

        var resultJson = result.ToJsonString();
        return resultJson;
    }
}
