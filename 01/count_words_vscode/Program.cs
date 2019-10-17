using System;
using System.IO;

namespace count_words
{
    class Program
    {
        static void Main(string[] args) {
        
            //open file given as args[1] for reading
            //streamreader...
            string source = args[1];
            bool word = false;
            int count = 0;

            using (StreamReader Str = new StreamReader(source)) {

                char c = null;

                while (c = (char)Str.Read() > -1) {

                    if (!(c == ' ' || c == '\t' || c == '\n')) {

                        word = true;
                    }
                    else {

                        word = false;
                        count++;
                    }
                }
            }

        }
    }
}
