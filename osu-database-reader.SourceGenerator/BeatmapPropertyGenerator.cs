using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Tommy;

namespace osu_database_reader.SourceGenerator
{
    [Generator]
    public class BeatmapPropertyGenerator : ISourceGenerator
    {
        private TomlTable document;

        public void Initialize(GeneratorInitializationContext context)
        {
            var asm = Assembly.GetExecutingAssembly();
            using var stream = asm.GetManifestResourceStream("osu_database_reader.SourceGenerator.Data.BeatmapProperties.toml");
            using var sr = new StreamReader(stream!);
            document = TOML.Parse(sr);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using osu_database_reader;");
            sb.AppendLine($"namespace {document["namespace"]} {{ ");
            sb.AppendLine($"partial class {document["class"]} {{ ");

            foreach (var kvp in document.RawTable.Where(x => x.Value is TomlTable))
            {
                var section = kvp.Key;
                var items = (TomlTable)kvp.Value;

                sb.AppendLine($"\t// Section {section}");
                foreach (var kvpInner in items.RawTable)
                {
                    var propName = kvpInner.Key;
                    var propType = kvpInner.Value.AsString.ToString();

                    var (type, getter) = GenerateSourceParts(section, propName, propType);
                    sb.AppendLine("\t" + $"public {type} {propName} => {getter};");
                }

                sb.AppendLine();
            }

            sb.AppendLine("}");
            sb.AppendLine("}");

            context.AddSource("BeatmapProperties.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private static (string type, string getter) GenerateSourceParts(string section, string propName, string propType)
        {
            string type;
            string getter;
            string dictionaryName = "Section" + section;

            switch (propType)
            {
                case "string":
                    type = "string";
                    getter = $"{dictionaryName}.GetValueOrNull(\"{propName}\")";
                    break;
                case "int":
                    type = "int";
                    getter = $"int.Parse({dictionaryName}.GetValueOrNull(\"{propName}\"), Constants.NumberFormat)";
                    break;
                case "float":
                    type = "float";
                    getter = $"float.Parse({dictionaryName}.GetValueOrNull(\"{propName}\"), Constants.NumberFormat)";
                    break;
                case "double":
                    type = "double";
                    getter = $"double.Parse({dictionaryName}.GetValueOrNull(\"{propName}\"), Constants.NumberFormat)";
                    break;
                case "intbool":
                    type = "bool";
                    getter = $"int.Parse({dictionaryName}.GetValueOrNull(\"{propName}\"), Constants.NumberFormat) == 1";
                    break;
                case "int[]":
                    type = "int[]";
                    getter = $"{dictionaryName}.GetValueOrNull(\"{propName}\")" +
                             ".Split(',')" +
                             ".Select(x => int.Parse(x, Constants.NumberFormat))" +
                             ".ToArray()";
                    break;
                default:
                    throw new NotImplementedException("Unknown beatmap property type: " + propType);
            }

            return (type, getter);
        }
    }
}