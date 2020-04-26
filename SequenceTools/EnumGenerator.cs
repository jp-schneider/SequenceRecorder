using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Linq.Expressions;
using Sequence.Recorder.Enums;
using Sequence.Recorder.Events;
using System.Windows;

namespace Sequence.Tools
{
    public class EnumGenerator
    {
        private static EnumGenerator _instance;
        public static EnumGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnumGenerator();
                }
                return _instance;
            }
        }

        private EnumGenerator()
        {

        }

        public string GenerateCSharpCode(CodeCompileUnit compileunit, string fileName)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            string sourceFile = fileName + "." + provider.FileExtension;

            // Create Output
            using (StreamWriter sw = new StreamWriter(sourceFile, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                provider.GenerateCodeFromCompileUnit(compileunit, tw, new CodeGeneratorOptions());
                tw.Close();
            }

            return sourceFile;
        }

        public void CreateEnum(string namesp, string name, List<EnumDefinition> enumMemberDefinition, string fileName)
        {
            var unit = EnumCompileUnit(namesp, name, enumMemberDefinition);
            GenerateCSharpCode(unit, fileName);
        }

        public CodeCompileUnit EnumCompileUnit(string namesp, string name, List<EnumDefinition> enumMemberDefinition)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace @namespace = new CodeNamespace(namesp);
            compileUnit.Namespaces.Add(@namespace);
            @namespace.Imports.Add(new CodeNamespaceImport("System"));
            CodeTypeDeclaration nameenum = new CodeTypeDeclaration(name);
            nameenum.IsEnum = true;
            // Add the new type to the namespace type collection.
            @namespace.Types.Add(nameenum);
            CodeMemberField @default = new CodeMemberField(name, "UNKNOWN");
            @default.InitExpression = new CodePrimitiveExpression(0);
            nameenum.Members.Add(@default);
            List<string> memberNames = new List<string>();
            foreach (var e in enumMemberDefinition)
            {
                string append = "";
                if (memberNames.Contains(e.Name))
                {
                    append = "_" + e.Description.EventHandler.Name;
                }
                CodeMemberField member = new CodeMemberField(name, e.Name + append);
                member.InitExpression = new CodePrimitiveExpression(e.Value);

                var desc = new CodeAttributeDeclaration
                {
                    Name = typeof(EventDescription).Name,
                    Arguments =
                    {
                        new CodeAttributeArgument() {
                            Value = new CodeArrayCreateExpression(new CodeTypeReference(typeof(Type)), e.Description.DeclaringTypes.Select(i => new CodeTypeOfExpression(i)).ToArray())
                        },
                        new CodeAttributeArgument()
                        {
                            Value = new CodePrimitiveExpression(e.Description.EventHandlerPropertyName)
                        },
                        new CodeAttributeArgument()
                        {
                            Value = new CodeTypeOfExpression(e.Description.EventHandler)
                        },
                        new CodeAttributeArgument()
                        {
                            Value = new CodeTypeOfExpression(e.Description.EventArgsType)
                        }
                    }
                };
                if (!String.IsNullOrWhiteSpace(e.Description.Description))
                {
                    desc.Arguments.Add(new CodeAttributeArgument()
                    {
                        Value = new CodePrimitiveExpression(e.Description.Description)
                    });
                }
                member.CustomAttributes.Add(desc);
                AddDescription(member, "This value can be used for all elements that derive from or declare at least one of the following types:" + String.Join(String.Empty, e.Description.DeclaringTypes.Select(i => " " + i.Name + ",").ToArray()).TrimEnd(new char[] { ',' }) + ".");
                memberNames.Add(member.Name);
                nameenum.Members.Add(member);
            };

            //Add Type Attribute
            var enumDec = new CodeAttributeDeclaration()
            {
                Name = typeof(EventDescription).Name,
                Arguments = {

                }
            };
            return compileUnit;
        }

        public void AddDescription(CodeTypeMember member, string text)
        {
            member.Comments.Add(new CodeCommentStatement("<summary>", true));
            member.Comments.Add(new CodeCommentStatement(text, true));
            member.Comments.Add(new CodeCommentStatement("</summary>", true));
        }

        public static void GenerateEventEnum()
        {
            EnumGenerator.Instance.CreateEnum("Sequence.Recorder.Enums", "EventType2", GetEnumDefinitions(), "../../../SequenceRecorder/Enums/Event");
        }

        public static List<EnumDefinition> GetEnumDefinitions()
        {
            var result = new List<EnumDefinition>();
            var Types = new List<Type>()
            {
                typeof(UIElement) //Need to add UIElement, because FrameworkElement inheritates from UIElement and in declares a lot of events.
            };
            Types = Types.Union(GetFrameworkElementTypes()).ToList();
            Types.ForEach(i =>
            {
                int index = result.Count() + 1;
                var definitions = GetEventDefinitionsForType(i, index);
                var intersec = definitions.Intersect(result, EnumDefinitionComparer.Instance).ToList();
                foreach (var dublicate in intersec)
                {
                    var original = result
                        .Where(item => EnumDefinitionComparer.Instance.Equals(item, dublicate))
                        .FirstOrDefault();
                    if (!original.Description.DeclaringTypes.Contains(dublicate.Description.DeclaringType))
                    {
                        //Dont Add if Declaring type is inheritated from one of the included types.
                        foreach (var type in original.Description.DeclaringTypes)
                        {
                            if (type.IsAssignableFrom(dublicate.Description.DeclaringType))
                            {
                                break;
                            }
                        }
                        original.Description.DeclaringTypes.Add(dublicate.Description.DeclaringType);
                    }
                };
                definitions = definitions.Except(intersec).ToList();
                result = result.Union(definitions).ToList();
            });
            return result;
        }

        public static List<EnumDefinition> GetEventDefinitionsForType(Type type, int startvalue = 0)
        {
            List<EnumDefinition> result = new List<EnumDefinition>();
            List<EventInfo> events = type.GetEvents().ToList();
            int index = startvalue;
            foreach (var @event in events)
            {
                if (@event != null)
                {
                    Type args = null;
                    if (@event.EventHandlerType.IsGenericType)
                    {
                        args = @event.EventHandlerType
                        .GetGenericArguments()[0];
                    }
                    else
                    {
                        args = @event.EventHandlerType.GetMethod("Invoke")
                        .GetParameters()[1].ParameterType;
                    }
                    var description = new EventDescription(
                                    new Type[] { @event.DeclaringType },
                                    @event.Name, @event.EventHandlerType,
                                    args,
                                    "Enum for the " + @event.Name + " Event.");
                    var definition = new EnumDefinition(
                                    @event.Name,
                                    index,
                                    description);
                    result.Add(definition);
                    index++;
                }
            };
            return result;
        }

        public static List<Type> GetFrameworkElementTypes()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(FrameworkElement));
            var typeList = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(FrameworkElement))
                || t.Equals(typeof(FrameworkElement)) && t.IsPublic)
                .ToList();
            return typeList;
        }
    }
}
