using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Deque<T> : IList<T> {
    private Data<T> Data;

    private int Version = 0;

    public bool Reversed { get; private set; }

    /// <summary>
    /// Indexer takes Reversed state of Deque in consideration and
    /// changes the index called for me. So whereever I use indexer
    /// and try to "rotate" the index for it, I musn't do it.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T this[int index] {
        get {
            if (Reversed) {
                return Data[Count - 1 - index];
            }
            return Data[index];
        }
        set {
            if (Reversed) {
                Data[Count - 1 - index] = value;
            }
            Data[index] = value; 
            Version++;
        } 
    }
    public int Count => Data.Count;

    public T First { get => this[0]; }

    public T Last { get => this[Count - 1]; }


    public bool IsReadOnly => false;

    public Deque() {
        Data = new Data<T>();
        Reversed = false;
    }

    public Deque(Deque<T> D) {
        Data = D.Data;
        this.Version = D.Version;
        Reversed = true;
    }

    public void Add(T item) {
        if (Reversed)
            Data.AddBeg(item);
        else
            Data.AddEnd(item);
        Version++;
    }

    public void AddBegin(T item) {
        if (!Reversed)
            Data.AddBeg(item);
        else
            Data.AddEnd(item);
        Version++;
    }

    public void Clear() {
        Data.Clear();
        Version++;
    }

    public bool Contains(T item) {
        for (int i = 0; i < Count; i++) {
            if (item.Equals(this[i])) {
                return true;
            }
        }

        return false;
    }

    public void CopyTo(T[] array, int arrayIndex) {
        int range = Math.Min(array.Length, Count);

        for (int i = arrayIndex; i < arrayIndex + range; i++) {
            array[i] = this[i - arrayIndex];
        }
    }

    public IEnumerator<T> GetEnumerator() {
        int version = Version;

        for (int i = 0; i < Count; i++) {
            yield return this[i];
            if (version != Version) {
                throw new InvalidOperationException("Kolekce byla upravena. Operace výčtu pravděpodobně nebude spuštěna.");
            }
        }
    }

    public int IndexOf(T item) {
        for (int i = 0; i < Count; i++) {
            if (object.Equals(this[i], item)) {
                return i;
            }
        }

        return -1;
    }

    public void Insert(int index, T item) {
        if (Reversed) {
            Data.AppendAt(index, item);
        }
        else {
            Data.InsertAt(index, item);
        }
        Version++;
    }

    public bool Remove(T item) {
        int index = IndexOf(item);
        if (index == -1) {
            return false;
        }

        Data.RemoveAt(index);
        Version++;

        return true;
    }

    public void RemoveAt(int index) {
        if (Reversed) {
            index = Count - 1 - index;
        }
        Data.RemoveAt(index);

        Version++;
    }

    public void Reverse() {
        Reversed = !Reversed;
        Version++;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        throw new NotImplementedException();
    }
}