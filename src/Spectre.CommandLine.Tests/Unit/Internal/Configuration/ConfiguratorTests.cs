﻿using Shouldly;
using Spectre.CommandLine.Internal.Configuration;
using Spectre.CommandLine.Tests.Data;
using Xunit;

namespace Spectre.CommandLine.Tests.Unit.Internal.Configuration
{
    public sealed class ConfiguratorTests
    {
        [Fact]
        public void Should_Create_Configured_Commands_Correctly()
        {
            // Given
            var configurator = new Configurator(null);
            configurator.AddCommand<AnimalSettings>("animal", animal =>
            {
                animal.AddCommand<MammalSettings>("mammal", mammal =>
                {
                    mammal.AddCommand<DogCommand>("dog");
                    mammal.AddCommand<HorseCommand>("horse");
                });
            });

            // When
            var commands = configurator.Commands;

            // Then
            commands.Count.ShouldBe(1);
            commands[0].As(animal =>
            {
                animal.ShouldBeProxy<AnimalSettings>();
                animal.Children.Count.ShouldBe(1);

                animal.Children[0].As(mammal =>
                {
                    mammal.ShouldBeProxy<MammalSettings>();
                    mammal.Children.Count.ShouldBe(2);

                    mammal.Children[0].As(dog =>
                    {
                        dog.ShouldBeCommand<DogCommand, DogSettings>();
                        dog.Children.Count.ShouldBe(0);
                    });

                    mammal.Children[1].As(horse =>
                    {
                        horse.ShouldBeCommand<HorseCommand, MammalSettings>();
                        horse.Children.Count.ShouldBe(0);
                    });
                });
            });
        }
    }
}
