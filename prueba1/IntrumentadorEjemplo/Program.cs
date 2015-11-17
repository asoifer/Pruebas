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

namespace IntrumentadorEjemplo
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
            var tmp = list.Any(x => x == 1);
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

            foreach (SyntaxTree sourceTree in compilation.SyntaxTrees)
            {
                var model = compilation.GetSemanticModel(sourceTree);

                LamdaRewriter rewriter = new LamdaRewriter(model);

                SyntaxNode newSource = rewriter.Visit(sourceTree.GetRoot());

                Console.WriteLine("Actual:");
                Console.WriteLine("");
                Console.Write(sourceTree.GetText());
                Console.WriteLine("");
                Console.WriteLine("Nuevo:");
                Console.WriteLine("");
                Console.Write(newSource.ToFullString());
            }
            Console.ReadKey();

            // Análisis de variables:
            var mainMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Single().Body;
            DataFlowAnalysis result = semanticModel.AnalyzeDataFlow(mainMethod);
            Console.WriteLine("");
            Console.WriteLine("Variables declaradas: ");
            foreach (var declaredVariable in result.VariablesDeclared)
                Console.WriteLine("Name: " + declaredVariable.Name);
            Console.ReadKey();
        }
    }
}
