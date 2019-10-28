using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace justify_reloaded
{
    class Program
    {
        static void Main(string[] args) {

            /* TODO
             * 
             * for output create these methods
             * PrintFormatedLine() #print line with special ' ' padding
             * PrintLine() #print line without any indentation
             * PrintEmptyLine() #indents block; then it is neccesary to set myWordReader.TrimState = default instead of EOL
             * 
             */
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
        private TrimState trimState = TrimState.Default;

        /// <summary>
        /// 
        /// </summary>
        private FillBufferState fillBufferState = FillBufferState.Default;

        /// <summary>
        /// 
        /// </summary>
        private GetWordState getWordState = GetWordState.Default;

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

                    state = TrimState.IN;

                    if (Buffer[index] == '\0') {

                        state = TrimState.EOF;
                    }

                    break;
                }
            }
            
            this.BufferIndex = index;
            this.trimState = state;
        }

        /// <summary>
        /// IN = "in line" = ' ' or '\t' was read;
        /// EOL = "end of line" = '\n' was read;
        /// EOF = "end of file" = '\0' was read
        /// </summary>
        private enum TrimState { IN, EOL, EOF, Default };

        /// <summary>
        /// IN = FillBuffer read 1024 chars;
        /// EOB = FillBuffer read < 1024 chars;
        /// </summary>
        private enum FillBufferState { IN, EOB, Default };

        /// <summary>
        /// EOW = "end of word" = White space was red;
        /// EOF = "end of file" = '\0' read
        /// </summary>
        private enum GetWordState { EOW, EOF, Default };
    }
}
