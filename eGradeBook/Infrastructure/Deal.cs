﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace eGradeBook.Infrastructure
{
    /// <summary>
    /// A primitive randomizer implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Deal<T> 
    {
        private List<T> _collection;
        private List<T> _remaining;
        private List<T> _used;
        private Random _rnd;

        /// <summary>
        /// Contstructor
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="seed"></param>
        public Deal(ICollection<T> collection, int seed = 100)
        {
            _collection = collection.ToList();
            _remaining = _collection.ToList();
            _used = new List<T>();
            _rnd = new Random(seed);
        }

        /// <summary>
        /// Deal one entry from the collection
        /// </summary>
        /// <returns></returns>
        public T DealOne()
        {
            int cnt = _remaining.Count;

            if (cnt < 1)
            {
                // empty
                // copy original collection and shuffle?
                _remaining = _collection.ToList();
                cnt = _remaining.Count;
            }

            int ix = _rnd.Next(cnt);
            // Debug.WriteLine("cnt: " + cnt + ", ix: " + ix);
            T one = _remaining.ElementAt(ix);

            _remaining.RemoveAt(ix);
            _used.Add(one);

            return one;
        }
    }
}