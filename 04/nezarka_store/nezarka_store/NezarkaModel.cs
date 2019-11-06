using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace NezarkaBookstore
{
	//
	// Model
	//

	class ModelStore {
		private List<Book> books = new List<Book>();
		private List<Customer> customers = new List<Customer>();

		public IList<Book> GetBooks() {

            return books;
		}

		public Book GetBook(int id) {

            try {

                return books.Find(b => b.Id == id);
            }
            catch (InvalidOperationException) {
                return default(Book);
            }
		}

		public Customer GetCustomer(int id) {

            try {

                return customers.Find(c => c.Id == id);
            }
            catch (InvalidOperationException) {

                return default(Customer);
            }
		}

		public static ModelStore LoadFrom(TextReader reader, out bool wrongInput) {
			var store = new ModelStore();

            wrongInput = false;

			try {
				if (reader.ReadLine() != "DATA-BEGIN") {

					return null;
				}

				while (true) {

                    wrongInput = false;
					string line = reader.ReadLine();

					if (line == null) {

						return null;

					} else if (line == "DATA-END") {

						break;
					}

					string[] tokens = line.Split(';');

					switch (tokens[0]) {

						case "BOOK":

                            if (Book.TryParse(tokens, out Book book)) {

                                //is this book original?
                                if (store.GetBook(book.Id) != null) {

                                    wrongInput = true;
                                }
                                else {

                                    store.books.Add(book);
                                }
                            }
                            else {

                                wrongInput = true;
                            }
							break;

						case "CUSTOMER":

                            if (Customer.TryParse(tokens, out Customer customer)) {

                                //is this customer new?
                                if (store.GetCustomer(customer.Id) != null) {

                                    wrongInput = true;
                                }
                                else {

                                    store.customers.Add(customer);
                                }
                            }
                            else {

                                wrongInput = true;
                            }
							break;

						case "CART-ITEM":

                            if (ShoppingCartItem.TryParse(tokens, out ShoppingCartItem shoppingCartItem, out int userID)) {

                                //does this customer exist?
                                if (store.GetCustomer(userID) != null) {

                                    //does the book exist?
                                    if (store.GetBook(shoppingCartItem.BookId) != null) {

                                        Customer existingCustomer = store.GetCustomer(userID);
                                        existingCustomer.ShoppingCart.AddItem(shoppingCartItem);
                                    }
                                    else {

                                        wrongInput = true;
                                    }
                                }
                                else {

                                    wrongInput = true;
                                }
                            }
                            else {

                                wrongInput = true;
                            }
							break;

						default:
							
                            wrongInput = true;
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

    class Book {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }

        /// <summary>
        /// Try to create new Book.
        /// </summary>
        /// <param name="data">Array of string. Splited input line.</param>
        /// <param name="book">Out value, contains new instance of Book.</param>
        /// <returns>True, if unsuccesfull return false.</returns>
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
            if (!decimal.TryParse(data[4], out price) || id < 0) {

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

	class Customer {
		private ShoppingCart shoppingCart;

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

        /// <summary>
        /// Try to create new Customer.
        /// </summary>
        /// <param name="data">Array of string. Splited input line.</param>
        /// <param name="customer">Out new instance of Customer.</param>
        /// <returns>True, if unsuccesfull return false.</returns>
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

	class ShoppingCartItem {
		public int BookId { get; set; }
		public int Count { get; set; }

        /// <summary>
        /// Try to create new ShopingCartItem.
        /// </summary>
        /// <param name="data">Array of string. Splited input line.</param>
        /// <param name="shoppingCartItem">Out new instance of ShopingCardItem.</param>
        /// <param name="userId">Out id of user the ShopingCart belongs to.</param>
        /// <returns>True, if unsuccesfull return false.</returns>
        public static bool TryParse( string[] data, out ShoppingCartItem shoppingCartItem, out int userId ) {

            shoppingCartItem = default(ShoppingCartItem);
            userId = 0;
            int bookId;
            int count;

            //arg count check
            if (data.Length != 4) {

                return false;
            }

            if (!int.TryParse(data[1], out userId) || userId < 0) {

                return false;
            }

            if (!int.TryParse(data[ 2 ], out bookId) || bookId < 0) {

                return false;
            }

            if (!int.TryParse(data[ 1 ], out count) || count < 0) {

                return false;
            }

            shoppingCartItem = new ShoppingCartItem {
                BookId = userId,
                Count = count
            };

            return true;
        }
	}

	class ShoppingCart {
		public int CustomerId { get; set; }
		public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();

        public void AddItem( ShoppingCartItem shoppingCartItem ) {

            Items.Add(shoppingCartItem);
        }
    }
}
