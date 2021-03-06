﻿using System;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CommandOptionAttribute : Attribute
    {
        public string LongName { get; }
        public string ShortName { get; }
        public string ValueName { get; }
        public bool IsRequired { get; }

        public CommandOptionAttribute(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var parts = template.Split(new[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (part.StartsWith("--", StringComparison.OrdinalIgnoreCase))
                {
                    if (part.Length > 3)
                    {
                        LongName = part.Substring(2);
                        continue;
                    }
                    throw new CommandAppException("Invalid long option.");
                }
                if (part.StartsWith("-", StringComparison.OrdinalIgnoreCase))
                {
                    if (part.Length == 2)
                    {
                        ShortName = part.Substring(1);
                        continue;
                    }
                    throw new CommandAppException("Invalid short option.");
                }
                if (IsRequiredOrOptionalArgument(part) && part.Length > 2)
                {
                    ValueName = TrimArgument(part);
                    IsRequired = IsRequiredArgument(part);
                    continue;
                }

                throw new InvalidOperationException("Invalid template pattern.");
            }

            if (string.IsNullOrWhiteSpace(ShortName) && string.IsNullOrWhiteSpace(LongName))
            {
                throw new InvalidOperationException("No long or short name for option has been specified.");
            }
        }

        private static bool IsRequiredOrOptionalArgument(string text)
        {
            if (text != null)
            {
                return (text.StartsWith("[", StringComparison.OrdinalIgnoreCase) && text.EndsWith("]", StringComparison.OrdinalIgnoreCase)) ||
                       (text.StartsWith("<", StringComparison.OrdinalIgnoreCase) && text.EndsWith(">", StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }

        private static bool IsRequiredArgument(string text)
        {
            if (text != null)
            {
                return text.StartsWith("<", StringComparison.OrdinalIgnoreCase) && text.EndsWith(">", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private static string TrimArgument(string text)
        {
            return text?.TrimStart('[', '<')?.TrimEnd(']', '>');
        }
    }
}
