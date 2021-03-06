﻿using System.ComponentModel;
using Spectre.CommandLine.Tests.Data.Converters;

namespace Spectre.CommandLine.Tests.Data
{
    public sealed class CatSettings : MammalSettings
    {
        [CommandOption("--agility <VALUE>")]
        [TypeConverter(typeof(CatAgilityConverter))]
        [DefaultValue(10)]
        [Description("The option description.")]
        public int Agility { get; set; }
    }
}