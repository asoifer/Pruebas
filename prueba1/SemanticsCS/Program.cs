using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SemanticsCS
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Declaraci�n de c�digo
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
        }
    }
}";
            #endregion

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

            var root = (CompilationUnitSyntax)tree.GetRoot();

            var compilation = CSharpCompilation.Create("HelloWorld")
                                               .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                                               .AddSyntaxTrees(tree);

            var model = compilation.GetSemanticModel(tree);

            var nameInfo = model.GetSymbolInfo(root.Usings[0].Name); // devuelve un SymbolInfo 
                                                                     // La propiedad m�s importante es "Symbol":
                                                                     // This property returns the Symbol this expression refers to. 
                                                                     // Para literales, el valor es null (no refiere a nada).
            var symbol = (INamespaceSymbol)nameInfo.Symbol;
            var tipo = symbol.Kind; // SymbolKind.Namespace

            // Los namespaces del s�mbolo de namespace System
            foreach (var ns in symbol.GetNamespaceMembers())
            {
                Console.WriteLine(ns.Name);
            }

            // Otro ejemplo de bindear: Hello World!
            // --> PARTE SINT�CTICA
            var helloWorldString = root.DescendantNodes()
                                       .OfType<LiteralExpressionSyntax>()
                                       .First();
            // --> PARTE SEM�NTICA
            var literalInfo = model.GetTypeInfo(helloWorldString);

            // M�todos del tipo de "Hello World!", o sea, del tipo System.String:
            var stringTypeSymbol = (INamedTypeSymbol)literalInfo.Type;

            Console.Clear();
            foreach (var name in (from method in stringTypeSymbol.GetMembers()
                                                              .OfType<IMethodSymbol>()
                                  where method.ReturnType.Equals(stringTypeSymbol) &&
                                        method.DeclaredAccessibility ==
                                                   Accessibility.Public
                                  select method.Name).Distinct())
            {
                Console.WriteLine(name);
            }
        }
    }
}