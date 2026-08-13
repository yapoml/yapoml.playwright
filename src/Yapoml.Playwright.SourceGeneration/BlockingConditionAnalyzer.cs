using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Yapoml.Playwright.SourceGeneration;

/// <summary>
/// Reports blocking calls inside condition types, which run eagerly instead of joining the awaited chain.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BlockingConditionAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "YA0002";

    private const string BaseConditionsMetadataName = "Yapoml.Playwright.Components.BaseConditions";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        "Blocking call inside a condition",
        "This blocking call runs eagerly instead of joining the awaited chain. Wrap the asynchronous work in 'Enqueue(async () => ...)' and 'await' it",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        if (context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken).Symbol is not IMethodSymbol method)
        {
            return;
        }

        var isBlocking =
            method.Name == "GetResult" && method.ContainingType?.ContainingNamespace?.ToDisplayString() == "System.Runtime.CompilerServices";

        if (!isBlocking)
        {
            return;
        }

        Report(context, invocation);
    }

    private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
    {
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;

        if (memberAccess.Name.Identifier.ValueText != "Result")
        {
            return;
        }

        // .Result on a Task is a blocking wait
        if (context.SemanticModel.GetTypeInfo(memberAccess.Expression, context.CancellationToken).Type is not INamedTypeSymbol receiver)
        {
            return;
        }

        var receiverName = receiver.ConstructedFrom?.ToDisplayString() ?? receiver.ToDisplayString();

        if (receiverName != "System.Threading.Tasks.Task" && receiverName != "System.Threading.Tasks.Task<TResult>")
        {
            return;
        }

        Report(context, memberAccess);
    }

    private static void Report(SyntaxNodeAnalysisContext context, SyntaxNode node)
    {
        if (!IsInsideConditions(context, node))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation()));
    }

    private static bool IsInsideConditions(SyntaxNodeAnalysisContext context, SyntaxNode node)
    {
        var typeDeclaration = node.Ancestors().OfType<TypeDeclarationSyntax>().FirstOrDefault();

        if (typeDeclaration is null)
        {
            return false;
        }

        var type = context.SemanticModel.GetDeclaredSymbol(typeDeclaration, context.CancellationToken);

        for (var current = type; current is not null; current = current.BaseType)
        {
            if ((current.ConstructedFrom ?? current).ToDisplayString().StartsWith(BaseConditionsMetadataName))
            {
                return true;
            }
        }

        return false;
    }
}
