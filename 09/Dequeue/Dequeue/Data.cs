using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal sealed class Data<T> {
    private const int ChunkSize = 128;

    private T[][] Chunks;
    private int DataBase;
    private int ChunkCount;

    public int Count { get; set; }

    public T this[int index] {
        get {
            CheckIndexRange(index);
            int deqIndex = index + DataBase;
            return Chunks[deqIndex / ChunkSize][deqIndex % ChunkSize];
        }
        set {
            CheckIndexRange(index);
            int deqIndex = index + DataBase;
            Chunks[deqIndex / ChunkSize][deqIndex % ChunkSize] = value;
        }
    }

    public Data() {
        Count = 0;
        ChunkCount = 2;
        Chunks = new T[2][];
        Chunks[0] = new T[ChunkSize];
        Chunks[1] = new T[ChunkSize];
        DataBase = ChunkSize / 2;
    }

    public void CheckIndexRange(int index) {
        if (index < 0 || (index >= Count && Count > 0)) {
            string mess = string.Format("out of ragnge on index {0}, count is {1}", index, Count);
            throw new ArgumentOutOfRangeException(mess);
        }
    }

    public void DoubleSize() {
        int offset = ChunkCount / 2;
        ChunkCount *= 2;

        T[][] tmp = new T[ChunkCount][];
        for (int i = 0; i < ChunkCount; i++) {
            if (i < offset || i >= ChunkCount - offset) {
                tmp[i] = new T[ChunkSize];
                continue;
            }

            tmp[i] = Chunks[i - offset];
        }

        Chunks = tmp;
        DataBase += offset * ChunkSize;
    }

    public void AddBeg(T item) {
        if (!CheckBounds()) {
            DoubleSize();
        }

        Count++;
        DataBase--;
        this[0] = item;
    }

    public void AddEnd(T item) {
        if (!CheckBounds()) {
            DoubleSize();
        }

        Count++;
        this[Count - 1] = item;
    }

    public void RemoveBeg() {
        this[0] = default(T);
        Count--;
        DataBase++;
    }

    public void RemoveEnd() {
        this[Count - 1] = default(T);
        Count--;
    }

    private bool CheckBounds() {
        if (DataBase == 0 || DataBase + Count == ChunkSize * ChunkCount - 1) {
            return false;
        }

        return true;
    }

    public void Clear() {
        for (int i = 0; i < Count; i++) {
            this[i] = default(T);
        }

        Count = 0;
        DataBase = ChunkCount * ChunkSize / 4;
    }

    public void RemoveAt(int index) {
        if (Count > 0 && index >= 0 && index <= Count - 1) {
            if (index == 0) {
                RemoveBeg();

                return;
            }
            if (index == Count - 1) {
                RemoveEnd();

                return;
            }
 
            for (int i = index; i < Count - 1; i++) {
                this[i] = this[i + 1];
            }

            this[Count - 1] = default(T);
            Count--;
        }
        else {
            throw new ArgumentOutOfRangeException();
        }
    }

    public void InsertAt(int index, T item) {
        if (index == 0) {
            AddBeg(item);

            return;
        }
        if (index == Count) {
            AddEnd(item);

            return;
        }
        if (Count > 0) {
            AddEnd(this[Count - 1]);

            for (int i = Count - 2; i > index; i--) {
                this[i] = this[i - 1];
            }
        }
        else {
            Count++;
        }

        this[index] = item;
    }

    public void AppendAt(int index, T item) {
        if (index == 0) {
            AddEnd(item);

            return;
        }
        if (index == Count) {
            AddBeg(item);

            return;
        }

        int revIndex = index;

        if (Count > 0) {
            revIndex = Count - index;
            AddEnd(this[Count - 1]);

            for (int i = Count - 2; i > revIndex; i--) {
                this[i] = this[i - 1];
            }
        }
        else {
            Count++;
        }

        this[revIndex] = item;
    }
}
