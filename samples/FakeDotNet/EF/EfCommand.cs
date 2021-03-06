﻿using System;
using System.Linq;
using Spectre.CommandLine;

namespace FakeDotNet.EF
{
    public abstract class EfCommand<TSettings> : Command<TSettings>
        where TSettings : EfCommandSettings
    {
        protected void DumpSettings(TSettings settings, ILookup<string, string> remaining)
        {
            var properties = settings.GetType().GetProperties();
            foreach (var group in properties.GroupBy(x => x.DeclaringType).Reverse())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{group.Key.FullName}");
                Console.ResetColor();

                foreach (var property in group)
                {
                    Console.WriteLine($"  {property.Name} = {property.GetValue(settings)}");
                }
            }

            if (remaining.Count > 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Remaining:");
                Console.ResetColor();
                foreach (var item in remaining)
                {
                    var value = string.Join(",", item);
                    Console.WriteLine(!string.IsNullOrWhiteSpace(value) ? $"  {item.Key} = {value}" : $"  {item.Key}");
                }
            }

            Console.WriteLine();
        }
    }
}
