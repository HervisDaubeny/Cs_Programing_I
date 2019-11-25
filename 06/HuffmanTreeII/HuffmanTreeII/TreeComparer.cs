using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTreeII {
    public class TreeComparer : IComparer<Tree> {
        /// <summary>
        /// Defines default way to compare two Trees.
        /// Firstly by their weight and by their time of creation, if weights are the same.
        /// </summary>
        /// <param name="firstTree"></param>
        /// <param name="secondTree"></param>
        /// <returns>-1 if "firstTree < secondTree", 0 if they are equal and 1 if "rightTree < leftTree"</returns>
        public int Compare(Tree firstTree, Tree secondTree) {
            int weightCompare = firstTree.Weight.CompareTo(secondTree.Weight);
            if (weightCompare != 0) {
                return weightCompare;
            }
            if (firstTree.Type.CompareTo(secondTree.Type) != 0) {
                if (firstTree.Type == Tree.TreeType.Leaf) {
                    return -1;
                }
                return 1;
            }
            return firstTree.TimeOfCreation.CompareTo(secondTree.TimeOfCreation);
        }
    }
}
