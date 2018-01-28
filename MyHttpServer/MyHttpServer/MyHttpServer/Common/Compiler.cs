using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace MyHttpServer.Common
{
    public class Compiler
    {
        public static bool CompileCSharpCode(String exeFile, params string[] sourceFiles)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();

            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters();

            // Add an assembly reference.
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("mscorlib.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.ReferencedAssemblies.Add("MyHttpServer.dll");
            //需要定义一个数据结构，存储命名空间所在的程序集的字典?

            // Set the assembly file name to generate.
            cp.OutputAssembly = exeFile;

            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFiles);
            //CompilerResults cr = provider.CompileAssemblyFromSource(cp, sourceFiles);

            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0} into {1}",
                    sourceFiles, cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Source {0} built into {1} successfully.",
                    sourceFiles, cr.PathToAssembly);
            }

            // Return the results of compilation.
            if (cr.Errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static Assembly Compile(string csFile)
        {
            var compileResult = Path.ChangeExtension(csFile, ".dll");
            if (!File.Exists(compileResult))
            {
                if (CompileCSharpCode(compileResult, csFile))
                    return Assembly.LoadFrom(compileResult);
                else
                    return null;
            }
            return Assembly.LoadFrom(compileResult);
        }
        public static List<string> GetCsFiles(string path)
        {
            List<string> r = new List<string>();
            var files = Directory.GetFiles(path, "*.cs");
            r.AddRange(files);
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                r.AddRange(GetCsFiles(dir));
            }
            return r;
        }
        public static Assembly CompilePath(string path, string exeFile)
        {
            var files = GetCsFiles(path);
            if (CompileCSharpCode(exeFile, files.ToArray()))
                return Assembly.LoadFrom(exeFile);
            else
                return null;
        }
    }
}
