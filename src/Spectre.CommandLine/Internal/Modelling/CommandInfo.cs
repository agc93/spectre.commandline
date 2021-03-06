using System;
using System.Collections.Generic;
using Spectre.CommandLine.Internal.Configuration;

namespace Spectre.CommandLine.Internal.Modelling
{
    internal sealed class CommandInfo : ICommandContainer
    {
        public string Name { get; }
        public string Description { get; set; }
        public Type CommandType { get; }
        public Type SettingsType { get; }
        public CommandInfo Parent { get; }
        public IList<CommandInfo> Children { get; }
        public IList<CommandParameter> Parameters { get; }

        public bool IsProxy => CommandType == null;
        IList<CommandInfo> ICommandContainer.Commands => Children;

        public CommandInfo(CommandInfo parent, ConfiguredCommand prototype)
        {
            Parent = parent;

            Name = prototype.Name;
            Description = prototype.Description;
            CommandType = prototype.CommandType;
            SettingsType = prototype.SettingsType;

            Children = new List<CommandInfo>();
            Parameters = new List<CommandParameter>();
        }
    }
}
