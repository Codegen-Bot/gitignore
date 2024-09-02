using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CodegenBot;

namespace GitIgnore;

public static partial class GraphQLOperations
{
    public static GetConfigurationData GetConfiguration()
    {
        var variables = new Dictionary<string, object?>();

        var request = new GraphQLRequest
        {
            Query =
                @"
            query GetConfiguration {
  configuration {
    outputPath
  }
},
            ",
            OperationName = "GetConfiguration",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<GetConfigurationData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }

    public static GetFilesData GetFiles(List<string>? whitelist, List<string>? blacklist)
    {
        var variables = new Dictionary<string, object?>();
        variables.Add("whitelist", whitelist);
        variables.Add("blacklist", blacklist);

        var request = new GraphQLRequest
        {
            Query =
                @"
            query GetFiles($whitelist: [String!]!, $blacklist: [String!]!) {
  files(whitelist: $whitelist, blacklist: $blacklist) {
    path
    kind
  }
},
            ",
            OperationName = "GetFiles",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<GetFilesData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }

    public static GetFileContentsData GetFileContents(string textFilePath)
    {
        var variables = new Dictionary<string, object?>();
        variables.Add("textFilePath", textFilePath);

        var request = new GraphQLRequest
        {
            Query =
                @"
            query GetFileContents($textFilePath: String!) {
  readTextFile(textFilePath: $textFilePath)
},
            ",
            OperationName = "GetFileContents",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<GetFileContentsData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }

    public static AddFileData AddFile(string filePath, string textAndCarets)
    {
        var variables = new Dictionary<string, object?>();
        variables.Add("filePath", filePath);
        variables.Add("textAndCarets", textAndCarets);

        var request = new GraphQLRequest
        {
            Query =
                @"
            mutation AddFile($filePath: String!, $textAndCarets: String!) {
  addFile(filePath: $filePath, textAndCarets: $textAndCarets) {
    id
  }
},
            ",
            OperationName = "AddFile",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddFileData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }

    public static AddTextData AddText(string caretId, string textAndCarets)
    {
        var variables = new Dictionary<string, object?>();
        variables.Add("caretId", caretId);
        variables.Add("textAndCarets", textAndCarets);

        var request = new GraphQLRequest
        {
            Query =
                @"
            mutation AddText($caretId: String!, $textAndCarets: String!) {
  addText(caretId: $caretId, textAndCarets: $textAndCarets) {
    id
  }
},
            ",
            OperationName = "AddText",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddTextData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }

    public static AddKeyedTextData AddKeyedText(string key, string caretId, string textAndCarets)
    {
        var variables = new Dictionary<string, object?>();
        variables.Add("key", key);
        variables.Add("caretId", caretId);
        variables.Add("textAndCarets", textAndCarets);

        var request = new GraphQLRequest
        {
            Query =
                @"
            mutation AddKeyedText($key: String!, $caretId: String!, $textAndCarets: String!) {
  addKeyedText(key: $key, caretId: $caretId, textAndCarets: $textAndCarets) {
    id
  }
},
            ",
            OperationName = "AddKeyedText",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddKeyedTextData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }

    public static AddTextByTagsData AddTextByTags(List<CaretTagInput>? tags, string textAndCarets)
    {
        var variables = new Dictionary<string, object?>();
        variables.Add("tags", tags);
        variables.Add("textAndCarets", textAndCarets);

        var request = new GraphQLRequest
        {
            Query =
                @"
            mutation AddTextByTags($tags: [CaretTagInput!]!, $textAndCarets: String!) {
  addTextByTags(tags: $tags, textAndCarets: $textAndCarets) {
    id
  }
},
            ",
            OperationName = "AddTextByTags",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddTextByTagsData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }

    public static AddKeyedTextByTagsData AddKeyedTextByTags(
        string key,
        List<CaretTagInput>? tags,
        string textAndCarets
    )
    {
        var variables = new Dictionary<string, object?>();
        variables.Add("key", key);
        variables.Add("tags", tags);
        variables.Add("textAndCarets", textAndCarets);

        var request = new GraphQLRequest
        {
            Query =
                @"
            mutation AddKeyedTextByTags($key: String!, $tags: [CaretTagInput!]!, $textAndCarets: String!) {
  addKeyedTextByTags(key: $key, tags: $tags, textAndCarets: $textAndCarets) {
    id
  }
},
            ",
            OperationName = "AddKeyedTextByTags",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<AddKeyedTextByTagsData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }

    public static LogData Log(LogSeverity severity, string message, List<string>? arguments)
    {
        var variables = new Dictionary<string, object?>();
        variables.Add("severity", severity.ToString());
        variables.Add("message", message);
        variables.Add("arguments", arguments);

        var request = new GraphQLRequest
        {
            Query =
                @"
            mutation Log($severity: LogSeverity!, $message: String!, $arguments: [String!]) {
  log(severity: $severity, message: $message, arguments: $arguments)
},
            ",
            OperationName = "Log",
            Variables = variables,
        };

        var response = CodegenBotHost.GraphQL(request);
        var result = JsonSerializer.Deserialize<GraphQLResponse<LogData>>(
            response,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        return result?.Data;
    }
}

public class GraphQLResponse<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("errors")]
    public List<GraphQLError>? Errors { get; set; }
}

public class GraphQLError
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public enum FileKind
{
    BINARY,
    TEXT,
}

public enum FileVersion
{
    GENERATED,
    HEAD,
}

public enum LogSeverity
{
    TRACE,
    DEBUG,
    INFORMATION,
    WARNING,
    ERROR,
    CRITICAL,
}

public class CaretTagInput
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class GetConfigurationData
{
    [JsonPropertyName("configuration")]
    public GetConfiguration Configuration { get; set; }
}

public class GetConfiguration
{
    [JsonPropertyName("outputPath")]
    public string OutputPath { get; set; }
}

public class GetFilesData
{
    [JsonPropertyName("kind")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FileKind Kind { get; set; }

    [JsonPropertyName("files")]
    public List<GetFiles>? Files { get; set; }
}

public class GetFiles
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("kind")]
    public FileKind Kind { get; set; }
}

public class GetFileContentsData
{
    [JsonPropertyName("readTextFile")]
    public string ReadTextFile { get; set; }
}

public class AddFileData
{
    [JsonPropertyName("addFile")]
    public AddFile AddFile { get; set; }
}

public class AddFile
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class AddTextData
{
    [JsonPropertyName("addText")]
    public AddText AddText { get; set; }
}

public class AddText
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class AddKeyedTextData
{
    [JsonPropertyName("addKeyedText")]
    public AddKeyedText AddKeyedText { get; set; }
}

public class AddKeyedText
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class AddTextByTagsData
{
    [JsonPropertyName("addTextByTags")]
    public List<AddTextByTags>? AddTextByTags { get; set; }
}

public class AddTextByTags
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class AddKeyedTextByTagsData
{
    [JsonPropertyName("addKeyedTextByTags")]
    public List<AddKeyedTextByTags>? AddKeyedTextByTags { get; set; }
}

public class AddKeyedTextByTags
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class LogData
{
    [JsonPropertyName("log")]
    public string Log { get; set; }
}
