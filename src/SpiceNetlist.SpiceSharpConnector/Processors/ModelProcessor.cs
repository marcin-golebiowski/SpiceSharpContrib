﻿using SpiceNetlist.SpiceObjects;
using SpiceNetlist.SpiceObjects.Parameters;
using SpiceSharp.Circuits;

namespace SpiceNetlist.SpiceSharpConnector.Processors
{
    /// <summary>
    /// Processes all supported <see cref="Model"/> from spice netlist object model.
    /// </summary>
    public class ModelProcessor : StatementProcessor<Model>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelProcessor"/> class.
        /// </summary>
        /// <param name="registry">The registry</param>
        public ModelProcessor(EntityGeneratorRegistry registry)
        {
            Registry = registry;
        }

        /// <summary>
        /// Gets the registry
        /// </summary>
        public EntityGeneratorRegistry Registry { get; }

        /// <summary>
        /// Processes a model statement and modifies the context
        /// </summary>
        /// <param name="statement">A statement to process</param>
        /// <param name="context">A context to modifify</param>
        public override void Process(Model statement, ProcessingContext context)
        {
            string name = statement.Name.ToLower();
            if (statement.Parameters.Count > 0)
            {
                if (statement.Parameters[0] is BracketParameter b)
                {
                    var type = b.Name.ToLower();

                    if (!Registry.Supports(type))
                    {
                        throw new System.Exception("Unsupported model type");
                    }
                    var generator = Registry.Get(type);

                    Entity spiceSharpModel = generator.Generate(
                        new SpiceSharp.Identifier(context.GenerateObjectName(name)),
                        name,
                        type,
                        b.Parameters,
                        context);

                    if (spiceSharpModel != null)
                    {
                        context.AddEntity(spiceSharpModel);
                    }
                }

                if (statement.Parameters[0] is SingleParameter single)
                {
                    var type = single.Image;

                    if (!Registry.Supports(type))
                    {
                        throw new System.Exception("Unsupported model type");
                    }

                    var generator = Registry.Get(type);
                    Entity spiceSharpModel = generator.Generate(new SpiceSharp.Identifier(context.GenerateObjectName(name)), name, type, statement.Parameters.Skip(1), context);

                    if (spiceSharpModel != null)
                    {
                        context.AddEntity(spiceSharpModel);
                    }
                }
            }
        }
    }
}