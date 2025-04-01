using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using CodegenBot;

namespace GitIgnore;

#pragma warning disable CS9113 // Parameter is unread.
public partial class Query(IServiceProvider services)
#pragma warning restore CS9113 // Parameter is unread.
{
    public JsonNode Resolve(
        IReadOnlyDictionary<string, object?> variables,
        IEnumerable<IPreParsedGraphQLSelection> selections
    )
    {
        var jsonNode = new JsonObject();

        foreach (var selection in selections)
        {
            if (selection is PreParsedGraphQLFieldSelection fieldSelection)
            {
                if (fieldSelection.Name == "hasIgnoreFile")
                {
                    string? folder = null;

                    foreach (var arg in fieldSelection.Arguments)
                    {
                        if (arg.Name == "folder")
                        {
                            if (arg.Value is PreParsedGraphQLStringValue literal)
                            {
                                folder = literal.Value;
                            }
                            else if (arg.Value is PreParsedGraphQLNullValue)
                            {
                                folder = null;
                            }
                            else if (arg.Value is PreParsedGraphQLVariableValue graphqlVariable)
                            {
                                if (!variables.TryGetValue(graphqlVariable.Name, out var result))
                                {
                                    throw new ArgumentException(
                                        $"{graphqlVariable.Name} is not specified"
                                    );
                                }
                                folder = ((JsonElement?)result)?.GetString();
                            }
                            else
                            {
                                throw new ArgumentException("folder is not specified");
                            }
                        }
                    }
                    jsonNode[fieldSelection.Alias ?? fieldSelection.Name] = GetHasIgnoreFile(
                        folder
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

    public partial bool GetHasIgnoreFile(string? folder);
}

#pragma warning disable CS9113 // Parameter is unread.
public partial class Mutation(IServiceProvider services)
#pragma warning restore CS9113 // Parameter is unread.
{
    public JsonNode Resolve(
        IReadOnlyDictionary<string, object?> variables,
        IEnumerable<IPreParsedGraphQLSelection> selections
    )
    {
        var jsonNode = new JsonObject();

        foreach (var selection in selections)
        {
            if (selection is PreParsedGraphQLFieldSelection fieldSelection)
            {
                if (fieldSelection.Name == "addIgnorePattern")
                {
                    string? folder = null;
                    IReadOnlyList<string>? pattern = null;

                    foreach (var arg in fieldSelection.Arguments)
                    {
                        if (arg.Name == "folder")
                        {
                            if (arg.Value is PreParsedGraphQLStringValue literal)
                            {
                                folder = literal.Value;
                            }
                            else if (arg.Value is PreParsedGraphQLNullValue)
                            {
                                folder = null;
                            }
                            else if (arg.Value is PreParsedGraphQLVariableValue graphqlVariable)
                            {
                                if (!variables.TryGetValue(graphqlVariable.Name, out var result))
                                {
                                    throw new ArgumentException(
                                        $"{graphqlVariable.Name} is not specified"
                                    );
                                }
                                folder = ((JsonElement?)result)?.GetString();
                            }
                            else
                            {
                                throw new ArgumentException("folder is not specified");
                            }
                        }
                        if (arg.Name == "pattern")
                        {
                            if (arg.Value is PreParsedGraphQLListValue literal)
                            {
                                pattern = literal
                                    .Value.Select(x =>
                                    {
                                        string? patternElement = null;
                                        if (x is PreParsedGraphQLStringValue literal)
                                        {
                                            patternElement = literal.Value;
                                        }
                                        else if (x is PreParsedGraphQLVariableValue graphqlVariable)
                                        {
                                            if (
                                                !variables.TryGetValue(
                                                    graphqlVariable.Name,
                                                    out var result
                                                )
                                            )
                                            {
                                                throw new ArgumentException(
                                                    $"{graphqlVariable.Name} is not specified"
                                                );
                                            }
                                            if (result is null)
                                            {
                                                throw new ArgumentNullException(
                                                    "patternElement is null"
                                                );
                                            }
                                            patternElement = ((JsonElement)result).GetString();
                                        }
                                        if (patternElement is null)
                                        {
                                            throw new ArgumentException(
                                                "patternElement is not specified"
                                            );
                                        }
                                        return patternElement;
                                    })
                                    .ToList();
                            }
                            else if (arg.Value is PreParsedGraphQLVariableValue graphqlVariable)
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
                                pattern = ((JsonElement)result)
                                    .EnumerateArray()
                                    .Select(x => x.GetString()!)
                                    .ToList();
                            }
                        }
                    }
                    jsonNode[fieldSelection.Alias ?? fieldSelection.Name] = AddIgnorePattern(
                        folder,
                        pattern ?? throw new ArgumentNullException("pattern is null")
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

    public partial bool AddIgnorePattern(string? folder, IReadOnlyList<string> pattern);
}

#pragma warning disable CS9113 // Parameter is unread.
public partial class GraphQLServer(IServiceProvider services)
#pragma warning restore CS9113 // Parameter is unread.
{
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
        Justification = "<Pending>"
    )]
    public string ProcessGraphQLRequest(string request)
    {
        var parsedRequest = PreParsedGraphQLRequest.FromJsonString(request);

        JsonNode? jsonNode = null;
        var errors = new JsonArray();

        if (parsedRequest is not null)
        {
            if (parsedRequest.Query.Operation.OperationType == PreParsedGraphQLOperationType.Query)
            {
                var obj = new Query(services);
                jsonNode = obj.Resolve(
                    parsedRequest.Variables ?? new(),
                    parsedRequest.Query.Operation.Selections
                );
            }
            if (
                parsedRequest.Query.Operation.OperationType
                == PreParsedGraphQLOperationType.Mutation
            )
            {
                var obj = new Mutation(services);
                jsonNode = obj.Resolve(
                    parsedRequest.Variables ?? new(),
                    parsedRequest.Query.Operation.Selections
                );
            }
        }
        else
        {
            errors.Add("Failed to parse request");
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
