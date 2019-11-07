using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nezarka_store {
    public class StoreViewer {

        private TextWriter Out;

        public StoreViewer( TextWriter Out ) {

            this.Out = Out;
        }

        public void GetShopingCart( Customer customer, ModelStore store ) {

            Out.WriteLine("<!DOCTYPE html>");
            Out.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Out.WriteLine("<head>");
            Out.WriteLine("	<meta charset=\"utf-8\" />");
            Out.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            Out.WriteLine("</head>");
            Out.WriteLine("<body>");
            Out.WriteLine("	<style type=\"text/css\">");
            Out.WriteLine("		table, th, td {");
            Out.WriteLine("			border: 1px solid black;");
            Out.WriteLine("			border-collapse: collapse;");
            Out.WriteLine("		}");
            Out.WriteLine("		table {");
            Out.WriteLine("			margin-bottom: 10px;");
            Out.WriteLine("		}");
            Out.WriteLine("		pre {");
            Out.WriteLine("			line-height: 70%;");
            Out.WriteLine("		}");
            Out.WriteLine("	</style>");
            Out.WriteLine("	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            Out.WriteLine($"	{customer.FirstName}, here is your menu:");
            Out.WriteLine("	<table>");
            Out.WriteLine("		<tr>");
            Out.WriteLine("			<td><a href=\"/Books\">Books</a></td>");
            Out.WriteLine($"			<td><a href=\"/ShoppingCart\">Cart ({customer.ShoppingCart.Items.Count})</a></td>");
            Out.WriteLine("		</tr>");
            Out.WriteLine("	</table>");

            if (customer.ShoppingCart.Items.Count == 0) {

                Out.WriteLine("	Your shopping cart is EMPTY.");
            }
            else {

                Out.WriteLine("	Your shopping cart:");
                Out.WriteLine("	<table>");
                Out.WriteLine("		<tr>");
                Out.WriteLine("			<th>Title</th>");
                Out.WriteLine("			<th>Count</th>");
                Out.WriteLine("			<th>Price</th>");
                Out.WriteLine("			<th>Actions</th>");
                Out.WriteLine("		</tr>");
                decimal SUM = 0;

                for (int i = 0; i < customer.ShoppingCart.Items.Count; i++) {

                    Book b = store.GetBook(customer.ShoppingCart.Items[ i ].BookId);
                    Out.WriteLine("		<tr>");
                    Out.WriteLine($"			<td><a href=\"/Books/Detail/{b.Id}\">{b.Title}</a></td>");
                    Out.WriteLine($"			<td>{customer.ShoppingCart.Items[ i ].Count}</td>");
                    if (customer.ShoppingCart.Items[ i ].Count > 1)
                        Out.WriteLine($"			<td>{customer.ShoppingCart.Items[ i ].Count} * {b.Price} = {customer.ShoppingCart.Items[ i ].Count * b.Price} EUR</td>");
                    else
                        Out.WriteLine($"			<td>{b.Price} EUR</td>");
                    SUM += ( customer.ShoppingCart.Items[ i ].Count * b.Price );
                    Out.WriteLine($"			<td>&lt;<a href=\"/ShoppingCart/Remove/{b.Id}\">Remove</a>&gt;</td>");
                    Out.WriteLine("		</tr>");
                }
                Out.WriteLine("	</table>");
                Out.WriteLine($"	Total price of all items: {SUM} EUR");
            }

            Out.WriteLine("</body>");
            Out.WriteLine("</html>");
            Out.Flush();
        }

        public void GetBookDetail( Book book, Customer customer ) {

            Out.WriteLine("<!DOCTYPE html>");
            Out.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Out.WriteLine("<head>");
            Out.WriteLine("	<meta charset=\"utf-8\" />");
            Out.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            Out.WriteLine("</head>");
            Out.WriteLine("<body>");
            Out.WriteLine("	<style type=\"text/css\">");
            Out.WriteLine("		table, th, td {");
            Out.WriteLine("			border: 1px solid black;");
            Out.WriteLine("			border-collapse: collapse;");
            Out.WriteLine("		}");
            Out.WriteLine("		table {");
            Out.WriteLine("			margin-bottom: 10px;");
            Out.WriteLine("		}");
            Out.WriteLine("		pre {");
            Out.WriteLine("			line-height: 70%;");
            Out.WriteLine("		}");
            Out.WriteLine("	</style>");
            Out.WriteLine("	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            Out.WriteLine($"	{customer.FirstName}, here is your menu:");
            Out.WriteLine("	<table>");
            Out.WriteLine("		<tr>");
            Out.WriteLine("			<td><a href=\"/Books\">Books</a></td>");
            Out.WriteLine($"			<td><a href=\"/ShoppingCart\">Cart ({customer.ShoppingCart.Items.Count})</a></td>");
            Out.WriteLine("		</tr>");
            Out.WriteLine("	</table>");
            Out.WriteLine("	Book details:");
            Out.WriteLine("	<h2>{0}</h2>", book.Title);
            Out.WriteLine("	<p style=\"margin-left: 20px\">");
            Out.WriteLine("	Author: {0}<br />", book.Author);
            Out.WriteLine("	Price: {0} EUR<br />", book.Price);
            Out.WriteLine("	</p>");
            Out.WriteLine("	<h3>&lt;<a href=\"/ShoppingCart/Add/{0}\">Buy this book</a>&gt;</h3>", book.Id);
            Out.WriteLine("</body>");
            Out.WriteLine("</html>");
            Out.Flush();
        }

        public void GetBooksHtml( Customer cust, ModelStore store ) {

            IList<Book> books = store.GetBooks();

            Out.WriteLine("<!DOCTYPE html>");
            Out.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Out.WriteLine("<head>");
            Out.WriteLine("	<meta charset=\"utf-8\" />");
            Out.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            Out.WriteLine("</head>");
            Out.WriteLine("<body>");
            Out.WriteLine("	<style type=\"text/css\">");
            Out.WriteLine("		table, th, td {");
            Out.WriteLine("			border: 1px solid black;");
            Out.WriteLine("			border-collapse: collapse;");
            Out.WriteLine("		}");
            Out.WriteLine("		table {");
            Out.WriteLine("			margin-bottom: 10px;");
            Out.WriteLine("		}");
            Out.WriteLine("		pre {");
            Out.WriteLine("			line-height: 70%;");
            Out.WriteLine("		}");
            Out.WriteLine("	</style>");
            Out.WriteLine("	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            Out.WriteLine("	{0}, here is your menu:", cust.FirstName);
			Out.WriteLine("	<table>");
            Out.WriteLine("		<tr>");
            Out.WriteLine("			<td><a href=\"/Books\">Books</a></td>");
            Out.WriteLine($"			<td><a href=\"/ShoppingCart\">Cart ({cust.ShoppingCart.Items.Count})</a></td>");
			Out.WriteLine("		</tr>");
            Out.WriteLine("	</table>");
            Out.WriteLine("	Our books for you:");
            Out.WriteLine("	<table>");

			if (books.Count > 0) {

                Out.WriteLine("		<tr>");
            }

            for (int i = 0; i < books.Count; i++) {

                if (i % 3 == 0 && i != 0) {

                    Out.WriteLine("		</tr>");
                    Out.WriteLine("		<tr>");
                }

                Out.WriteLine("			<td style=\"padding: 10px;\">");
                Out.WriteLine("				<a href=\"/Books/Detail/{0}\">{1}</a><br />", books[ i ].Id, books[ i ].Title);
                Out.WriteLine("				Author: {0}<br />", books[ i ].Author);
                Out.WriteLine("				Price: {0} EUR &lt;<a href=\"/ShoppingCart/Add/{1}\">Buy</a>&gt;", books[ i ].Price, books[ i ].Id);
                Out.WriteLine("			</td>");
            }

            if (books.Count > 0) {

                Out.WriteLine("		</tr>");
            }

            Out.WriteLine("	</table>");
            Out.WriteLine("</body>");
            Out.WriteLine("</html>");
            Out.Flush();
        }

        public void InvalidRequest() {

            Out.WriteLine("<!DOCTYPE html>");
            Out.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Out.WriteLine("<head>");
            Out.WriteLine("	<meta charset=\"utf-8\" />");
            Out.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
            Out.WriteLine("</head>");
            Out.WriteLine("<body>");
            Out.WriteLine("<p>Invalid request.</p>");
            Out.WriteLine("</body>");
            Out.WriteLine("</html>");
            Out.Flush();
        }

        public void Indent() {

            Out.WriteLine("====");
            Out.Flush();
        }

        public void Dispose() {

            this.Out = null;
        }
    }
}
