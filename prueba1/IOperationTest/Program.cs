using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using Microsoft.CodeAnalysis.Semantics;

namespace IOperationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Declaración de código
            var code = @"using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
            var i = 1;
            var list = new List<int>() { 1, 2, 3 };
            var tmp = list.Any(x => x == i);
        }
    }
}";
            #endregion

            // Árbol sintáctico
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            // Compilación
            var compilation = CSharpCompilation.Create("HelloWorld").AddSyntaxTrees(syntaxTree);
            // Modelo semántico
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Extraemos el método principal
            var mainMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Single().Body;
            IBlockStatement operation = semanticModel.GetOperation(mainMethod) as IBlockStatement;
            IStatement lastStatement = operation.Statements.Last();


            var variablesDependientes = semanticModel.AnalyzeDataFlow(lastStatement.Syntax).WrittenOutside;
        }
    }
}
