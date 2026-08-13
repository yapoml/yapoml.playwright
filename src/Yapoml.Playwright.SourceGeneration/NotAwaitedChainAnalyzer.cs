using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Yapoml.Playwright.SourceGeneration;

/// <summary>
/// Reports fluent chains which are never awaited, and therefore never executed.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NotAwaitedChainAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "YA0001";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        "Chain is not awaited",
        "The chain is not awaited, so none of its steps are executed. Add 'await' before the expression",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.ExpressionStatement);
    }

    private static void Analyze(SyntaxNodeAnalysisContext context)
    {
        var statement = (ExpressionStatementSyntax)context.Node;

        if (statement.Expression is AwaitExpressionSyntax || statement.Expression is AssignmentExpressionSyntax)
        {
            return;
        }

        // Chains built inside synchronous page object methods are meant to be awaited by their caller.
        if (!IsInAsyncContext(statement))
        {
            return;
        }

        var type = context.SemanticModel.GetTypeInfo(statement.Expression, context.CancellationToken).Type;

        if (type is null || type.TypeKind == TypeKind.Error)
        {
            return;
        }

        // Task-returning calls are already covered by CS4014.
        if (type.Name == "Task" || type.Name == "ValueTask")
        {
            return;
        }

        if (IsAwaitable(type))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, statement.GetLocation()));
        }
    }

    private static bool IsAwaitable(ITypeSymbol type)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            var found = current.GetMembers("GetAwaiter")
                .OfType<IMethodSymbol>()
                .Any(m => !m.IsStatic && m.Parameters.Length == 0 && m.DeclaredAccessibility == Accessibility.Public);

            if (found)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsInAsyncContext(SyntaxNode node)
    {
        foreach (var ancestor in node.Ancestors())
        {
            switch (ancestor)
            {
                case MethodDeclarationSyntax method:
                    return method.Modifiers.Any(SyntaxKind.AsyncKeyword);
                case LocalFunctionStatementSyntax localFunction:
                    return localFunction.Modifiers.Any(SyntaxKind.AsyncKeyword);
                case AnonymousFunctionExpressionSyntax anonymousFunction:
                    return anonymousFunction.AsyncKeyword != default;
                case AccessorDeclarationSyntax:
                case ConstructorDeclarationSyntax:
                    return false;
            }
        }

        return false;
    }
}
