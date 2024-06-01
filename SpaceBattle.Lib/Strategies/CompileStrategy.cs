using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CompileStrategy
{
    public object Run(params object[] args)
    {
        Assembly assembly;
        var code = (string)args[0];

        var assemblyStr = IoC.Resolve<string>("Assembly.Create.Name");
        var reference = IoC.Resolve<IEnumerable<MetadataReference>>("Compile.References");

        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        var tree = CSharpSyntaxTree.ParseText(code);

        var compile = CSharpCompilation.Create(assemblyStr).AddReferences(reference);
        compile = compile.WithOptions(options).AddSyntaxTrees(tree);

        using (var memory = new System.IO.MemoryStream())
        {
            var result = compile.Emit(memory);
            memory.Seek(0, SeekOrigin.Begin);
            assembly = Assembly.Load(memory.ToArray());
        }

        return assembly;
    }
}

