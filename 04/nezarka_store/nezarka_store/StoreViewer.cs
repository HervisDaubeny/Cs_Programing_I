using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nezarka_store {
    public class StoreViewer {

        private TextWriter Writer;

        /// <summary>
        /// Constructor. Takes in TextWriter instance, wich it sets as its property.
        /// </summary>
        public StoreViewer( TextWriter writer ) {

            this.Writer = writer;
        }

        /// <summary>
        /// Prints HTML of customers shoping cart.
        /// </summary>
        /// <param name="customer">Customer object from Nezarka's database.</param>
        /// <param name="store">Nezarka it self. Used solely for it's database records.</param>
        public void GetShopingCart( Customer customer, ModelStore store ) {

            Writer.WriteLine("<!DOCTYPE html>");
            Writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Writer.WriteLine("<head>");
            Writer.WriteLine("	<meta charset=\"utf-8\" />");
            Writer.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            Writer.WriteLine("</head>");
            Writer.WriteLine("<body>");
            Writer.WriteLine("	<style type=\"text/css\">");
            Writer.WriteLine("		table, th, td {");
            Writer.WriteLine("			border: 1px solid black;");
            Writer.WriteLine("			border-collapse: collapse;");
            Writer.WriteLine("		}");
            Writer.WriteLine("		table {");
            Writer.WriteLine("			margin-bottom: 10px;");
            Writer.WriteLine("		}");
            Writer.WriteLine("		pre {");
            Writer.WriteLine("			line-height: 70%;");
            Writer.WriteLine("		}");
            Writer.WriteLine("	</style>");
            Writer.WriteLine("	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            Writer.WriteLine($"	{customer.FirstName}, here is your menu:");
            Writer.WriteLine("	<table>");
            Writer.WriteLine("		<tr>");
            Writer.WriteLine("			<td><a href=\"/Books\">Books</a></td>");
            Writer.WriteLine($"			<td><a href=\"/ShoppingCart\">Cart ({customer.ShoppingCart.Items.Count})</a></td>");
            Writer.WriteLine("		</tr>");
            Writer.WriteLine("	</table>");

            if (customer.ShoppingCart.Items.Count == 0) {

                Writer.WriteLine("	Your shopping cart is EMPTY.");
            }
            else {

                Writer.WriteLine("	Your shopping cart:");
                Writer.WriteLine("	<table>");
                Writer.WriteLine("		<tr>");
                Writer.WriteLine("			<th>Title</th>");
                Writer.WriteLine("			<th>Count</th>");
                Writer.WriteLine("			<th>Price</th>");
                Writer.WriteLine("			<th>Actions</th>");
                Writer.WriteLine("		</tr>");
                decimal SUM = 0;

                for (int i = 0; i < customer.ShoppingCart.Items.Count; i++) {

                    Book b = store.GetBook(customer.ShoppingCart.Items[ i ].BookId);
                    Writer.WriteLine("		<tr>");
                    Writer.WriteLine($"			<td><a href=\"/Books/Detail/{b.Id}\">{b.Title}</a></td>");
                    Writer.WriteLine($"			<td>{customer.ShoppingCart.Items[ i ].Count}</td>");
                    if (customer.ShoppingCart.Items[ i ].Count > 1)
                        Writer.WriteLine($"			<td>{customer.ShoppingCart.Items[ i ].Count} * {b.Price} = {customer.ShoppingCart.Items[ i ].Count * b.Price} EUR</td>");
                    else
                        Writer.WriteLine($"			<td>{b.Price} EUR</td>");
                    SUM += ( customer.ShoppingCart.Items[ i ].Count * b.Price );
                    Writer.WriteLine($"			<td>&lt;<a href=\"/ShoppingCart/Remove/{b.Id}\">Remove</a>&gt;</td>");
                    Writer.WriteLine("		</tr>");
                }
                Writer.WriteLine("	</table>");
                Writer.WriteLine($"	Total price of all items: {SUM} EUR");
            }

            Writer.WriteLine("</body>");
            Writer.WriteLine("</html>");
            Writer.Flush();
        }

        /// <summary>
        /// Prints HTML with detailed information about specific book of specific user.
        /// </summary>
        /// <param name="book">Book from Nezarka's database.</param>
        /// <param name="customer">Customer object from Nezarka's database.</param>
        public void GetBookDetail( Book book, Customer customer ) {

            Writer.WriteLine("<!DOCTYPE html>");
            Writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Writer.WriteLine("<head>");
            Writer.WriteLine("	<meta charset=\"utf-8\" />");
            Writer.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            Writer.WriteLine("</head>");
            Writer.WriteLine("<body>");
            Writer.WriteLine("	<style type=\"text/css\">");
            Writer.WriteLine("		table, th, td {");
            Writer.WriteLine("			border: 1px solid black;");
            Writer.WriteLine("			border-collapse: collapse;");
            Writer.WriteLine("		}");
            Writer.WriteLine("		table {");
            Writer.WriteLine("			margin-bottom: 10px;");
            Writer.WriteLine("		}");
            Writer.WriteLine("		pre {");
            Writer.WriteLine("			line-height: 70%;");
            Writer.WriteLine("		}");
            Writer.WriteLine("	</style>");
            Writer.WriteLine("	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            Writer.WriteLine($"	{customer.FirstName}, here is your menu:");
            Writer.WriteLine("	<table>");
            Writer.WriteLine("		<tr>");
            Writer.WriteLine("			<td><a href=\"/Books\">Books</a></td>");
            Writer.WriteLine($"			<td><a href=\"/ShoppingCart\">Cart ({customer.ShoppingCart.Items.Count})</a></td>");
            Writer.WriteLine("		</tr>");
            Writer.WriteLine("	</table>");
            Writer.WriteLine("	Book details:");
            Writer.WriteLine("	<h2>{0}</h2>", book.Title);
            Writer.WriteLine("	<p style=\"margin-left: 20px\">");
            Writer.WriteLine("	Author: {0}<br />", book.Author);
            Writer.WriteLine("	Price: {0} EUR<br />", book.Price);
            Writer.WriteLine("	</p>");
            Writer.WriteLine("	<h3>&lt;<a href=\"/ShoppingCart/Add/{0}\">Buy this book</a>&gt;</h3>", book.Id);
            Writer.WriteLine("</body>");
            Writer.WriteLine("</html>");
            Writer.Flush();
        }

        /// <summary>
        /// Prints HTML containing list of all books the customer has.
        /// </summary>
        /// <param name="customer">Customer object from Nezarka's database.</param>
        /// <param name="store">Nezarka it self. Used solely for it's database records.</param>
        public void GetBooksHtml( Customer customer, ModelStore store ) {

            IList<Book> books = store.GetBooks();

            Writer.WriteLine("<!DOCTYPE html>");
            Writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Writer.WriteLine("<head>");
            Writer.WriteLine("	<meta charset=\"utf-8\" />");
            Writer.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            Writer.WriteLine("</head>");
            Writer.WriteLine("<body>");
            Writer.WriteLine("	<style type=\"text/css\">");
            Writer.WriteLine("		table, th, td {");
            Writer.WriteLine("			border: 1px solid black;");
            Writer.WriteLine("			border-collapse: collapse;");
            Writer.WriteLine("		}");
            Writer.WriteLine("		table {");
            Writer.WriteLine("			margin-bottom: 10px;");
            Writer.WriteLine("		}");
            Writer.WriteLine("		pre {");
            Writer.WriteLine("			line-height: 70%;");
            Writer.WriteLine("		}");
            Writer.WriteLine("	</style>");
            Writer.WriteLine("	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            Writer.WriteLine("	{0}, here is your menu:", customer.FirstName);
			Writer.WriteLine("	<table>");
            Writer.WriteLine("		<tr>");
            Writer.WriteLine("			<td><a href=\"/Books\">Books</a></td>");
            Writer.WriteLine($"			<td><a href=\"/ShoppingCart\">Cart ({customer.ShoppingCart.Items.Count})</a></td>");
			Writer.WriteLine("		</tr>");
            Writer.WriteLine("	</table>");
            Writer.WriteLine("	Our books for you:");
            Writer.WriteLine("	<table>");

			if (books.Count > 0) {

                Writer.WriteLine("		<tr>");
            }

            for (int i = 0; i < books.Count; i++) {

                if (i % 3 == 0 && i != 0) {

                    Writer.WriteLine("		</tr>");
                    Writer.WriteLine("		<tr>");
                }

                Writer.WriteLine("			<td style=\"padding: 10px;\">");
                Writer.WriteLine("				<a href=\"/Books/Detail/{0}\">{1}</a><br />", books[ i ].Id, books[ i ].Title);
                Writer.WriteLine("				Author: {0}<br />", books[ i ].Author);
                Writer.WriteLine("				Price: {0} EUR &lt;<a href=\"/ShoppingCart/Add/{1}\">Buy</a>&gt;", books[ i ].Price, books[ i ].Id);
                Writer.WriteLine("			</td>");
            }

            if (books.Count > 0) {

                Writer.WriteLine("		</tr>");
            }

            Writer.WriteLine("	</table>");
            Writer.WriteLine("</body>");
            Writer.WriteLine("</html>");
            Writer.Flush();
        }

        /// <summary>
        /// Prints HTML shown when the request was invalid either in syntax or data.
        /// </summary>
        public void InvalidRequest() {

            Writer.WriteLine("<!DOCTYPE html>");
            Writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Writer.WriteLine("<head>");
            Writer.WriteLine("	<meta charset=\"utf-8\" />");
            Writer.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            Writer.WriteLine("</head>");
            Writer.WriteLine("<body>");
            Writer.WriteLine("<p>Invalid request.</p>");
            Writer.WriteLine("</body>");
            Writer.WriteLine("</html>");
            Writer.Flush();
        }

        /// <summary>
        /// Prints indentation line.
        /// </summary>
        public void Indent() {

            Writer.WriteLine("====");
            Writer.Flush();
        }

        /// <summary>
        /// Clears its Writer property from memory.
        /// </summary>
        public void Dispose() {

            this.Writer = null;
        }
    }
}
