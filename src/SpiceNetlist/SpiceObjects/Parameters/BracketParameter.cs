﻿using System.Text;

namespace SpiceNetlist.SpiceObjects.Parameters
{
    /// <summary>
    /// A bracket parameter
    /// </summary>
    public class BracketParameter : Parameter
    {
        /// <summary>
        /// Gets or sets the name of the bracket parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameters inside the bracket
        /// </summary>
        public ParameterCollection Parameters { get; set; }

        /// <summary>
        /// Gets the string represenation of the bracket parameter
        /// </summary>
        public override string Image
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                if (Parameters.Count > 0)
                {
                    builder.Append(Name + "(");

                    for (var i = 0; i < Parameters.Count; i++)
                    {
                        builder.Append(Parameters[i].Image);

                        if (i != Parameters.Count - 1)
                        {
                            builder.Append(",");
                        }
                    }

                    builder.Append(")");
                }

                return builder.ToString();
            }
        }
    }
}