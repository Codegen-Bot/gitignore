using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CodegenBot;

namespace GitIgnore;

public partial class GraphQLResponse<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("errors")]
    public List<GraphQLError>? Errors { get; set; }
}

public partial class GraphQLError
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}

[JsonSerializable(typeof(GraphQLError))]
[JsonSerializable(typeof(FileKind))]
[JsonSerializable(typeof(FileVersion))]
[JsonSerializable(typeof(GraphQLOperationType))]
[JsonSerializable(typeof(LogSeverity))]
[JsonSerializable(typeof(AdditionalFileInput))]
[JsonSerializable(typeof(CaretTagInput))]
[JsonSerializable(typeof(AddFileVariables))]
[JsonSerializable(typeof(AddFileData))]
[JsonSerializable(typeof(GraphQLResponse<AddFileData>))]
[JsonSerializable(typeof(GraphQLRequest<AddFileVariables>))]
[JsonSerializable(typeof(AddFile))]
[JsonSerializable(typeof(AddKeyedTextVariables))]
[JsonSerializable(typeof(AddKeyedTextData))]
[JsonSerializable(typeof(GraphQLResponse<AddKeyedTextData>))]
[JsonSerializable(typeof(GraphQLRequest<AddKeyedTextVariables>))]
[JsonSerializable(typeof(AddKeyedText))]
[JsonSerializable(typeof(AddKeyedTextByTagsVariables))]
[JsonSerializable(typeof(AddKeyedTextByTagsData))]
[JsonSerializable(typeof(GraphQLResponse<AddKeyedTextByTagsData>))]
[JsonSerializable(typeof(GraphQLRequest<AddKeyedTextByTagsVariables>))]
[JsonSerializable(typeof(AddKeyedTextByTags))]
[JsonSerializable(typeof(AddTextVariables))]
[JsonSerializable(typeof(AddTextData))]
[JsonSerializable(typeof(GraphQLResponse<AddTextData>))]
[JsonSerializable(typeof(GraphQLRequest<AddTextVariables>))]
[JsonSerializable(typeof(AddText))]
[JsonSerializable(typeof(AddTextByTagsVariables))]
[JsonSerializable(typeof(AddTextByTagsData))]
[JsonSerializable(typeof(GraphQLResponse<AddTextByTagsData>))]
[JsonSerializable(typeof(GraphQLRequest<AddTextByTagsVariables>))]
[JsonSerializable(typeof(AddTextByTags))]
[JsonSerializable(typeof(GetFilesVariables))]
[JsonSerializable(typeof(GetFilesData))]
[JsonSerializable(typeof(GraphQLResponse<GetFilesData>))]
[JsonSerializable(typeof(GraphQLRequest<GetFilesVariables>))]
[JsonSerializable(typeof(GetFiles))]
[JsonSerializable(typeof(LogVariables))]
[JsonSerializable(typeof(LogData))]
[JsonSerializable(typeof(GraphQLResponse<LogData>))]
[JsonSerializable(typeof(GraphQLRequest<LogVariables>))]
[JsonSerializable(typeof(ParseGraphQLOperationVariables))]
[JsonSerializable(typeof(ParseGraphQLOperationData))]
[JsonSerializable(typeof(GraphQLResponse<ParseGraphQLOperationData>))]
[JsonSerializable(typeof(GraphQLRequest<ParseGraphQLOperationVariables>))]
[JsonSerializable(typeof(ParseGraphQLOperation))]
[JsonSerializable(typeof(ParseGraphQLOperationOperation))]
[JsonSerializable(typeof(ParseGraphQLOperationOperationVariable))]
[JsonSerializable(typeof(ParseGraphQLOperationOperationVariableType))]
[JsonSerializable(typeof(ParseGraphQLOperationOperationNestedSelection))]
[JsonSerializable(typeof(ParseGraphQLOperationOperationNestedSelectionFieldSelection))]
[JsonSerializable(typeof(ParseGraphQLOperationOperationNestedSelectionFieldSelectionArgument))]
[JsonSerializable(typeof(ParseGraphQLOperationOperationNestedSelectionFragmentSpreadSelection))]
[JsonSerializable(typeof(ReadTextFileVariables))]
[JsonSerializable(typeof(ReadTextFileData))]
[JsonSerializable(typeof(GraphQLResponse<ReadTextFileData>))]
[JsonSerializable(typeof(GraphQLRequest<ReadTextFileVariables>))]
public partial class GraphQLClientJsonSerializerContext : JsonSerializerContext { }

public static partial class GraphQLClient
{
    public static AddFileData AddFile(string filePath, string textAndCarets)
    {
        var request = new GraphQLRequest<AddFileVariables>
        {
            Query = """
                mutation AddFile($filePath: String!, $textAndCarets: String!) {
                  addFile(filePath: $filePath, textAndCarets: $textAndCarets) {
                    id
                  }
                }
                """,
            OperationName = "AddFile",
            Variables = new AddFileVariables()
            {
                FilePath = filePath,
                TextAndCarets = textAndCarets,
            },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestAddFileVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddFileData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseAddFileData
        );
        return result?.Data
            ?? throw new InvalidOperationException("Received null data for request AddFile.");
    }

    public static AddKeyedTextData AddKeyedText(string key, string caretId, string textAndCarets)
    {
        var request = new GraphQLRequest<AddKeyedTextVariables>
        {
            Query = """
                mutation AddKeyedText($key: String!, $caretId: String!, $textAndCarets: String!) {
                  addKeyedText(key: $key, caretId: $caretId, textAndCarets: $textAndCarets) {
                    id
                  }
                }
                """,
            OperationName = "AddKeyedText",
            Variables = new AddKeyedTextVariables()
            {
                Key = key,
                CaretId = caretId,
                TextAndCarets = textAndCarets,
            },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestAddKeyedTextVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddKeyedTextData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseAddKeyedTextData
        );
        return result?.Data
            ?? throw new InvalidOperationException("Received null data for request AddKeyedText.");
    }

    public static AddKeyedTextByTagsData AddKeyedTextByTags(
        string key,
        List<CaretTagInput> tags,
        string textAndCarets
    )
    {
        var request = new GraphQLRequest<AddKeyedTextByTagsVariables>
        {
            Query = """
                mutation AddKeyedTextByTags($key: String!, $tags: [CaretTagInput!]!, $textAndCarets: String!) {
                  addKeyedTextByTags(key: $key, tags: $tags, textAndCarets: $textAndCarets) {
                    id
                  }
                }
                """,
            OperationName = "AddKeyedTextByTags",
            Variables = new AddKeyedTextByTagsVariables()
            {
                Key = key,
                Tags = tags,
                TextAndCarets = textAndCarets,
            },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestAddKeyedTextByTagsVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddKeyedTextByTagsData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseAddKeyedTextByTagsData
        );
        return result?.Data
            ?? throw new InvalidOperationException(
                "Received null data for request AddKeyedTextByTags."
            );
    }

    public static AddTextData AddText(string caretId, string textAndCarets)
    {
        var request = new GraphQLRequest<AddTextVariables>
        {
            Query = """
                mutation AddText($caretId: String!, $textAndCarets: String!) {
                  addText(caretId: $caretId, textAndCarets: $textAndCarets) {
                    id
                  }
                }
                """,
            OperationName = "AddText",
            Variables = new AddTextVariables() { CaretId = caretId, TextAndCarets = textAndCarets },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestAddTextVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddTextData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseAddTextData
        );
        return result?.Data
            ?? throw new InvalidOperationException("Received null data for request AddText.");
    }

    public static AddTextByTagsData AddTextByTags(List<CaretTagInput> tags, string textAndCarets)
    {
        var request = new GraphQLRequest<AddTextByTagsVariables>
        {
            Query = """
                mutation AddTextByTags($tags: [CaretTagInput!]!, $textAndCarets: String!) {
                  addTextByTags(tags: $tags, textAndCarets: $textAndCarets) {
                    id
                  }
                }
                """,
            OperationName = "AddTextByTags",
            Variables = new AddTextByTagsVariables() { Tags = tags, TextAndCarets = textAndCarets },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestAddTextByTagsVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddTextByTagsData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseAddTextByTagsData
        );
        return result?.Data
            ?? throw new InvalidOperationException("Received null data for request AddTextByTags.");
    }

    public static GetFilesData GetFiles(List<string> whitelist, List<string> blacklist)
    {
        var request = new GraphQLRequest<GetFilesVariables>
        {
            Query = """
                query GetFiles($whitelist: [String!]!, $blacklist: [String!]!) {
                  files(whitelist: $whitelist, blacklist: $blacklist) {
                    path
                    kind
                  }
                }
                """,
            OperationName = "GetFiles",
            Variables = new GetFilesVariables() { Whitelist = whitelist, Blacklist = blacklist },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestGetFilesVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<GetFilesData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseGetFilesData
        );
        return result?.Data
            ?? throw new InvalidOperationException("Received null data for request GetFiles.");
    }

    public static LogData Log(LogSeverity severity, string message, List<string>? arguments)
    {
        var request = new GraphQLRequest<LogVariables>
        {
            Query = """
                mutation Log($severity: LogSeverity!, $message: String!, $arguments: [String!]) {
                  log(severity: $severity, message: $message, arguments: $arguments)
                }
                """,
            OperationName = "Log",
            Variables = new LogVariables()
            {
                Severity = severity,
                Message = message,
                Arguments = arguments,
            },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestLogVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<LogData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseLogData
        );
        return result?.Data
            ?? throw new InvalidOperationException("Received null data for request Log.");
    }

    public static ParseGraphQLOperationData ParseGraphQLOperation(List<AdditionalFileInput> graphql)
    {
        var request = new GraphQLRequest<ParseGraphQLOperationVariables>
        {
            Query = """
                query ParseGraphQLOperation($graphql: [AdditionalFileInput!]!) {
                  graphQL(additionalFiles: $graphql) {
                    operations {
                      name
                      operationType
                      text
                      variables {
                        name
                        type {
                          text
                        }
                      }
                      nestedSelection {
                        depth
                        fieldSelection {
                          name
                          alias
                          arguments {
                            name
                            type {
                              text
                            }
                          }
                        }
                        fragmentSpreadSelection {
                          name
                        }
                      }
                    }
                  }
                }
                """,
            OperationName = "ParseGraphQLOperation",
            Variables = new ParseGraphQLOperationVariables() { Graphql = graphql },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestParseGraphQLOperationVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<ParseGraphQLOperationData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseParseGraphQLOperationData
        );
        return result?.Data
            ?? throw new InvalidOperationException(
                "Received null data for request ParseGraphQLOperation."
            );
    }

    public static ReadTextFileData ReadTextFile(string textFilePath)
    {
        var request = new GraphQLRequest<ReadTextFileVariables>
        {
            Query = """
                query ReadTextFile($textFilePath: String!) {
                  readTextFile(textFilePath: $textFilePath)
                }
                """,
            OperationName = "ReadTextFile",
            Variables = new ReadTextFileVariables() { TextFilePath = textFilePath },
        };

        var response = Imports.GraphQL(
            request,
            GraphQLClientJsonSerializerContext.Default.GraphQLRequestReadTextFileVariables
        );
        var result = JsonSerializer.Deserialize<GraphQLResponse<ReadTextFileData>>(
            response,
            GraphQLClientJsonSerializerContext.Default.GraphQLResponseReadTextFileData
        );
        return result?.Data
            ?? throw new InvalidOperationException("Received null data for request ReadTextFile.");
    }
}

[JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumMemberConverter))]
public enum FileKind
{
    [EnumMember(Value = "BINARY")]
    BINARY,

    [EnumMember(Value = "TEXT")]
    TEXT,
}

[JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumMemberConverter))]
public enum FileVersion
{
    [EnumMember(Value = "GENERATED")]
    GENERATED,

    [EnumMember(Value = "HEAD")]
    HEAD,
}

[JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumMemberConverter))]
public enum GraphQLOperationType
{
    [EnumMember(Value = "QUERY")]
    QUERY,

    [EnumMember(Value = "MUTATION")]
    MUTATION,

    [EnumMember(Value = "SUBSCRIPTION")]
    SUBSCRIPTION,
}

[JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumMemberConverter))]
public enum LogSeverity
{
    [EnumMember(Value = "TRACE")]
    TRACE,

    [EnumMember(Value = "DEBUG")]
    DEBUG,

    [EnumMember(Value = "INFORMATION")]
    INFORMATION,

    [EnumMember(Value = "WARNING")]
    WARNING,

    [EnumMember(Value = "ERROR")]
    ERROR,

    [EnumMember(Value = "CRITICAL")]
    CRITICAL,
}

public partial class AdditionalFileInput
{
    [JsonPropertyName("filePath")]
    public required string FilePath { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }
}

public partial class CaretTagInput
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("value")]
    public required string Value { get; set; }
}

public partial class AddFileData
{
    [JsonPropertyName("addFile")]
    public required AddFile AddFile { get; set; }
}

public partial class AddFileVariables
{
    [JsonPropertyName("filePath")]
    public required string FilePath { get; set; }

    [JsonPropertyName("textAndCarets")]
    public required string TextAndCarets { get; set; }
}

public partial class AddFile
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}

public partial class AddKeyedTextData
{
    [JsonPropertyName("addKeyedText")]
    public required AddKeyedText AddKeyedText { get; set; }
}

public partial class AddKeyedTextVariables
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("caretId")]
    public required string CaretId { get; set; }

    [JsonPropertyName("textAndCarets")]
    public required string TextAndCarets { get; set; }
}

public partial class AddKeyedText
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}

public partial class AddKeyedTextByTagsData
{
    [JsonPropertyName("addKeyedTextByTags")]
    public required List<AddKeyedTextByTags> AddKeyedTextByTags { get; set; }
}

public partial class AddKeyedTextByTagsVariables
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("tags")]
    public required List<CaretTagInput> Tags { get; set; }

    [JsonPropertyName("textAndCarets")]
    public required string TextAndCarets { get; set; }
}

public partial class AddKeyedTextByTags
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}

public partial class AddTextData
{
    [JsonPropertyName("addText")]
    public required AddText AddText { get; set; }
}

public partial class AddTextVariables
{
    [JsonPropertyName("caretId")]
    public required string CaretId { get; set; }

    [JsonPropertyName("textAndCarets")]
    public required string TextAndCarets { get; set; }
}

public partial class AddText
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}

public partial class AddTextByTagsData
{
    [JsonPropertyName("addTextByTags")]
    public required List<AddTextByTags> AddTextByTags { get; set; }
}

public partial class AddTextByTagsVariables
{
    [JsonPropertyName("tags")]
    public required List<CaretTagInput> Tags { get; set; }

    [JsonPropertyName("textAndCarets")]
    public required string TextAndCarets { get; set; }
}

public partial class AddTextByTags
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}

public partial class GetFilesData
{
    [JsonPropertyName("files")]
    public required List<GetFiles> Files { get; set; }
}

public partial class GetFilesVariables
{
    [JsonPropertyName("whitelist")]
    public required List<string> Whitelist { get; set; }

    [JsonPropertyName("blacklist")]
    public required List<string> Blacklist { get; set; }
}

public partial class GetFiles
{
    [JsonPropertyName("path")]
    public required string Path { get; set; }

    [JsonPropertyName("kind")]
    public required FileKind Kind { get; set; }
}

public partial class LogData
{
    [JsonPropertyName("log")]
    public required string Log { get; set; }
}

public partial class LogVariables
{
    [JsonPropertyName("severity")]
    public required LogSeverity Severity { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("arguments")]
    public List<string>? Arguments { get; set; }
}

public partial class ParseGraphQLOperationData
{
    [JsonPropertyName("graphQL")]
    public required ParseGraphQLOperation GraphQL { get; set; }
}

public partial class ParseGraphQLOperationVariables
{
    [JsonPropertyName("graphql")]
    public required List<AdditionalFileInput> Graphql { get; set; }
}

public partial class ParseGraphQLOperation
{
    [JsonPropertyName("operations")]
    public required List<ParseGraphQLOperationOperation> Operations { get; set; }
}

public partial class ParseGraphQLOperationOperation
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("operationType")]
    public required GraphQLOperationType OperationType { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }

    [JsonPropertyName("variables")]
    public required List<ParseGraphQLOperationOperationVariable> Variables { get; set; }

    [JsonPropertyName("nestedSelection")]
    public required List<ParseGraphQLOperationOperationNestedSelection> NestedSelection { get; set; }
}

public partial class ParseGraphQLOperationOperationVariable
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required ParseGraphQLOperationOperationVariableType Type { get; set; }
}

public partial class ParseGraphQLOperationOperationVariableType
{
    [JsonPropertyName("text")]
    public required string Text { get; set; }
}

public partial class ParseGraphQLOperationOperationNestedSelection
{
    [JsonPropertyName("depth")]
    public required int Depth { get; set; }

    [JsonPropertyName("fieldSelection")]
    public ParseGraphQLOperationOperationNestedSelectionFieldSelection? FieldSelection { get; set; }

    [JsonPropertyName("fragmentSpreadSelection")]
    public ParseGraphQLOperationOperationNestedSelectionFragmentSpreadSelection? FragmentSpreadSelection { get; set; }
}

public partial class ParseGraphQLOperationOperationNestedSelectionFieldSelection
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    [JsonPropertyName("arguments")]
    public required List<ParseGraphQLOperationOperationNestedSelectionFieldSelectionArgument> Arguments { get; set; }
}

public partial class ParseGraphQLOperationOperationNestedSelectionFieldSelectionArgument
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}

public partial class ParseGraphQLOperationOperationNestedSelectionFragmentSpreadSelection
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}

public partial class ReadTextFileData
{
    [JsonPropertyName("readTextFile")]
    public string? ReadTextFile { get; set; }
}

public partial class ReadTextFileVariables
{
    [JsonPropertyName("textFilePath")]
    public required string TextFilePath { get; set; }
}
