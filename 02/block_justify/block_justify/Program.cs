using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace block_justify
{
    class Program
    {
        static int Length;
        static void Main(string[] args)
        {
            TextReader textReader;
            TextWriter textWriter;
            WordReader WordReader;

            /* Check if correct args were provided, continue if so, otherwise print out and exit. */
            if (ArgCheck(args) != 0)
            {
                Console.WriteLine("Argument Error");

                return;
            }

            /* Try to open input file for reading. If any error is encountered, print out and exit. */
            try
            {
                textReader = File.OpenText(args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine("File Error");

                return;
            }
            /* Try to open output file for writing. If any error is encountered, print out and exit. */
            try
            {
                textWriter = File.CreateText(args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine("File Error");

                return;
            }

            /* Creating new WordReader object. */
            WordReader = new WordReader(textReader);

            //I didn't have enough time to implement my object BlockWriter so I'm just using textWriter.
            BuildLines(WordReader, textWriter);

        }
        /// <summary>
        /// Checks, if given arguments are correct.
        /// </summary>
        /// <param name="args">Takes array containing program arguments.</param>
        /// <returns>0 if ok 1 if wrong.</returns>
        static int ArgCheck(string[] args)
        {
            if (args.Length < 3 || args.Length > 3)
            {
                return 1;
            }

            if (int.TryParse(args[2], out Program.Length))
            {
                return 0;
            }

            return 1;
        }

        static void BuildLines(WordReader WordReader, TextWriter textWriter)
        {
            List<string> Words = new List<string>();
            int LineSum = 0;


            while (true)
            {
                string currW = WordReader.GetWord();
                bool standart = WordReader.Trim();
                if (LineSum + (Words.Count - 1) + currW.Length + 1 < Length)
                {
                    //Is safe to add
                    Words.Add(currW);
                    LineSum += currW.Length;
                    continue;
                }
                else
                {
                    //Is not safe to add => I need to decide following
                    if (standart && !(Words[0].Length >= Length))
                    {
                        WriteIndented(textWriter, Words, Length - (LineSum + Words.Count - 1));
                    }
                    else
                    {
                        WriteNormal(textWriter, Words);
                        if (!WordReader.EOF)
                        {
                            textWriter.WriteLine();
                        }
                    }

                    //Clear the list for new words.
                    Words.Clear();

                    Words.Add(currW);
                }

                if (WordReader.EOF)
                {
                    break;
                }
            }
        }

        static void WriteIndented(TextWriter writer, List<string> lineList, int noSpaces)
        {
            StringBuilder bld = new StringBuilder();

            int actSpaces = 0;
            int diffSpaces = 0;

            if (lineList.Count > 1)
            {
                actSpaces = (noSpaces / (lineList.Count - 1)) * (lineList.Count - 1);
                diffSpaces = noSpaces - actSpaces;
            }
            else
            {
                diffSpaces = noSpaces;
            }
            

            for (int i = 0; i < lineList.Count; i++)
            {
                bld.Append(lineList[i]);
                if (i != lineList.Count)
                {
                    for (int j = 0; j < noSpaces / (lineList.Count - 1); j++)
                    {
                        bld.Append(' ');
                    }

                    if (i < diffSpaces)
                    {
                        bld.Append(' ');
                    }
                }
            }
            writer.WriteLine(bld.ToString());
        }

        static void WriteNormal(TextWriter writer, List<string> lineList)
        {
            StringBuilder bld = new StringBuilder();
            for (int i = 0; i < lineList.Count; i++)
            {
                bld.Append(lineList[i]);
                if(i != lineList.Count)
                {
                    bld.Append(' ');
                }
            }
            writer.WriteLine(bld.ToString());
        }
    }

    class WordReader : IDisposable
    {
        public TextReader Reader { get; private set; }
        public char[] Buffer { get; private set; }
        public const int BufferSize = 1024;
        /// <summary>
        /// If true, there are no other words to read from the buffer.
        /// </summary>
        public bool EOF = false;
        /// <summary>
        /// True if FillBuffer() found end of file. => no further FillBuffer() is possible.
        /// </summary>
        private bool EOFreached = false;
        /// <summary>
        /// Variable used by GetWord() to determine where inside a Buffer to look.
        /// </summary>
        private int CurrentIndex;
        /// <summary>
        /// Object constructor. Creates an object, that can read from file.
        /// </summary>
        /// <param name="reader">File opened for reading.</param>
        public readonly char[] Delimeters = { ' ', '\t', '\n' };
        public WordReader(TextReader reader)
        {
            this.Reader = reader;
            this.Buffer = new char[BufferSize];
            this.CurrentIndex = 0;
            FillBuffer();
            Trim();
        }
        /// <summary>
        /// Fills a buffer with chars. Sets EOF if encountered.
        /// </summary>
        public void FillBuffer()
        {
            this.Buffer = new char[BufferSize];
            int charsRead = Reader.ReadBlock(Buffer, 0, BufferSize);

            if (charsRead < BufferSize)
            {
                this.EOFreached = true;
            }
        }
        public string GetWord()
        {
            int index = this.CurrentIndex;
            StringBuilder word = new StringBuilder();

            while (true)
            {
                if (this.Buffer[index] == '\0')
                {
                    EOF = true;
                    break;
                }
                else if (!(Delimeters.Contains(this.Buffer[index])))
                {
                    word.Append(this.Buffer[index]);

                    index++;
                }                
                else
                {
                    break;
                }

                if (index >= BufferSize && !(EOFreached))
                {
                    index = 0;
                    FillBuffer();
                }
            }

            CurrentIndex = index;

            return word.ToString();
        }
        
        /// <summary>
        /// Trim skips the withe spaces before finding a bigining of an actual word.
        /// </summary>
        /// <returns>True if in further reading is safe. False if on the end of a paragraph.</returns>
        public bool Trim()
        {
            int index = CurrentIndex;
            int eolCounter = 0;
            bool theEnd = false;

            while (true)
            {
                if (this.Buffer[index] == ' ' || this.Buffer[index] == '\t')
                {
                    index++;
                }
                else if (this.Buffer[index] == '\n')
                {
                    eolCounter++;
                    index++;
                }
                else
                {
                    break;
                }
                
                if (index >= BufferSize)
                {
                    index = 0;
                    FillBuffer();
                }

                if (this.Buffer[index] == default(char))
                {
                    theEnd = true;
                    break;
                }
            }

            this.CurrentIndex = index;

            if (theEnd && EOFreached)
            {
                EOF = true;
            }

            if (eolCounter > 1)
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            this.Buffer = null;
        }
    }

    class BlockBuilder
    {
        public TextWriter Writer { get; private set; }
        public BlockBuilder(TextWriter writer)
        {
            this.Writer = writer;
        }
    }
}
