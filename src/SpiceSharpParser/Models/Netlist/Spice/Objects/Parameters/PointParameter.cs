﻿using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace SpiceSharpParser.Models.Netlist.Spice.Objects.Parameters
{
    /// <summary>
    /// A point parameter.
    /// </summary>
    public class PointParameter : Parameter
    {
        /// <summary>
        /// Gets or sets the elements of the point.
        /// </summary>
        public PointValues Values { get; set; }

        /// <summary>
        /// Gets the string representation of the point.
        /// </summary>
        public override string Image
        {
            get
            {
                return $"({string.Join(",", Values.Items.Select(v => v.Image).ToArray())})";
            }
        }

        /// <summary>
        /// Clones the object.
        /// </summary>
        /// <returns>A clone of the object.</returns>
        public override SpiceObject Clone()
        {
            var result = new PointParameter { Values = (PointValues) Values.Clone() };

            return result;
        }
    }
}
