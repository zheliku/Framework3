// ------------------------------------------------------------
// @file       PriorityQueue.cs
// @brief
// @author     zheliku
// @Modified   2025-02-23 06:02:34
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     支持按优先级排序的列表（最小优先级在前），相同优先级后插入的排在后面
    /// </summary>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class PrioritySortedList<TElement, TPriority> : IEnumerable<TElement>
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly List<Entry> _elements = new();

        private readonly EntryComparer _entryComparer;

        private int _sequenceCounter;

        public PrioritySortedList() : this(Comparer<TPriority>.Default) { }

        public PrioritySortedList(IComparer<TPriority> priorityComparer)
        {
            var priorityComparer1 = priorityComparer ?? throw new ArgumentNullException(nameof(priorityComparer));
            _entryComparer = new EntryComparer(priorityComparer1);
        }

        public int Count
        {
            get => _elements.Count;
        }

        public bool IsEmpty
        {
            get => _elements.Count == 0;
        }

        public TElement this[int index]
        {
            get
            {
                if (index < 0 || index >= _elements.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return _elements[index].Element;
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.Select(entry => entry.Element).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TElement element, TPriority priority)
        {
            var entry            = new Entry(element, priority, ++_sequenceCounter);
            var index            = _elements.BinarySearch(entry, _entryComparer);
            if (index < 0) index = ~index;
            _elements.Insert(index, entry);
        }

        public bool TryDequeue(out TElement element)
        {
            if (_elements.Count == 0)
            {
                element = default(TElement);
                return false;
            }

            element = _elements[0].Element;
            _elements.RemoveAt(0);
            return true;
        }

        public TElement Dequeue()
        {
            if (!TryDequeue(out var element))
                throw new InvalidOperationException("Queue is empty");
            return element;
        }

        public bool TryPeek(out TElement element)
        {
            if (_elements.Count == 0)
            {
                element = default(TElement);
                return false;
            }

            element = _elements[0].Element;
            return true;
        }

        public TElement Peek()
        {
            if (!TryPeek(out var element))
                throw new InvalidOperationException("Queue is empty");
            return element;
        }

        public bool Remove(TElement element)
        {
            var comparer = EqualityComparer<TElement>.Default;

            // 倒序遍历以避免索引错位
            for (var i = _elements.Count - 1; i >= 0; i--)
            {
                if (comparer.Equals(_elements[i].Element, element))
                {
                    _elements.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public int RemoveAll(Predicate<TElement> match)
        {
            return _elements.RemoveAll(entry => match(entry.Element));
        }

        public void Clear()
        {
            _elements.Clear();
            _sequenceCounter = 0;
        }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly struct Entry
        {
        #if ODIN_INSPECTOR
            [ShowInInspector]
        #endif
            public TElement Element { get; }

        #if ODIN_INSPECTOR
            [ShowInInspector]
        #endif
            public TPriority Priority { get; }

        #if ODIN_INSPECTOR
            [ShowInInspector]
        #endif
            public int SequenceNumber { get; }

            public Entry(TElement element, TPriority priority, int sequenceNumber)
            {
                Element        = element;
                Priority       = priority;
                SequenceNumber = sequenceNumber;
            }
        }

        private class EntryComparer : IComparer<Entry>
        {
            private readonly IComparer<TPriority> _priorityComparer;

            public EntryComparer(IComparer<TPriority> priorityComparer)
            {
                _priorityComparer = priorityComparer;
            }

            public int Compare(Entry x, Entry y)
            {
                var priorityCompare = _priorityComparer.Compare(x.Priority, y.Priority);
                return priorityCompare != 0
                           ? priorityCompare
                           : x.SequenceNumber.CompareTo(y.SequenceNumber);
            }
        }
    }
}