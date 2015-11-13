using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GettingStartedCS
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Declaración de código
            var _code = @"using System;
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
            var i = 1;
        }
    }
}";
            #endregion

            SyntaxTree tree = CSharpSyntaxTree.ParseText(_code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            var namespaceMember = (NamespaceDeclarationSyntax)root.Members.First();

            var firstMember = root.Members[0];

            var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;

            var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];

            var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];

            var main_returnType = mainDeclaration.ReturnType;
            var main_identifier = mainDeclaration.Identifier;

            // Parámetros
            var argsParameter = mainDeclaration.ParameterList.Parameters[0];

            // Vemos los members y los tipos de cada uno dentro del namespace:
            foreach (BaseTypeDeclarationSyntax member in namespaceMember.Members.Cast<BaseTypeDeclarationSyntax>())
            {
                // VER Identifier y Kind
                // "Identifier, which is of type 'Kind'"
                //      --> Ejemplo: Program
                // El kind lo estoy reemplazando por: Enum.GetName(typeof(SyntaxKind), <Kind>) porque falla sino.
                //      --> Ejemplo: ClassDeclaration
                Console.ReadKey();
            }
        }
    }
}
