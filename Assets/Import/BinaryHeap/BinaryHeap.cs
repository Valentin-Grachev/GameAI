/**
 * BinaryHeap.cs - a generic Binary Heap implementation in C#
 * 
 * T must implement IComparable
 * 
 * BinaryHeap<T> bh = new BinaryHeap<T>();  creates empty binary heap of Ts with natural order
 * BinaryHeap<T> bh = new BinaryHeap<T>(compareFunc);  creates empty binary heap with a comparison function
 * i.e. for reverse sorting, sorting by alternate criterion etc.
 * 
 * use bh.push(x) to add x to the binary heap
 * use bh.pop() to retrieve the top element. Throws IndexOutOfRangeException if called on empty heap.
 * use bh.peek() to retrieve but not remove the top element. Throws IndexOutOfRangeException if called on empty heap.
 */


using System;
using System.Collections.Generic;


namespace DataStructures
{
    public delegate int Compare(IComparable x, IComparable y);

    public class BinaryHeap<T> where T:IComparable
    {
        Compare comp;   // comparison function, pass delegate to constructor to use
        List<T> data = new List<T>();

        public int Size { get { return data.Count; } private set { } }
                

        public BinaryHeap() { comp = (x, y) => { return x.CompareTo(y); }; }

        public BinaryHeap(Compare compareTo) { comp = compareTo; }


        public bool IsEmpty() { return data.Count == 0; }


        /**
         * add element to the heap, reordering to maintain consistency
         */
        public void Push(T x)
        {
            data.Add(x);
            int index = Size - 1;

            while (index > 0)
            {
                int parent = ParentIndex(index);

                if (comp(data[index], data[parent]) < 0)
                {
                    SwapAt(index, parent);
                    index = parent;
                }
                else break;
            }
        }


        public void Push(T[] xs)
        {
            foreach (T x in xs) Push(x);
        }


        /**
         * removes and returns the top element from the heap, reordering to maintain consistency
         */
        public T Pop()
        {
            if (IsEmpty()) throw new System.IndexOutOfRangeException("Called Pop() on empty BinaryHeap");

            T ret = data[0];
            int index = 0;

            SwapAt(0, Size - 1);
            data.RemoveAt(Size - 1);

            while (index < Size)
            {
                int childIndex = MinChildIndex(index);

                if (childIndex >= 0 && comp(data[childIndex], data[index]) < 0)
                {
                    SwapAt(index, childIndex);
                    index = childIndex;
                }
                else break;
            }

            return ret;
        }


        public T[] Pop(int n)
        {
            T[] ret = new T[n];
            for (int i = 0; i < n; ++i)
            {
                ret[i] = Pop();
            }
            return ret;
        }


        public T[] PopAll()
        {
            return Pop(Size);
        }


        /**
         * get top element without removing it
         */
        public T Peek()
        {
            if (IsEmpty()) throw new System.IndexOutOfRangeException("Called Peek() on empty BinaryHeap");
            return data[0];
        }


        /**
         * gets n elements without removing them (puts the array back before returning)
         */
        public T[] Peek(int n)
        {
            T[] ret = new T[n];
            for (int i = 0; i < n; ++i)
            {
                ret[i] = Pop();
            }

            Push(ret);
            return ret;
        }


        /**
         * returns the contents as array without popping. Copies the list first, pops everything, then restores data to original
         */
        public T[] PeekAll()
        {
            List<T> original = new List<T>(data);
            int n = Size;
            T[] ret = new T[n];
            for (int i = 0; i < n; ++i)
            {
                ret[i] = Pop();
            }
            data = original;

            return ret;
        }


        /**
         * -------- INTERNAL HELPER FUNCTIONS ---------
         */
         
        int ParentIndex(int i)
        {
            return (i + 1) / 2 - 1;
        }


        /**
         * finds the array index of the minimum of the two children of element at index i
         */
        int MinChildIndex(int i)
        {
            int childR = (i + 1) * 2;
            int childL = childR - 1;

            if (childL >= Size) return -1;
            else if (childR < Size && comp(data[childR], data[childL]) < 0) return childR;
            else return childL;
        }

        /**
         * swaps elements at indices i and j in the array
         */
        void SwapAt(int i, int j)
        {
            T temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }
    }
}