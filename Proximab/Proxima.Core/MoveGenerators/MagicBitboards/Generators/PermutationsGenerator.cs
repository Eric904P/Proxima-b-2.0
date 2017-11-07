﻿using Proxima.Core.Boards;
using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Generators
{
    public class PermutationsGenerator
    {
        public List<ulong> GetMaskPermuatations(ulong mask)
        {
            var permuatations = new List<ulong>();
            var bitIndexes = GetBitIndexes(mask);
            var permutationsCount = 1 << bitIndexes.Count;

            for (int i = 0; i<permutationsCount; i++)
            {
                permuatations.Add(CalculatePermutation(i, bitIndexes));
            }

            return permuatations;
        }

        List<int> GetBitIndexes(ulong mask)
        {
            var bitIndexes = new List<int>();

            while(mask != 0)
            {
                var lsb = BitOperations.GetLSB(ref mask);
                var index = BitOperations.GetBitIndex(lsb);

                bitIndexes.Add(index);
            }

            return bitIndexes;
        }

        ulong CalculatePermutation(int permutationIndex, List<int> bitIndexes)
        {
            var permutation = 0ul;

            while(permutationIndex != 0)
            {
                var lsb = BitOperations.GetLSB(ref permutationIndex);
                var lsbIndex = BitOperations.GetBitIndex((ulong)lsb);

                permutation |= 1ul << bitIndexes[lsbIndex];
            }

            return permutation;
        }
    }
}