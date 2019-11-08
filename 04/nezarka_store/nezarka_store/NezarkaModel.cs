using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace nezarka_store {

    /// <summary>
    /// Represents Nezarka's database.
    /// </summary>
	public class ModelStore {
		private List<Book> books = new List<Book>();
		private List<Customer> customers = new List<Customer>();

        /// <summary>
        /// Get list of books in database.
        /// </summary>
        /// <returns>List of Book.</returns>
		public IList<Book> GetBooks() {

            return books;
		}

        /// <summary>
        /// Get specific book from the database.
        /// </summary>
        /// <param name="id">Identification number of the book.</param>
        /// <returns>Book object.</returns>
		public Book GetBook(int id) {

            if (IsBook(id)) {

                return books.Find(b => b.Id == id);
            }
            else {

                return default(Book);
            }
		}

        /// <summary>
        /// Get specific customer from the database.
        /// </summary>
        /// <param name="id">Identification number of the customer.</param>
        /// <returns>Customer object.</returns>
        public Customer GetCustomer( int id ) {

            if (IsCustomer(id)) {

                return customers.Find(c => c.Id == id);
            }
            else {

                return default(Customer);
            }
		}

        /// <summary>
        /// Check if customer is listed in the database.
        /// </summary>
        /// <param name="id">Identification number of the customer.</param>
        /// <returns>True if customer has existing record in the database, false if not.</returns>
        public bool IsCustomer( int id ) {

            return ( customers.Where(x => x.Id == id).Count() == 1 );
        }

        /// <summary>
        /// Check if book is listed in the database.
        /// </summary>
        /// <param name="id">Identification number of the book.</param>
        /// <returns>True if book has existing record in the database, false if not.</returns>
        public bool IsBook( int id ) {

            return ( books.Where(x => x.Id == id).Count() == 1 );
        }

        /// <summary>
        /// Parses input data to database friendly format an includes them in it.
        /// If there is any error in the given data, process is aborted and empty object is returned.
        /// </summary>
        /// <param name="reader">Text reader. Must be set to read STDIN.</param>
        /// <param name="wrongInput">Flag that carries information of error in data loading / parsing.</param>
        /// <returns>Database object if the data were correct. NULL if they were not.</returns>
        public static ModelStore LoadFrom(TextReader reader, out bool wrongInput) {
			var store = new ModelStore();

            wrongInput = true;

			try {

				if (reader.ReadLine() != "DATA-BEGIN") {

					return null;
				}

				while (true) {

                    wrongInput = true;
					string line = reader.ReadLine();

					if (line == null) {

						return null;

					} 
                    else if (line == "DATA-END") {

                        wrongInput = false;

						break;
					}

					string[] tokens = line.Split(';');

					switch (tokens[0]) {

						case "BOOK":

                            if (Book.TryParse(tokens, out Book book)) {

                                //is this book original?
                                if (store.GetBook(book.Id) == null) {

                                    store.books.Add(book);
                                    wrongInput = false;
                                }
                            }

							break;

						case "CUSTOMER":

                            if (Customer.TryParse(tokens, out Customer customer)) {

                                //is this customer new?
                                if (store.GetCustomer(customer.Id) == null) {

                                    store.customers.Add(customer);
                                    wrongInput = false;
                                }
                            }

							break;

						case "CART-ITEM":

                            if (ShoppingCartItem.TryParse(tokens, out ShoppingCartItem shoppingCartItem, out int userID)) {

                                //does this customer exist?
                                if (store.GetCustomer(userID) == null) {

                                    break;
                                }

                                //does the book exist?
                                if (store.GetBook(shoppingCartItem.BookId) == null) {

                                    break;
                                }

                                Customer existingCustomer = store.GetCustomer(userID);
                                existingCustomer.ShoppingCart.AddItem(shoppingCartItem);
                                wrongInput = false;
                            }

							break;

						default:

                            break;
					}

                    if (wrongInput) {

                        return default(ModelStore);
                    }
				}
			} catch (Exception ex) {

				if (ex is FormatException || ex is IndexOutOfRangeException) {

					return null;
				}
				throw;
			}

			return store;
		}
	}

    /// <summary>
    /// Represents book object in Nezarka's database.
    /// </summary>
    public class Book {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }

        /// <summary>
        /// Try to parse input data and create new Book object.
        /// </summary>
        /// <param name="data">Array of string. Splited input line.</param>
        /// <param name="book">Out value, contains new instance of Book.</param>
        /// <returns>True on succes, false otherwise.</returns>
        public static bool TryParse( string[] data, out Book book ) {

            int id;
            string title;
            string author;
            decimal price;
            book = default(Book);

            //arg count check
            if (data.Length != 5) {

                return false;
            }

            //id
            if (!int.TryParse(data[ 1 ], out id) || id < 0) {

                return false;
            }

            //title and author conditions are tested by split(';') and ReadLine()
            title = data[ 2 ];
            author = data[ 3 ];

            //price
            if (!decimal.TryParse(data[4], out price) || price < 0) {

                return false;
            }

            book = new Book {
                Id = id,
                Title = title,
                Author = author,
                Price = price
            };

            return true;
        }
	}

    /// <summary>
    /// Represents customer object in Nezarka's database.
    /// </summary>
	public class Customer {
		private ShoppingCart shoppingCart;

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

        /// <summary>
        /// Try to parse input data and create new Customer object.
        /// </summary>
        /// <param name="data">Array of string. Splited input line.</param>
        /// <param name="customer">Out new instance of Customer.</param>
        /// <returns>True on succes, false otherwise.</returns>
        public static bool TryParse( string[] data, out Customer customer ) {

            int id;
            string name;
            string surename;
            customer = default(Customer);

            //arg count check
            if (data.Length != 4) {

                return false;
            }

            //id
            if (!int.TryParse(data[ 1 ], out id) || id < 0) {

                return false;
            }

            //name and surname conditions are tested by split(';') and ReadLine()
            name = data[ 2 ];
            surename = data[ 3 ];

            customer = new Customer {
                Id = id,
                FirstName = name,
                LastName = surename,
            };

            return true;
        }

        /// <summary>
        /// Represents its customer's shoping cart.
        /// </summary>
        public ShoppingCart ShoppingCart {
			get {
				if (shoppingCart == null) {
					shoppingCart = new ShoppingCart();
				}
				return shoppingCart;
			}
			set {
				shoppingCart = value;
			}
		}
	}

    /// <summary>
    /// Represents shoping cart's item object in Nezarka's database.
    /// </summary>
	public class ShoppingCartItem {
		public int BookId { get; set; }
		public int Count { get; set; }

        /// <summary>
        /// Try to parse input data and create new ShopingCartItem object.
        /// </summary>
        /// <param name="data">Array of string. Splited input line.</param>
        /// <param name="shoppingCartItem">Out new instance of ShopingCardItem.</param>
        /// <param name="userId">Out id of user the ShopingCart belongs to.</param>
        /// <returns>True on succes, false otherwise.</returns>
        public static bool TryParse( string[] data, out ShoppingCartItem shoppingCartItem, out int userId ) {

            shoppingCartItem = default(ShoppingCartItem);
            userId = 0;
            int bookId;
            int count;

            //arg count check
            if (data.Length != 4) {

                return false;
            }

            if (!int.TryParse(data[ 1 ], out userId) || userId < 0) {

                return false;
            }

            if (!int.TryParse(data[ 2 ], out bookId) || bookId < 0) {

                return false;
            }

            if (!int.TryParse(data[ 3 ], out count) || count < 0) {

                return false;
            }

            shoppingCartItem = new ShoppingCartItem {
                BookId = bookId,
                Count = count
            };

            return true;
        }
	}

    /// <summary>
    /// Represents shoping cart object in Nezarka's database.
    /// </summary>
	public class ShoppingCart {
		public int CustomerId { get; set; }
		public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();

        /// <summary>
        /// Adds item to its customer's shoping cart.
        /// </summary>
        /// <param name="shoppingCartItem">Book object to be added.</param>
        public void AddItem( ShoppingCartItem shoppingCartItem ) {

            Items.Add(shoppingCartItem);
        }

        /// <summary>
        /// Check if this shoping cart contains specific book.
        /// </summary>
        /// <param name="bookId">Identification number of the book.</param>
        /// <returns>True if shoping cart contains the book, false if it doesn't.</returns>
        public bool ConstainsBook( int bookId ) {

            return Items.Where(x => x.BookId == bookId).Count() == 1;
        }

        /// <summary>
        /// Tries to add a book to this shoping cart. If it is already there, its number is incremented. 
        /// Otherwise new Book object is created and AddItem() is called.
        /// </summary>
        /// <param name="bookId"></param>
        public void AddBook( int bookId ) {

            if (ConstainsBook(bookId)) {

                ShoppingCartItem book = Items.Find(x => x.BookId == bookId);
                book.Count++;
            }
            else {

                ShoppingCartItem newBook = new ShoppingCartItem() {

                    BookId = bookId,
                    Count = 1,
                };
                AddItem(newBook);
            }
        }

        /// <summary>
        /// Tries to remove a book from this shoping cart by decrementing its number of occurances.
        /// Result depends on check of book's presence in the cart.
        /// If the book is not present false is returned.
        /// If last occurance of this book is removed, whole item is removed from the cart.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>True if the book was succesfuly removed, false if there was no book to remove.</returns>
        public bool RemoveBook( int bookId ) {

            if (ConstainsBook(bookId)) {

                ShoppingCartItem book = Items.Find(x => x.BookId == bookId);
                book.Count--;
                if (book.Count == 0) {

                    Items.Remove(book);
                }
                return true;
            }
            return false;
        }
    }
}
