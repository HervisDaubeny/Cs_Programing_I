using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace justify_reloaded
{
    public static class Program
    {

        public static void Main(string[] args) {


            /* TODO
             * 
             * for output create these methods
             * PrintFormatedLine() #print line with special ' ' padding
             * PrintLine() #print line without any indentation
             * PrintEmptyLine() #indents block; then it is neccesary to set myWordReader.TrimState = default instead of EOL
             * 
             */
            int size = 0;
            bool endOfFile = false;

            //object 
            MyWordReader myWordReader;
            MyWordWriter myWordWriter;

            //arg check
            //in file
            try {

                myWordReader = new MyWordReader(File.OpenText(args[ 0 ]));
            }
            catch (Exception e) {

                Console.WriteLine("File Error");

                return;
            }

            //line length
            if(args.Length < 3 || args.Length > 3) {

                Console.WriteLine("Argument Error");

                return;
            }
            else {

                bool result = int.TryParse(args[ 2 ], out size);

                if (!result) {

                    Console.WriteLine("Argument Error");

                    return;
                }

                if (size < 1) {

                    Console.WriteLine("Argument Error");

                    return;
                }
            }

            //outfile
            try {

                myWordWriter = new MyWordWriter(File.CreateText(args[ 1 ]), size);
            }
            catch (Exception e) {

                Console.WriteLine("File Error");

                return;
            }

            //try run
            List<string> words = new List<string>();
            int currentLength = 0;

            while (true) {


                string newWord = "";
                bool wordRemains = false;

                newWord =  myWordReader.GetWord();
                myWordReader.Trim();

                if (currentLength + newWord.Length + words.Count <= size) {

                    words.Add(newWord);
                    currentLength += newWord.Length;
                }
                else {
                    wordRemains = true;

                    // print too long or single word
                    if (words.Count < 2 && myWordReader.trimState == MyWordReader.TrimState.Default) {

                        myWordWriter.PrintNormalLine(words);

                        words.Clear();
                        currentLength = 0;
                    }
                    else if (words.Count > 1 && !(myWordReader.trimState == MyWordReader.TrimState.EOF /*|| myWordReader.trimState == MyWordReader.TrimState.EOLF*/ || myWordReader.getWordState == MyWordReader.GetWordState.EOF)) {

                        myWordWriter.PrintFormatedLine(words, currentLength);

                        words.Clear();

                        words.Add(newWord);
                        currentLength = newWord.Length;
                    }
                }
                
                // print on the end of block
                if (myWordReader.trimState == MyWordReader.TrimState.EOL) {

                    if (wordRemains && words.Count > 1) {

                        myWordWriter.PrintFormatedLine(words, currentLength);

                        words.Clear();

                        words.Add(newWord);
                        currentLength = newWord.Length;
                    }
                    else if (wordRemains && words.Count < 2) {

                        myWordWriter.PrintNormalLine(words);

                        words.Clear();

                        words.Add(newWord);
                        currentLength = newWord.Length;
                    }
                    myWordWriter.PrintNormalLine(words);
                    myWordWriter.PrintEmptyLine();

                    words.Clear();
                    currentLength = 0;
                    myWordReader.ResetTrimState();
                }
                
                //end of file detection
                if (myWordReader.trimState == MyWordReader.TrimState.EOF/* || myWordReader.trimState == MyWordReader.TrimState.EOLF*/) {

                    endOfFile = true;
                }

                if (myWordReader.getWordState == MyWordReader.GetWordState.EOF) {

                    endOfFile = true;
                }

                //print on end of file
                if (endOfFile) {

                    if (wordRemains) {

                        if (words.Count > 1) {

                            myWordWriter.PrintFormatedLine(words, currentLength);
                        }
                        else {

                            if (currentLength > 0) {

                                myWordWriter.PrintNormalLine(words);
                            }
                        }

                        words.Clear();

                        words.Add(newWord);
                        currentLength = newWord.Length;
                    }
                    if (currentLength > 0) {

                        myWordWriter.PrintNormalLine(words);
                    }

                    words.Clear();
                    currentLength = 0;

                    myWordWriter.Flush();
                    break;
                }
            }

            //Console.ReadKey();
            //return;
        }
    }

    class MyWordReader {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private char[] Buffer;

        /// <summary>
        /// 
        /// </summary>
        private const int BufferSize = 1024;

        /// <summary>
        /// 
        /// </summary>
        public TrimState trimState { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public FillBufferState fillBufferState { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public GetWordState getWordState { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentBufferSize { get; private set; } //do I need this?

        /// <summary>
        /// 
        /// </summary>
        public int BufferIndex { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private TextReader textReader;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textReader"></param>
        public MyWordReader(TextReader textReader) {

            this.textReader = textReader;
            this.getWordState = GetWordState.Default;
            this.fillBufferState = FillBufferState.Default;
            this.fillBufferState = FillBufferState.Default;

            FillBuffer();
            Trim();
            this.trimState = TrimState.Default;
        }

        /// <summary>
        /// Reads 1024 (if possible) chars from currently opened file.
        /// </summary>
        public void FillBuffer() {

            //might want to change to return data type / dont yet know which
            int count = 0;
            Buffer = new char[ BufferSize ];

            count = this.textReader.ReadBlock(this.Buffer, 0, BufferSize);

            if (count < BufferSize) {

                this.fillBufferState = FillBufferState.EOB;

                return;
            }

            this.fillBufferState = FillBufferState.IN;

        }
        public string GetWord() {

            int index = this.BufferIndex;
            char[] whites = new char[] { ' ', '\n', '\t' };
            GetWordState state = GetWordState.Default;
            StringBuilder word = new StringBuilder();

            while (true) {

                //Check index out of range.
                if (index >= BufferSize) {

                    FillBuffer();
                    index = 0;
                }

                //End reading on whitespace.
                if (whites.Contains(this.Buffer[ index ])) {

                    state = GetWordState.EOW;
                    break;
                }

                //End reading on EOF.
                if (this.Buffer[ index ] == '\0') {

                    state = GetWordState.EOF;
                    break;
                }

                //valid character read
                word.Append(this.Buffer[ index ]);
                index++;
            }

            this.getWordState = state;
            this.BufferIndex = index;

            return word.ToString();
        }

        /// <summary>
        /// Clears all white characters from BufferIndex to first non white char and
        /// updates BufferIndex variable.
        /// </summary>
        public void Trim() {

            int index = this.BufferIndex;
            int EOLCounter = 0;
            char[] regularWhite = new char[] { ' ', '\t' };
            TrimState state = TrimState.Default;

            while ( true ) {

                //check index out of range
                if (index >= BufferSize && state != TrimState.EOF) {

                    FillBuffer();
                    index = 0;
                }

                //check for ' ', '\n', '\t', '\0'
                if (regularWhite.Contains(Buffer[index])) {

                    index++;
                }
                else if (Buffer[index] == '\n') {

                    EOLCounter++;
                    index++;

                    if (EOLCounter > 1) {

                        state = TrimState.EOL;
                    }
                }
                else {

                    if (Buffer[index] == '\0') {

                        /*
                        if (state == TrimState.EOL) {

                            state = TrimState.EOLF;
                        }
                        else {

                            state = TrimState.EOF;
                        }
                        */
                        state = TrimState.EOF;
                    }

                    break;
                }
            }
            
            this.BufferIndex = index;
            this.trimState = state;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetTrimState() {

            this.trimState = TrimState.Default;
        }

        /// <summary>
        /// <para>IN = "in line" = ' ' or '\t' was read;</para>
        /// <para>EOL = "end of line" = multiple '\n' was read;</para>
        /// <para>EOF = "end of file" = '\0' was read</para>
        /// <para>EOLF = "end of line, file" = multiple '\n' were read and'\0' was read</para>
        /// </summary>
        public enum TrimState {
            EOLF, 
            EOL, 
            EOF, Default };

        /// <summary>
        /// IN = FillBuffer read 1024 chars;
        /// EOB = FillBuffer read < 1024 chars;
        /// </summary>
        public enum FillBufferState { IN, EOB, Default };

        /// <summary>
        /// EOW = "end of word" = White space was red;
        /// EOF = "end of file" = '\0' read
        /// </summary>
        public enum GetWordState { EOW, EOF, Default };
    }

    class MyWordWriter {

        private TextWriter textWriter;
        private int LineSize;
        public MyWordWriter( TextWriter textWriter, int lineSize ) {

            this.textWriter = textWriter;
            this.LineSize = lineSize;
        }

        public void PrintEmptyLine() {

            this.textWriter.WriteLine();
        }

        public void PrintNormalLine(List<string> words) {

            StringBuilder line = new StringBuilder();

            for (int i = 0; i < words.Count; i++) {

                line.Append(words[ i ]);

                if (i < (words.Count - 1)) {

                    line.Append(' ');
                }
            }

            this.textWriter.WriteLine(line.ToString());
        }

        public void PrintFormatedLine(List<string> words, int currentLength) {

            int numSpaces = words.Count - 1;
            int toFill = this.LineSize - currentLength;

            int evenSpaces = toFill / numSpaces;
            int remainingSpaces = toFill % numSpaces;

            StringBuilder line = new StringBuilder();

            for (int i = 0; i < words.Count; i++) {

                line.Append(words[ i ]);

                if (i < words.Count - 1) {
                    
                    for (int j = 0; j < evenSpaces; j++) {

                        line.Append(' ');
                    }

                    if (remainingSpaces > 0) {

                        line.Append(' ');

                        remainingSpaces--;
                    }
                }
            }

            this.textWriter.WriteLine(line.ToString());
        }

        public void Flush() {

            this.textWriter.Flush();
        }
    }
}
