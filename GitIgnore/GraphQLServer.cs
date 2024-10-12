using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using CodegenBot;

namespace GitIgnore;

[JsonSerializable(typeof(GraphQLServerErrorResponse))]
[JsonSerializable(typeof(GraphQLServerRequest))]
public partial class GraphQLServerJsonSerializerContext : JsonSerializerContext { }

public partial class GraphQLServer
{
    public string Execute(string requestBody)
    {
        var request = GraphQLServerRequest.FromJsonString(requestBody);

        if (request is null)
        {
            return JsonSerializer.Serialize(
                new GraphQLServerErrorResponse()
                {
                    Errors = ["Could not parse GraphQL request JSON"],
                },
                GraphQLServerJsonSerializerContext.Default.GraphQLServerErrorResponse
            );
        }

        var query = GraphQLClient
            .ParseGraphQLOperation(
                [new AdditionalFileInput() { FilePath = "tmp.graphql", Content = request.Query }]
            )
            .GraphQL.Operations.SingleOrDefault();

        if (query is null)
        {
            return JsonSerializer.Serialize(
                new GraphQLServerErrorResponse()
                {
                    Errors = ["Could not parse GraphQL request query"],
                },
                GraphQLServerJsonSerializerContext.Default.GraphQLServerErrorResponse
            );
        }

        var result = new JsonObject();

        if (query.OperationType == GraphQLOperationType.MUTATION)
        {
            Mutation.AddSelectedFields(
                query.Variables,
                query.NestedSelection.ToSelections(),
                result
            );
        }
        else
        {
            Imports.Log(
                new LogEvent()
                {
                    Level = LogEventLevel.Critical,
                    Message = "Invalid operation type {OperationType}",
                    Args = [query.OperationType.ToString()],
                }
            );
        }

        var resultString = result.ToJsonString();
        return resultString;
    }

    public Mutation Mutation { get; } = new();
}

public class GraphQLServerErrorResponse
{
    public required List<string> Errors { get; set; }
}

public class GraphQLServerRequest
{
    [JsonPropertyName("query")]
    public required string Query { get; set; }

    [JsonPropertyName("operationName")]
    public string? OperationName { get; set; }

    [JsonPropertyName("variables")]
    public Dictionary<string, object?>? Variables { get; set; }

    public static GraphQLServerRequest? FromJsonString(string requestBody)
    {
        return JsonSerializer.Deserialize(
            requestBody,
            GraphQLServerJsonSerializerContext.Default.GraphQLServerRequest
        );
    }

    public string ToJsonString()
    {
        return JsonSerializer.Serialize(
            this,
            GraphQLServerJsonSerializerContext.Default.GraphQLServerRequest
        );
    }
}

public partial class ParseGraphQLOperationOperationNestedSelection
    : INestedSelection<
        ParseGraphQLOperationOperationNestedSelectionFieldSelection,
        ParseGraphQLOperationOperationNestedSelectionFragmentSpreadSelection
    > { }

public partial class Mutation
{
    public void AddSelectedFields(
        IReadOnlyList<ParseGraphQLOperationOperationVariable> variables,
        IReadOnlyList<ParseGraphQLOperationOperationNestedSelection> selection,
        JsonObject result
    ) { }

    public string GetAddIgnorePattern(string? directory, string pattern) { }
}
