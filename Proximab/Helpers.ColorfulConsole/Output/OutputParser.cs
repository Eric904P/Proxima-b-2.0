﻿using System;
using System.Collections.Generic;

namespace Helpers.ColorfulConsole.Output
{
    /// <summary>
    /// Represents a set of methods to parse the formatted text into the output chunks.
    /// </summary>
    public class OutputParser
    {
        private readonly char[] _separators = { '$' };

        /// <summary>
        /// Calculates output chunks by splitting the text and parsing color symbols.
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>
        /// The list of chunks (split output text with assigned color enums) which can
        /// be easily printed on the console.
        /// </returns>
        public IList<OutputChunk> GetOutputChunks(string text)
        {
            var outputChunks = new List<OutputChunk>();
            var splitOutput = text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (var chunk in splitOutput)
            {
                var colorSymbol = chunk[0].ToString();
                var content = chunk.Remove(0, 1);

                var colorType = ColorDefinitions.Colors[Convert.ToChar(colorSymbol)];

                outputChunks.Add(new OutputChunk(colorType, content));
            }

            return outputChunks;
        }
    }
}
