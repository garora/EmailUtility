using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Utility.Core
{
    public static class LINQDynamicCompile
    {
        private static readonly Dictionary<string, MethodInfo> queryDictionary = new Dictionary<string, MethodInfo>();

        public static MethodInfo GetLINQMethodPaged(string linq, string businessNamespace, string objectName)
        {
            return GetLINQMethodPaged(linq, businessNamespace, objectName, new Assembly[0]);
        }

        public static MethodInfo GetLINQMethodPaged(string linq, string businessNamespace, string objectName,
            Assembly[] assemblies)
        {
            HashHelper.ComputeHash(linq + "|" + businessNamespace + "|" + objectName + "|Paged");
            var key = DateTime.Now.ToString("HHmmss.fff");
            MethodInfo methodInfo;
            if (!queryDictionary.ContainsKey(key))
            {
                methodInfo = CreateScriptClassPaged(Guid.NewGuid(), businessNamespace, linq, objectName, assemblies);
                queryDictionary.Add(key, methodInfo);
            }
            else
                methodInfo = queryDictionary[key];
            return methodInfo;
        }

        public static MethodInfo GetLINQMethodAll(string linq, string businessNamespace, string objectName)
        {
            HashHelper.ComputeHash(linq + "|" + businessNamespace + "|" + objectName + "|All");
            var key = DateTime.Now.ToString("HHmmss.fff");
            MethodInfo methodInfo;
            if (!queryDictionary.ContainsKey(key))
            {
                methodInfo = CreateScriptClassAll(Guid.NewGuid(), businessNamespace, linq, objectName);
                queryDictionary.Add(key, methodInfo);
            }
            else
                methodInfo = queryDictionary[key];
            return methodInfo;
        }

        private static MethodInfo CreateScriptClassPaged(Guid currentGuid, string businessNamespace, string linq,
            string objectName, Assembly[] assemblies)
        {
            var str1 = currentGuid.ToString("N");
            var stringBuilder1 = new StringBuilder();
            stringBuilder1.AppendLine("using System;");
            stringBuilder1.AppendLine("using System.Linq;");
            stringBuilder1.AppendLine("using System.Linq.Expressions;");
            stringBuilder1.AppendLine("using " + businessNamespace + ".Objects;");
            stringBuilder1.AppendLine("using " + businessNamespace + ".LINQ;");
            stringBuilder1.AppendLine("namespace Widgetpshere.MemoryGeneration");
            stringBuilder1.AppendLine("{");
            stringBuilder1.AppendLine("public static class " + objectName + "Queries" + str1);
            stringBuilder1.AppendLine("{");
            stringBuilder1.AppendLine("    public static " + objectName + "Collection ZZ" + str1 +
                                      "(Pyramid.Core.DataAccess.IPagingObject paging)");
            stringBuilder1.AppendLine("    {");
            stringBuilder1.AppendLine("      if (!(paging is " + objectName + "Paging))");
            stringBuilder1.AppendLine("      {");
            stringBuilder1.AppendLine("\t\t\t\t" + objectName + "Paging newPaging = new " + objectName + "Paging();");
            stringBuilder1.AppendLine("\t\t\t\tnewPaging.PageIndex = paging.PageIndex;");
            stringBuilder1.AppendLine("\t\t\t\tnewPaging.RecordsperPage = paging.RecordsperPage;");
            stringBuilder1.AppendLine("        paging = newPaging;");
            stringBuilder1.AppendLine("      }");
            stringBuilder1.AppendLine();
            stringBuilder1.AppendLine("        return " + objectName + "Collection.RunSelect(" + linq + ", (" +
                                      objectName + "Paging)paging);");
            stringBuilder1.AppendLine("    }");
            stringBuilder1.AppendLine("}");
            stringBuilder1.AppendLine("}");
            try
            {
                var csharpCodeProvider = new CSharpCodeProvider(new Dictionary<string, string>
                {
                    {
                        "CompilerVersion",
                        "v3.5"
                    }
                });
                var options = new CompilerParameters();
                var assembly1 = Assembly.LoadWithPartialName(businessNamespace.Remove(businessNamespace.Length - 9));
                string path1 = ConfigurationManager.AppSettings["AppServerBinDirectory"];
                if (string.IsNullOrEmpty(path1))
                    path1 = new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Directory.FullName;
                if (!string.IsNullOrEmpty(path1))
                {
                    var list = new List<string>();
                    if (!path1.ToLower().Contains("windows\\assembly\\gac_msil") &&
                        !path1.ToLower().Contains("microsoft.net\\assembly"))
                    {
                        options.CompilerOptions = string.Format("/lib:\"{0}\"", path1);
                        list.Add(path1);
                    }
                    else
                    {
                        var fileInfo = new FileInfo(new Uri(assembly1.CodeBase).LocalPath);
                        options.CompilerOptions = string.Format("/lib:\"{0}\"", path1) +
                                                  string.Format(" /lib:\"{0}\"", fileInfo.Directory.FullName);
                        list.Add(path1);
                        list.Add(fileInfo.Directory.FullName);
                    }
                    foreach (var assembly2 in assemblies)
                    {
                        var fileInfo = new FileInfo(assembly2.Location);
                        if (!list.Contains(fileInfo.DirectoryName))
                        {
                            var compilerParameters = options;
                            var str2 = compilerParameters.CompilerOptions +
                                       string.Format(" /lib:\"{0}\"", fileInfo.DirectoryName) + " ";
                            compilerParameters.CompilerOptions = str2;
                        }
                    }
                }
                options.ReferencedAssemblies.Add("System.dll");
                options.ReferencedAssemblies.Add("System.Core.dll");
                var flag = false;
                if (File.Exists(Path.Combine(path1, businessNamespace.Remove(businessNamespace.Length - 9) + ".dll")))
                {
                    flag = true;
                    var str2 = Path.Combine(path1, businessNamespace.Remove(businessNamespace.Length - 9) + ".dll");
                    options.ReferencedAssemblies.Add(str2);
                }
                foreach (var assembly2 in assemblies)
                    options.ReferencedAssemblies.Add(assembly2.Location);
                if (flag)
                {
                    options.ReferencedAssemblies.Add("Pyramid.Core.dll");
                }
                else
                {
                    var path = Path.Combine(path1, "Pyramid.Core.dll");
                    if (File.Exists(path))
                        options.ReferencedAssemblies.Add(path);
                }
                options.GenerateExecutable = false;
                options.GenerateInMemory = true;
                var compilerResults = csharpCodeProvider.CompileAssemblyFromSource(options, stringBuilder1.ToString());
                if (compilerResults.Errors.HasErrors)
                {
                    var stringBuilder2 = new StringBuilder();
                    stringBuilder2.Append("Error Compiling Expression: ");
                    foreach (CompilerError compilerError in compilerResults.Errors)
                        stringBuilder2.AppendFormat("{0}\n", compilerError.ErrorText);
                    throw new Exception("Error Compiling Expression: " + stringBuilder2);
                }
                var types = compilerResults.CompiledAssembly.GetTypes();
                if (types.Length == 1)
                    return types[0].GetMethod("ZZ" + str1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private static MethodInfo CreateScriptClassAll(Guid currentGuid, string businessNamespace, string linq,
            string objectName)
        {
            var str1 = currentGuid.ToString("N");
            var stringBuilder1 = new StringBuilder();
            stringBuilder1.AppendLine("using System;");
            stringBuilder1.AppendLine("using System.Linq;");
            stringBuilder1.AppendLine("using System.Linq.Expressions;");
            stringBuilder1.AppendLine("using " + businessNamespace + ".Objects;");
            stringBuilder1.AppendLine("using " + businessNamespace + ".LINQ;");
            stringBuilder1.AppendLine("namespace Widgetpshere.MemoryGeneration");
            stringBuilder1.AppendLine("{");
            stringBuilder1.AppendLine("public static class " + objectName + "Queries" + str1);
            stringBuilder1.AppendLine("{");
            stringBuilder1.AppendLine("    public static " + objectName + "Collection ZZ" + str1 + "()");
            stringBuilder1.AppendLine("    {");
            stringBuilder1.AppendLine("        return " + objectName + "Collection.RunSelect(" + linq + ");");
            stringBuilder1.AppendLine("    }");
            stringBuilder1.AppendLine("}");
            stringBuilder1.AppendLine("}");
            try
            {
                var provider = CodeDomProvider.CreateProvider("C#");
                var options = new CompilerParameters();
                var assembly = Assembly.LoadWithPartialName(businessNamespace.Remove(businessNamespace.Length - 9));
                string str2 = ConfigurationManager.AppSettings["AppServerBinDirectory"];
                if (string.IsNullOrEmpty(str2))
                    str2 = new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Directory.FullName;
                if (!string.IsNullOrEmpty(str2))
                {
                    if (!str2.ToLower().Contains("windows\\assembly\\gac_msil") &&
                        !str2.ToLower().Contains("microsoft.net\\assembly"))
                    {
                        options.CompilerOptions = string.Format("/lib:\"{0}\"", str2);
                    }
                    else
                    {
                        var fileInfo = new FileInfo(new Uri(assembly.CodeBase).LocalPath);
                        options.CompilerOptions = string.Format("/lib:\"{0}\"", str2) +
                                                  string.Format(" /lib:\"{0}\"", fileInfo.Directory.FullName);
                    }
                }
                options.ReferencedAssemblies.Add("System.dll");
                options.ReferencedAssemblies.Add("System.Core.dll");
                options.ReferencedAssemblies.Add(businessNamespace.Remove(businessNamespace.Length - 9) + ".dll");
                options.ReferencedAssemblies.Add("Pyramid.Core.dll");
                options.GenerateExecutable = false;
                options.GenerateInMemory = true;
                var compilerResults = provider.CompileAssemblyFromSource(options, stringBuilder1.ToString());
                if (compilerResults.Errors.HasErrors)
                {
                    var stringBuilder2 = new StringBuilder();
                    stringBuilder2.Append("Error Compiling Expression: ");
                    foreach (CompilerError compilerError in compilerResults.Errors)
                        stringBuilder2.AppendFormat("{0}\n", compilerError.ErrorText);
                    throw new Exception("Error Compiling Expression: " + stringBuilder2);
                }
                var types = compilerResults.CompiledAssembly.GetTypes();
                if (types.Length == 1)
                    return types[0].GetMethod("ZZ" + str1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}