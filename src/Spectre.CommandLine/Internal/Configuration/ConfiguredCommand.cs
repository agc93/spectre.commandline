﻿using System;
using System.Collections.Generic;

namespace Spectre.CommandLine.Internal.Configuration
{
    internal sealed class ConfiguredCommand
    {
        public string Name { get; }
        public string Description { get; set; }
        public Type CommandType { get; }
        public Type SettingsType { get; }

        public IList<ConfiguredCommand> Children { get; }

        public ConfiguredCommand(string name, Type commandType, Type settingsType)
        {
            Name = name;
            CommandType = commandType;
            SettingsType = settingsType;
            Children = new List<ConfiguredCommand>();
        }
    }
}
