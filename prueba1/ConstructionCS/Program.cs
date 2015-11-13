using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ConstructionCS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ejemplo para evaluar el símbolo de algo
            NameSyntax name = IdentifierName("System"); // Es el identificador básico: "System" (IdentifierNameSyntax)
            name = QualifiedName(name, IdentifierName("Collections")); // Le podemos agregar un namespace de profundidad, ej.: Collections (QualifiedNameSyntax)
            name = QualifiedName(name, IdentifierName("Generic"));

            // Ahora vamos a hacer reemplazos
            #region Declaración de código
            var code = @"using System;
using System.Collections;
using System.Linq;
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

            var oldUsing = root.Usings[1];
            var newUsing = oldUsing.WithName(name); // La instrucción "WithName" reemplaza los nombres
            root = root.ReplaceNode(oldUsing, newUsing); // Reemplazo y uno nuevo
        }
    }
}
