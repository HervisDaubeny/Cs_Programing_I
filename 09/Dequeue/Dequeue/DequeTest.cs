using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DequeTest {
    public static IList<T> GetReverseView<T>(Deque<T> d) {
        return new Deque<T>(d);
    }
}