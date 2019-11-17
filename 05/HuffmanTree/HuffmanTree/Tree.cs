using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree {
    
    public class Tree {

        public string Value { get; private set; }
        public int TimeOfCreation { get; private set; }
        public int Weight { get; private set; }
        public TreeType Type { get; private set; }
        public Tree LeftSon { get; private set; }
        public Tree RightSon { get; private set; }


        /// <summary>
        /// Creates new leaf.
        /// </summary>
        /// <param name="character">Number of character to be represented by this leaf.</param>
        /// <param name="weight">Number of occurances of character.</param>
        public Tree( int originTime, int character, int weight ) {

            this.TimeOfCreation = originTime;
            this.Value = convertLeafValueToString(character, weight);
            this.Weight = weight;
        }

        /// <summary>
        /// Creates new root.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="originTime"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public Tree( int originTime, Tree left, Tree right) {

            this.Type = TreeType.Knot;
            this.Value = convertRootValueToString(left.Weight, right.Weight);
            this.Weight = left.Weight + right.Weight;
            this.TimeOfCreation = originTime;
            this.LeftSon = left;
            this.RightSon = right;
        }

        /// <summary>
        /// Creates new Tree object of root type.
        /// Appends firstTree as left son and secondTree as right.
        /// </summary>
        /// <param name="firstTree">Tree to become left subtree of new Tree.</param>
        /// <param name="secondTree">Tree to become right subtree of new Tree.</param>
        /// <param name="originTime">Step of treebuilding algorithm describing when this Tree will have been created.</param>
        /// <returns></returns>
        public Tree MergeTrees( int originTime, Tree firstTree, Tree secondTree) {

            Tree iAmRoot = new Tree(originTime, firstTree, secondTree);

            return iAmRoot;
        }

        /// <summary>
        /// Describes position of vertex in tree strucrure.
        /// </summary>
        public enum TreeType {

            Leaf,
            Knot
        }

        /// <summary>
        /// Convert int values to one string.
        /// </summary>
        /// <param name="character">Number of character.</param>
        /// <param name="occurances">Number of occurances of character in INPUT file.</param>
        /// <returns>String in format required for leaf.</returns>
        public string convertLeafValueToString(int character, int occurances) {

            return "*" + character.ToString() + ":" + occurances.ToString();
        }

        /// <summary>
        /// Convert int values to one string.
        /// </summary>
        /// <param name="firstWeight">Weight of first tree.</param>
        /// <param name="secondWeight">Weighr of second tree.</param>
        /// <returns>String in format required for root.</returns>
        public string convertRootValueToString(int firstWeight, int secondWeight) {

            return ( firstWeight + secondWeight ).ToString();
        }
    }
}
