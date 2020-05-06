using System;
using System.Collections;
using System.Collections.Generic;

namespace RedOwl.Core
{
    public class RingBuffer<T> : IEnumerable<T>
    {
        private readonly T[] _buffer;

        private int _start;
        private int _end;
        private int _size;
        
        public RingBuffer(int capacity) : this(capacity, new T[] { }) {}
        
        public RingBuffer(int capacity, T[] items)
        {
            if (capacity < 1)
            {
                throw new ArgumentException("Circular buffer cannot have negative or zero capacity.", nameof(capacity));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Length > capacity)
            {
                throw new ArgumentException("Too many items to fit circular buffer", nameof(items));
            }

            _buffer = new T[capacity];

            Array.Copy(items, _buffer, items.Length);
            _size = items.Length;

            _start = 0;
            _end = _size == capacity ? 0 : _size;
        }
        
        private int InternalIndex(int index)
        {
            return _start + (index < (Capacity - _start) ? index : index - Capacity);
        }
        
        private void Increment(ref int index)
        {
            if (++index == Capacity)
            {
                index = 0;
            }
        }

        private void Decrement(ref int index)
        {
            if (index == 0)
            {
                index = Capacity;
            }
            index--;
        }
        
        private void ThrowIfEmpty(string message = "Cannot access an empty buffer.")
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException(message);
            }
        }

        #region IEnumerable implementation
        private ArraySegment<T> ArrayOne()
        {
            if (_start < _end)
            {
                return new ArraySegment<T>(_buffer, _start, _end - _start);
            }
            else
            {
                return new ArraySegment<T>(_buffer, _start, _buffer.Length - _start);
            }
        }

        private ArraySegment<T> ArrayTwo()
        {
            if (_start < _end)
            {
                return new ArraySegment<T>(_buffer, _end, 0);
            }
            else
            {
                return new ArraySegment<T>(_buffer, 0, _end);
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            var segments = new ArraySegment<T>[2] { ArrayOne(), ArrayTwo() };
            foreach (ArraySegment<T> segment in segments)
            {
                for (int i = 0; i < segment.Count; i++)
                {
                    yield return segment.Array[segment.Offset + i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        #endregion
        
        #region API
        public int Size { get { return _size; } }
        
        public int Capacity { get { return _buffer.Length; } }

        public bool IsFull {
            get {
                return Size == Capacity;
            }
        }

        public bool IsEmpty {
            get {
                return Size == 0;
            }
        }
        
        public void Clear()
        {
            _size = _start = _end = 0;
        }
        
        public T this[int index] {
            get {
                if (IsEmpty) throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer is empty", index));
                if (index >= _size) throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer size is {1}", index, _size));
                int actualIndex = InternalIndex(index);
                return _buffer[actualIndex];
            }
            set {
                if (IsEmpty) throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer is empty", index));
                if (index >= _size) throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer size is {1}", index, _size));
                int actualIndex = InternalIndex(index);
                _buffer[actualIndex] = value;
            }
        }
        
        /// <summary>
        /// Element at the front of the buffer - this[0].
        /// </summary>
        /// <returns>The value of the element of type T at the front of the buffer.</returns>
        public T Front()
        {
            ThrowIfEmpty();
            return _buffer[_start];
        }

        /// <summary>
        /// Element at the back of the buffer - this[Size - 1].
        /// </summary>
        /// <returns>The value of the element of type T at the back of the buffer.</returns>
        public T Back()
        {
            ThrowIfEmpty();
            return _buffer[(_end != 0 ? _end : Capacity) - 1];
        }
        
        /// <summary>
        /// Pushes a new element to the back of the buffer. Back()/this[Size-1]
        /// will now return this element.
        /// 
        /// When the buffer is full, the element at Front()/this[0] will be 
        /// popped to allow for this new element to fit.
        /// </summary>
        /// <param name="item">Item to push to the back of the buffer</param>
        public void PushBack(T item)
        {
            if (IsFull) {
                _buffer[_end] = item;
                Increment(ref _end);
                _start = _end;
            } else {
                _buffer[_end] = item;
                Increment(ref _end);
                ++_size;
            }
        }

        /// <summary>
        /// Pushes a new element to the front of the buffer. Front()/this[0]
        /// will now return this element.
        /// 
        /// When the buffer is full, the element at Back()/this[Size-1] will be 
        /// popped to allow for this new element to fit.
        /// </summary>
        /// <param name="item">Item to push to the front of the buffer</param>
        public void PushFront(T item)
        {
            if (IsFull) {
                Decrement(ref _start);
                _end = _start;
                _buffer[_start] = item;
            } else {
                Decrement(ref _start);
                _buffer[_start] = item;
                ++_size;
            }
        }
        
        /// <summary>
        /// Removes the element at the back of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        public void PopBack()
        {
            ThrowIfEmpty("Cannot take elements from an empty buffer.");
            Decrement(ref _end);
            _buffer[_end] = default(T);
            --_size;
        }

        /// <summary>
        /// Removes the element at the front of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        public void PopFront()
        {
            ThrowIfEmpty("Cannot take elements from an empty buffer.");
            _buffer[_start] = default(T);
            Increment(ref _start);
            --_size;
        }
        
        /// <summary>
        /// Copies the buffer contents to an array, according to the logical
        /// contents of the buffer (i.e. independent of the internal 
        /// order/contents)
        /// </summary>
        /// <returns>A new array with a copy of the buffer contents.</returns>
        public T[] ToArray()
        {
            if (IsEmpty) return new T[0];
            T[] newArray = new T[Size];
            int newArrayOffset = 0;
            var segments = new ArraySegment<T>[2] { ArrayOne(), ArrayTwo() };
            foreach (ArraySegment<T> segment in segments)
            {
                Array.Copy(segment.Array, segment.Offset, newArray, newArrayOffset, segment.Count);
                newArrayOffset += segment.Count;
            }
            return newArray;
        }
        #endregion
    }
}
