using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree {

    class Forest {

        public SortedSet<Tree> Trees { get; private set; }
        public int Time { get; private set; }

        /// <summary>
        /// Creates new instance of Forest class.
        /// </summary>
        public Forest() {

            Trees = new SortedSet<Tree>(new TreeComparer());
            Time = 0;
        }

        /// <summary>
        /// Adds new Tree to correct place in the forrest.
        /// </summary>
        /// <param name="sappling">Tree object to be planted.</param>
        public void PlantTree(int character, int occurance) {

            Tree sappling = new Tree(++this.Time, character, occurance);
            this.Trees.Add(sappling);
        }

        /// <summary>
        /// Takes in array describing occurances and converts it to forest.
        /// </summary>
        /// <param name="characterOcurrances">Array of character occurances in INPUT file.</param>
        public void PlantForest(int[] characterOcurrances) {

            for (int i = 0; i < characterOcurrances.Length; i++) {

                if (characterOcurrances[ i ] > 0) {

                    PlantTree(i, characterOcurrances[ i ]);
                }
            }
        }
        /// <summary>
        /// Get Tree with lowest value from the forest.
        /// Remove it from the forest afterwards.
        /// </summary>
        /// <returns>Tree object.</returns>
        public Tree ExtractSmallestTree() {

            Tree smallest = this.Trees.First<Tree>();

            this.Trees.Remove(smallest);

            return smallest;
        }

        /// <summary>
        /// Connect trees to eachother until only one super tree remains.
        /// </summary>
        /// <returns>Huffmans tree of text contained in INPUT file.</returns>
        public void GrowHuffmansTree() {

            while (this.Trees.Count > 1) {

                Tree firstTree = ExtractSmallestTree();
                Tree secondTree = ExtractSmallestTree();

                Tree mergedTree = new Tree(++this.Time, firstTree, secondTree);

                this.Trees.Add(mergedTree);
            }
        }

        /// <summary>
        /// Disposes of this objects properties.
        /// </summary>
        public void Dispose() {

            this.Trees = null;
        }
    }
}
