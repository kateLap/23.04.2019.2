﻿using System;
using System.Collections;
using System.Collections.Generic;
using PseudoLINQ;

namespace PseudoEnumerable
{
    public static class Enumerable
    {
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> to filter.</param>
        /// <param name="predicate">A function to test each source element for a condition</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> that contains elements from the input
        ///     sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="predicate"/> is null.</exception>
        public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            ValidateIsNull(predicate, nameof(predicate));
            ValidateIsNull(source, nameof(source));

            IEnumerable<TSource> Iterate()
            {
                foreach (var item in source)
                {
                    if (!predicate(item))
                    {
                        continue;
                    }

                    yield return item;
                }
            }

            return Iterate();
        }

        /// <summary>
        /// Transforms each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by transformer.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="transformer">A transform function to apply to each source element.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TResult}"/> whose elements are the result of
        ///     invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="transformer"/> is null.</exception>
        public static IEnumerable<TResult> Transform<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TResult> transformer)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns the range of integers
        /// </summary>
        /// <param name="start">First number of the range</param>
        /// <param name="count">Count of numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when count is less than zero.</exception>
        /// <returns>The range of integers.</returns>
        public static IEnumerable<int> Range(int start, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(count)} is less than zero");
            }

            checked
            {
                int last = start + count;
            }

            IEnumerable<int> Iterate()
            {
                while ((count--) > 0)
                {
                    yield return start++;               
                }
            }

            return Iterate();
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by key.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="key">A function to extract a key from an element.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="key"/> is null.</exception>
        public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> key)
        {
            ValidateIsNull(source, nameof(source));
            ValidateIsNull(key, nameof(key));

            return SortBy(source, key, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according by using a specified comparer for a key .
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by key.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="key">A function to extract a key from an element.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="comparer"/> is null.</exception>
        public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> key, IComparer<TKey> comparer)
        {
            ValidateIsNull(source, nameof(source));
            ValidateIsNull(key, nameof(key));
            ValidateIsNull(comparer, nameof(comparer));

            var list = new List<KeyValue<TKey, TSource>>();

            foreach (var item in source)
            {
                list.Add(new KeyValue<TKey, TSource>(key(item), item, comparer));
            }

            list.Sort();

            foreach (var item in list)
            {
                yield return item.Value;
            }
        }

        /// <summary>
        /// Casts the elements of an IEnumerable to the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to cast the elements of source to.</typeparam>
        /// <param name="source">The <see cref="IEnumerable"/> that contains the elements to be cast to type TResult.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{T}"/> that contains each element of the source sequence cast to the specified type.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type TResult.</exception>
        public static IEnumerable<TResult> CastTo<TResult>(IEnumerable source)
        {
            ValidateIsNull(source, nameof(source));

            if (source is IEnumerable<TResult> castedSource)
            {
                return castedSource;
            }

            IEnumerable<TResult> Iterate()
            {
                foreach (var item in source)
                {
                    yield return (TResult)item;
                }
            }
            
            return Iterate();
        }

        /// <summary>
        /// Determines whether all elements of a sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///     true if every element of the source sequence passes the test in the specified predicate,
        ///     or if the sequence is empty; otherwise, false
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="predicate"/> is null.</exception>
        public static bool ForAll<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ValidateIsNull(predicate, nameof(predicate));
            ValidateIsNull(source, nameof(source));

            foreach (var item in source)
            {
                if(!predicate(item))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if value is null.
        /// </summary>
        /// <param name="value">The reference to check.</param>
        /// <exception cref="ArgumentNullException">Throws when collection is null.</exception>
        private static void ValidateIsNull<T>(T value, string name) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{name} cannot be null");
            }
        }
    }
}