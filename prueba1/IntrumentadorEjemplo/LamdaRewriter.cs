using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IntrumentadorEjemplo
{
    // El CSharpSyntaxRewriter hereda de SyntaxVisitor, vamos a recorrer los nodos del código con este visitor
    class LamdaRewriter : CSharpSyntaxRewriter
    {
        private readonly SemanticModel SemanticModel;

        public LamdaRewriter(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
        }

        public override SyntaxNode Visit(SyntaxNode node)
        {
            return base.Visit(node);
        }
        
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var newList = new List<StatementSyntax>();
            foreach (var statement in node.Body.ChildNodes().OfType<StatementSyntax>())
            {
                if (statement.DescendantNodes().OfType<LambdaExpressionSyntax>().Any() ||
                    statement.DescendantNodes().OfType<SimpleLambdaExpressionSyntax>().Any())
                    newList.Add(GetConsoleWriteLine().WithTriviaFrom(statement));
                newList.Add(statement);
            }
            var newBlockNode = Block(newList).WithTriviaFrom(node.Body);

            var blockNode = node.Body;
            node = node.ReplaceNode(blockNode, newBlockNode);
            return base.VisitMethodDeclaration(node);
        }
        public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            return base.VisitParenthesizedLambdaExpression(node);
        }
        public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            return base.VisitSimpleLambdaExpression(node);
        }

        ExpressionStatementSyntax GetConsoleWriteLine()
        {
            var console = SyntaxFactory.IdentifierName("Console");
            var writeline = SyntaxFactory.IdentifierName("WriteLine");
            var memberaccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, console, writeline);

            var argument = SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("A")));
            var argumentList = SyntaxFactory.SeparatedList(new[] { argument });

            var writeLineCall =
                SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(memberaccess,
                SyntaxFactory.ArgumentList(argumentList)));

            return writeLineCall;
        }
    }
}
