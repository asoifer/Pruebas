using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOperationTest
{
    class Program3
    {
        static void Main()
        {
            var src = @"C:\Users\Camilo\Desktop\Slicer\integracion\test\.NET Frameworks\IOC\MEF\MEF_Program_Files\CalculatorDemoUsingMEF.sln";
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(src).Result;
            var project = solution.Projects.Where(x => x.Name == "CalculatorUI").Single();
            var options = project.ParseOptions.WithFeatures(
                                project.ParseOptions.Features.Union(
                                new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("IOperation", "true") }));
            var ioperation_project = project.WithParseOptions(options);
            var compilatedProject = ioperation_project.GetCompilationAsync().Result;
            var document = ioperation_project.Documents.Where(x => x.Name == "MainWindow.xaml.cs").Single();
            var syntaxTree = document.GetSyntaxTreeAsync().Result;
            var syntaxNode = syntaxTree.GetRoot().DescendantNodes().Where(x => x.GetText().ToString().Trim() == "InitializeComponent()").First();
            var semanticModel = compilatedProject.GetSemanticModel(syntaxTree);
            var ioperationValue = semanticModel.GetOperation(syntaxNode);
            Console.WriteLine(ioperationValue.Kind.ToString());
        }
    }
}
