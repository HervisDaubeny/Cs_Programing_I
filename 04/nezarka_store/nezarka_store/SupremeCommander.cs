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
        /// Clean your memmory.
        /// </summary>
        public void DisposeStore() {

            this.Nezarka = null;
        }

        /// <summary>
        /// On any error in INPUT data.
        /// </summary>
        public void PrintDataError() {

            Console.WriteLine("Data error.");
        }

        /// <summary>
        /// Read commands from stdin. If a command is correct, it is executed. If not, invalid HTML is printed.
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
                    this.storeViewer.InvalidRequest();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="type"></param>
        /// <param name="customerId"></param>
        /// <param name="bookId"></param>
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

        public void ExecuteCommand(CommandType type, int customerId, int bookId) {

            if (!Nezarka.IsCustomer(customerId)) {

                this.storeViewer.InvalidRequest();

                return;
            }

            if (type == CommandType.PrintBookDetail || type == CommandType.CartAddBook || type == CommandType.CartRemoveBook) {

                if (!Nezarka.IsBook(bookId)) {

                    this.storeViewer.InvalidRequest();

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
