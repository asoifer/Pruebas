using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UsingCollectorCS
{
    class UsingCollector : CSharpSyntaxWalker
    {
        public readonly List<UsingDirectiveSyntax> Usings = new List<UsingDirectiveSyntax>();
        
        // Hay un visit por cada tipo de nodo. Es bastante útil.
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            // Propiedad últil de los nodos: CSharpKind (en este caso sería siempre UsingDirective)

            /*Note the Name property of type NameSyntax; this stores the name of the namespace being imported.*/
            if (node.Name.ToString() != "System" &&
                !node.Name.ToString().StartsWith("System."))
            {
                this.Usings.Add(node);
            }
        }
    }
}
