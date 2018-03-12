﻿using SpiceNetlist.SpiceSharpConnector.Processors;
using SpiceNetlist.SpiceSharpConnector.Processors.EntityGenerators.Models;
using SpiceNetlist.SpiceSharpConnector.Processors.Waveforms;

namespace SpiceNetlist.SpiceSharpConnector.Registries
{
    /// <summary>
    /// Interface for all entity generator registries
    /// </summary>
    public interface IEntityGeneratorRegistry
    {
        EntityGenerator Get(string type);

        /// <summary>
        /// Gets a value indicating whether a specified entity generator is in registry
        /// </summary>
        /// <param name="type">Type of exporter</param>
        /// <returns>
        /// A value indicating whether a specified entity generator is in registry
        /// </returns>
        bool Supports(string type);

        /// <summary>
        /// Adds an entity generator to registy
        /// </summary>
        /// <param name="generator">
        /// A generator to add
        /// </param>
        void Add(EntityGenerator generator);
    }
}
