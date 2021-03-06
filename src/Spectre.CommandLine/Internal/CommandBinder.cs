using System;
using System.ComponentModel;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Parsing;

namespace Spectre.CommandLine.Internal
{
    internal sealed class CommandBinder
    {
        public void Bind(CommandTree tree, ref object obj, ITypeResolver resolver)
        {
            ValidateRequiredParameters(tree);

            TypeConverter GetConverter(CommandParameter parameter)
            {
                if (parameter.Converter == null)
                {
                    return TypeDescriptor.GetConverter(parameter.ParameterType);
                }
                var type = Type.GetType(parameter.Converter.ConverterTypeName);
                return resolver.Resolve(type) as TypeConverter;
            }

            while (tree != null)
            {
                // Process mapped parameters.
                foreach (var (parameter, value) in tree.Mapped)
                {
                    var converter = GetConverter(parameter);
                    parameter.Assign(obj, converter.ConvertFromInvariantString(value));
                }

                // Process unmapped parameters.
                foreach (var parameter in tree.Unmapped)
                {
                    // Is this an option with a default value?
                    if (parameter is CommandOption option && option.DefaultValue != null)
                    {
                        parameter.Assign(obj, option.DefaultValue.Value);
                    }
                }

                tree = tree.Next;
            }
        }

        private static void ValidateRequiredParameters(CommandTree tree)
        {
            var node = tree.GetRootCommand();
            while (node != null)
            {
                foreach (var parameter in node.Unmapped)
                {
                    if (parameter.Required)
                    {
                        switch (parameter)
                        {
                            case CommandOption option:
                                throw new CommandAppException($"Command '{node.Command.Name}' is missing required option '{option.GetOptionName()}'.");
                            case CommandArgument argument:
                                throw new CommandAppException($"Command '{node.Command.Name}' is missing required argument '{argument.Value}'.");
                        }
                    }
                }
                node = node.Next;
            }
        }
    }
}
