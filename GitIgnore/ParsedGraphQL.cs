using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitIgnore;

public class GraphQLOperation
{
    public required string? Name { get; set; }
    public GraphQLOperationType OperationType { get; set; }
    public List<GraphQLVariable> Variables { get; set; } = new();
    public List<GraphQLDirective> Directives { get; set; } = new();
    public List<IGraphQLSelection> Selections { get; set; } = new();
}

public enum GraphQLOperationType
{
    Query,
    Mutation,
    Subscription,
}

public class GraphQLFragment
{
    public required string Name { get; set; }
    public required string TypeCondition { get; set; }
    public List<GraphQLVariable> Variables { get; set; } = new();
    public List<GraphQLDirective> Directives { get; set; } = new();
    public List<IGraphQLSelection> Selections { get; set; } = new();
}

public class GraphQLVariable
{
    public GraphQLValue? DefaultValue { get; set; }
    public List<GraphQLDirective> Directives { get; set; } = new();
    public required string Name { get; set; }
    public required TypeRef Type { get; set; }
}

[JsonPolymorphic]
[JsonDerivedType(typeof(GraphQLBooleanValue), nameof(GraphQLBooleanValue))]
[JsonDerivedType(typeof(GraphQLEnumValue), nameof(GraphQLEnumValue))]
[JsonDerivedType(typeof(GraphQLFloatValue), nameof(GraphQLFloatValue))]
[JsonDerivedType(typeof(GraphQLIntValue), nameof(GraphQLIntValue))]
[JsonDerivedType(typeof(GraphQLListValue), nameof(GraphQLListValue))]
[JsonDerivedType(typeof(GraphQLNullValue), nameof(GraphQLNullValue))]
[JsonDerivedType(typeof(GraphQLObjectValue), nameof(GraphQLObjectValue))]
[JsonDerivedType(typeof(GraphQLStringValue), nameof(GraphQLStringValue))]
[JsonDerivedType(typeof(GraphQLVariableValue), nameof(GraphQLVariableValue))]
public abstract record GraphQLValue();

public record GraphQLBooleanValue(bool Value) : GraphQLValue();

public record GraphQLEnumValue(string Value) : GraphQLValue();

public record GraphQLFloatValue(string Value) : GraphQLValue();

public record GraphQLIntValue(string Value) : GraphQLValue();

public record GraphQLListValue(IReadOnlyList<GraphQLValue> Value) : GraphQLValue();

public record GraphQLNullValue() : GraphQLValue();

public record GraphQLObjectValue(ImmutableDictionary<string, GraphQLValue> Value) : GraphQLValue();

public record GraphQLStringValue(string Value) : GraphQLValue();

public record GraphQLVariableValue(string Name) : GraphQLValue();

public record TypeRef(string Name, ImmutableList<TypeRef> GenericArguments, string Text) { }

public class GraphQLDirective
{
    public required string Name { get; set; }
    public List<GraphQLArgument> Arguments { get; set; } = new();
}

public class GraphQLArgument
{
    public required string Name { get; set; }
    public required GraphQLValue Value { get; set; }
}

[JsonPolymorphic()]
[JsonDerivedType(typeof(GraphQLFieldSelection), nameof(GraphQLFieldSelection))]
[JsonDerivedType(typeof(GraphQLFragmentSpreadSelection), nameof(GraphQLFragmentSpreadSelection))]
[JsonDerivedType(typeof(GraphQLInlineFragmentSelection), nameof(GraphQLInlineFragmentSelection))]
public interface IGraphQLSelection { }

public class GraphQLFieldSelection : IGraphQLSelection
{
    public required string Name { get; set; }
    public string? Alias { get; set; }
    public List<GraphQLArgument> Arguments { get; set; } = new();
    public List<GraphQLDirective> Directives { get; } = new();
    public List<IGraphQLSelection> Selections { get; } = new();
}

public class GraphQLFragmentSpreadSelection : IGraphQLSelection
{
    public required string FragmentName { get; set; }
}

public class GraphQLInlineFragmentSelection : IGraphQLSelection
{
    public required string TypeName { get; set; }
    public List<IGraphQLSelection> Selections { get; set; } = new();
}
