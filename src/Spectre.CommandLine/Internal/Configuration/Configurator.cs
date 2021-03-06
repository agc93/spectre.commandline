﻿using System;
using System.Collections.Generic;

namespace Spectre.CommandLine.Internal.Configuration
{
    internal sealed class Configurator : IConfigurator, IConfiguration
    {
        private readonly ITypeRegistrar _registrar;

        public IList<ConfiguredCommand> Commands { get; }
        public string ApplicationName { get; private set; }

        public Configurator(ITypeRegistrar registrar)
        {
            _registrar = registrar;
            Commands = new List<ConfiguredCommand>();
        }

        public void SetApplicationName(string name)
        {
            ApplicationName = name;
        }

        public void AddCommand<TCommand>(string name) where TCommand : class, ICommand
        {
            var settingsType = ConfigurationHelper.GetSettingsType(typeof(TCommand));
            var command = new ConfiguredCommand(name, typeof(TCommand), settingsType);
            Commands.Add(command);

            // Register the command and the settings.
            _registrar?.Register(typeof(ICommand), typeof(TCommand));
            _registrar?.Register(typeof(TCommand), typeof(TCommand));
            _registrar?.Register(settingsType, settingsType);
        }

        public void AddCommand<TSettings>(string name, Action<IConfigurator<TSettings>> action)
            where TSettings : class
        {
            var command = new ConfiguredCommand(name, null, typeof(TSettings));
            action(new Configurator<TSettings>(command, _registrar));
            Commands.Add(command);
        }
    }
}
