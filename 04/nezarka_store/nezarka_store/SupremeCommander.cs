using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nezarka_store {
    public class SupremeCommander {

        private const string EmptyInput = "";
        internal ModelStore Nezarka;
        internal StoreViewer storeViewer;

        /// <summary>
        /// Parse INPUT data and create new ModelStore object.
        /// </summary>
        /// <param name="wrongInput">Flag signaling weather the INPUT was correct or not.</param>
        public void GetData(TextReader dataReader, out bool wrongInput) {

            wrongInput = default(bool);

            this.Nezarka = ModelStore.LoadFrom(dataReader, out wrongInput);
        }

        /// <summary>
        /// Clears the memmory.
        /// </summary>
        public void DisposeStore() {

            this.Nezarka = null;
        }

        /// <summary>
        /// On any error in INPUT data print to STDOUT.
        /// </summary>
        public void PrintDataError() {

            Console.WriteLine("Data error.");
        }

        /// <summary>
        /// Reads commands from stdin. If a command is correct, it is executed. If not, invalid HTML is printed.
        /// </summary>
        /// <param name="commandReader">Text reader. Has to read STDIN.</param>
        public void InititateCommandProcessing(TextReader commandReader, StoreViewer storeViewer) {

            this.storeViewer = storeViewer;

            while (true) {

                string line = commandReader.ReadLine();
                CommandType type = default(CommandType);
                int customerId = default(int);
                int bookId = default(int);

                if (line == EmptyInput || line == null) {

                    break;
                }

                if (ParseCommand(line, out type, out customerId, out bookId)) {

                    ExecuteCommand(type, customerId, bookId);
                }
                else {

                    this.storeViewer.InvalidRequest();
                    this.storeViewer.Indent();
                }
            }
        }

        /// <summary>
        /// Parses each command, that has been read. Checks for misspeling, unexisting data, logical mistakes, etc.
        /// </summary>
        /// <param name="line">String containing line with the command as it was read from the input.</param>
        /// <param name="type">Carries information of type of the command. Later used by ExecuteCommand method to decide output.</param>
        /// <param name="customerId">Identification of customer in Nezarka's database.</param>
        /// <param name="bookId">Identification of book in Nezarka's database.</param>
        /// <returns>True if command is valid from syntax perspective. False otherwise.</returns>
        public bool ParseCommand( string line, out CommandType type, out int customerId, out int bookId ) {

            string[] data;

            data = line.Split(' ');

            type = default(CommandType);
            customerId = default(int);
            bookId = default(int);

            if (data.Length != 3 || data[ 0 ] != "GET") {

                return false;
            }

            if (!int.TryParse(data[ 1 ], out customerId)) {

                return false;
            }

            if (data[ 2 ] == "http://www.nezarka.net/Books") {

                type = CommandType.ListBooks;
                return true;
            }

            if (data[ 2 ].Length >= 36) {

                if (data[ 2 ].Substring(0, 36) == "http://www.nezarka.net/Books/Detail/") {

                    type = CommandType.PrintBookDetail;
                    return int.TryParse(data[ 2 ].Substring(36), out bookId);
                }
            }

            if (data[ 2 ] == "http://www.nezarka.net/ShoppingCart") {

                type = CommandType.ListCart;
                return true;
            }

            if (data[ 2 ].Length >= 40) {

                if (data[ 2 ].Substring(0, 40) == "http://www.nezarka.net/ShoppingCart/Add/") {

                    type = CommandType.CartAddBook;
                    return int.TryParse(data[ 2 ].Substring(40), out bookId);
                }
            }

            if (data[ 2 ].Length >= 43) {

                if (data[ 2 ].Substring(0, 43) == "http://www.nezarka.net/ShoppingCart/Remove/") {

                    type = CommandType.CartRemoveBook;
                    return int.TryParse(data[ 2 ].Substring(43), out bookId);
                }
            }

            return false;
        }

        /// <summary>
        /// Depending on CommandType this method chooses wich HTML page to show the user.
        /// </summary>
        /// <param name="type">Carries a type variable. Crucial for this method.</param>
        /// <param name="customerId">Identification of the customer in Nezarka's database.</param>
        /// <param name="bookId">Identification of the book in Nezarka's database.</param>
        public void ExecuteCommand(CommandType type, int customerId, int bookId) {

            if (!this.Nezarka.IsCustomer(customerId)) {

                this.storeViewer.InvalidRequest();
                this.storeViewer.Indent();

                return;
            }

            if (type == CommandType.PrintBookDetail || type == CommandType.CartAddBook || type == CommandType.CartRemoveBook) {

                if (!this.Nezarka.IsBook(bookId)) {

                    this.storeViewer.InvalidRequest();
                    this.storeViewer.Indent();

                    return;
                }
            }

            switch (type) {

                case CommandType.Invalid:

                    this.storeViewer.InvalidRequest();
                    break;

                case CommandType.ListBooks:

                    this.storeViewer.GetBooksHtml(Nezarka.GetCustomer(customerId), this.Nezarka);
                    break;

                case CommandType.PrintBookDetail:

                    this.storeViewer.GetBookDetail(this.Nezarka.GetBook(bookId), this.Nezarka.GetCustomer(customerId));
                    break;

                case CommandType.ListCart:

                    this.storeViewer.GetShopingCart(this.Nezarka.GetCustomer(customerId), this.Nezarka);
                    break;

                case CommandType.CartAddBook: {

                        Customer customer = this.Nezarka.GetCustomer(customerId);
                        customer.ShoppingCart.AddBook(bookId);

                        this.storeViewer.GetShopingCart(this.Nezarka.GetCustomer(customerId), this.Nezarka);
                } break;

                case CommandType.CartRemoveBook: {

                        Customer customer = this.Nezarka.GetCustomer(customerId);
                        if (customer.ShoppingCart.RemoveBook(bookId)) {

                            this.storeViewer.GetShopingCart(this.Nezarka.GetCustomer(customerId), this.Nezarka);
                        }
                        else {

                            this.storeViewer.InvalidRequest();
                        }

                } break;
            }

            this.storeViewer.Indent();
        }
        
        /// <summary>
        /// Type used to carry information of wich type the command is. Used when deciding what HTML to print out.
        /// </summary>
        public enum CommandType {

            Invalid,
            ListBooks,
            PrintBookDetail,
            ListCart,
            CartAddBook,
            CartRemoveBook
        }
    }
}
