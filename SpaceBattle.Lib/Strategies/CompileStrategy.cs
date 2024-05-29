using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Hwdtech;

namespace SpaceGame.Lib;

public class CompileStrategy
{
    public object Run(params object[] args)
    {
        var codeString = (string)args[0];

        var assemblyName = IoC.Resolve<string>("Assembly.Name.Create");
        var references = IoC.Resolve<IEnumerable<MetadataReference>>("Compile.References");
        var choices = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        var syntaxTree = CSharpSyntaxTree.ParseText(codeString);

        var compilation = CSharpCompilation.Create(assemblyName).AddReferences(references);
        compilation = compilation.WithOptions(choices).AddSyntaxTrees(syntaxTree);

        using (var ms = new System.IO.MemoryStream())
        {
            var result = compilation.Emit(ms);
            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(ms.ToArray());
        }

        return assembly;
    }
}

