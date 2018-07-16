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

namespace Tests.Cases.Roslyn
{
    delegate void Printer(string s);
    class AnonymousMethod
    {
        //public static void Main(string[] args)
        //{
        //    //Printer p = delegate (string j)
        //    //{
        //    //    System.Console.WriteLine(j);
        //    //};
        //    //p("The delegate using the anonymous method is called.");
        //}

        static void Main(string[] args)
        {
            PRUEBA s1 = new PRUEBA() { tmp = "A1" };
            PRUEBA s2 = new PRUEBA() { tmp = "A2" };
            testMethod(s1, s2);
            System.Console.WriteLine(s1.tmp);
            //s1 = s1 + "s";
            return;
        }

        static void testMethod(PRUEBA s1, PRUEBA s2) 
        {
            s1.tmp = s2.tmp;
        }

        class PRUEBA
        {
            public string tmp { get; set; }
        }

        static void Main2(string[] args)
        {
            var code = ""; // getCode1();

            // Árbol sintáctico
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            // Compilación
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var collections = MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location);
            var linq = MetadataReference.CreateFromFile(typeof(System.Linq.Queryable).Assembly.Location);

            MetadataReference[] references = { mscorlib, collections, linq };

            var compilation = CSharpCompilation.Create("HelloWorld", new SyntaxTree[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.ConsoleApplication));
            var diagnostico = compilation.GetDiagnostics();
            // Modelo semántico
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Extraemos el método principal
            var mainMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Single();
            var mainMethodBody = root.DescendantNodes().OfType<BlockSyntax>().First();
            //var mainClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            //var namespaceDeclaration = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Single();

            var tmp1 = semanticModel.GetOperation(mainMethod);
            var tmp2 = semanticModel.GetOperation(mainMethodBody);
            //var tmp3 = semanticModel.GetOperation(mainClass);
            //var tmp4 = semanticModel.GetOperation(namespaceDeclaration);

            IBlockStatement operation = semanticModel.GetOperation(mainMethodBody) as IBlockStatement;
            //analize(semanticModel, operation);
            //IStatement lastStatement = operation.Statements.Last();

            //TODO: Descomentar
            //foreach (var statement in operation.Statements)
            //    analize(semanticModel, statement);
            Console.ReadKey();
        }


        public static void analize(SemanticModel semanticModel, IOperation statement)
        {
            if (statement is IBlockStatement)
                Console.WriteLine("Analizando un bloque de código");
            else
                Console.WriteLine("Analizando: " + statement.Syntax.ToString());

            var analisis = semanticModel.AnalyzeDataFlow(statement.Syntax);
            var readInside = analisis.ReadInside;
            Console.WriteLine("\t" + "ReadInside:");
            print(readInside);
            var readOutside = analisis.ReadOutside;
            Console.WriteLine("\t" + "ReadOutside:");
            print(readOutside);
            var writtenInside = analisis.WrittenInside;
            Console.WriteLine("\t" + "WrittenInside:");
            print(writtenInside);
            var writtenOutside = analisis.WrittenOutside;
            Console.WriteLine("\t" + "WrittenOutside:");
            print(writtenOutside);
            Console.WriteLine("");
        }

        public static void print(System.Collections.Immutable.ImmutableArray<ISymbol> lista)
        {
            foreach (var variable in lista)
            {
                Console.WriteLine("\t\t" + variable.Name);
            }
        }

        static string getCode1()
        {
            return @"using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(""Hello, World!"");
            //var i = 1;
            //var list = new List<int>() { 1, 2, 3 };
            //var tmp = list.Any(x => x == i);
            //var nuevaPrueba = 1 + 2 + 3 + 4;
            //var prueba2 = new CA(new CB()) { tmp2 = 1 };
            //var j = prueba2.tmp2;
            var prueba = new CA();
            prueba.f = new CA();
            prueba.f.f = new CA();
            prueba.f.f.g = new CB();
            var a = prueba.f.f.g;
            prueba.f = prueba.f.f; 
        }
    }

    public class CA { public CA f; public CB g; public int tmp2 { get; set; } public CA(CB _cb) { } }
    public class CB { }
}";
        }

        static string getCode2()
        {
            return @"using System;

namespace HelloWorld
{
    class LiteralString
    {
        static void Main(string[] args)
        {
            string a = ""hello, world"";                  // hello, world
           /* string b = @""hello, world"";               // hello, world
            string c = ""hello \t world"";               // hello     world
            string d = @""hello \t world"";               // hello \t world
            string e = ""Joe said \""Hello\"" to me"";      // Joe said ""Hello"" to me
            string f = @""Joe said """"Hello"""" to me"";   // Joe said ""Hello"" to me
            string g = ""\\\\server\\share\\file.txt"";   // \\server\share\file.txt
            string h = @""\\server\share\file.txt"";      // \\server\share\file.txt
            string i = ""one\r\ntwo\r\nthree""; */

            string result = a;
            /*string result = a + b;
            result = result + c;
            result = result + d;
            result = result + e;
            result = result + f;
            result = result + g;
            result = result + h;
            result = result + i;*/
            Console.WriteLine(result);
            return;
        }
    }
}
";
        }

    }
}