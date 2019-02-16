using System;
using System.Collections.Generic;

namespace Assumptions
{
    internal class UsualityDeclarations
    {
        private static readonly Dictionary<Int64, UsualityDeclaration> usualityDeclarations =
            new Dictionary<Int64, UsualityDeclaration>();

        public static UsualityDeclaration FindOrCreateUsabilityDeclaration(ProbabiltyFunc probability, SourceCodeLocation sourceCodeLocation)
        {
            lock (usualityDeclarations)
            {
                var key = sourceCodeLocation.Key;
                if (!usualityDeclarations.TryGetValue(key, out var declaration))
                {
                    usualityDeclarations[key] = declaration = new UsualityDeclaration
                    {
                        Probability = probability,
                        SourceCodeLocation = sourceCodeLocation
                    };
                }

                return declaration;
            }
        }
    }
}