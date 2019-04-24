using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PseudoEnumerable.Tests.Comparers;

namespace PseudoEnumerable.Tests
{
    [TestFixture]
    public class EnumerableTests
    {
        #region Filter

        [TestCase(new int[0], ExpectedResult = new int[0])]
        [TestCase(new int[] { 0 }, ExpectedResult = new int[] { })]
        [TestCase(new[] { 51, 50, 52, 53 }, ExpectedResult = new[] { 51, 52, 53 })]
        [TestCase(new[] { 7, 1, 2, 3, 4, 5, 6, 7, 68, 69, 70, 15, 17 }, ExpectedResult = new[] { 68, 69, 70 })]
        public IEnumerable<int> Filter_Integer_GreaterThanFifty_CanFilter(int[] array)
            => array.Filter(i => i > 50);

        [TestCase(new int[0], ExpectedResult = new int[0])]
        [TestCase(new int[] { 0 }, ExpectedResult = new int[] { 0 })]
        [TestCase(new int[] { 1, -1, 3, 7, 9 }, ExpectedResult = new int[] { })]
        [TestCase(new[] { 7, 1, 2, 3, 4, 5, 6, 7, 68, 69, 70, 15, 17 }, ExpectedResult = new[] { 2, 4, 6, 68, 70 })]
        public IEnumerable<int> Filter_Integer_IsEven_CanFilter(int[] array)
            => array.Filter(i => (i & 1) == 0);

        [TestCase(arg: new[] { "111", "gggggg","22","", "3333", "0000" }, ExpectedResult = new string[]{ "gggggg", "3333", "0000" })]
        [TestCase(arg: new[] { "a", "aaa", "", "aaaaa", "nnnnnn" }, ExpectedResult = new[] { "aaaaa", "nnnnnn" })]
        [TestCase(arg: new[] { "gg" }, ExpectedResult = new string[0])]
        public IEnumerable<string> Filter_String_CanFilter(string[] array)
            => array.Filter((s) => s.Length > 3);

        [Test]
        public void Filter_ArrayIsNull_Throw_ArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(
                () => Enumerable.Filter<int>(null, i => i > 0));

        #endregion

        #region ForAll

        [TestCase(new[] { 7, 1, 2, 3, 4, 5, 6, 7, 68, 69, 70, 15, 17 }, ExpectedResult = true)]
        [TestCase(new[] { 7, 1, 2, 3, 4, 5, 6, 7, 68, 69, 70, 15, -1 }, ExpectedResult = false)]
        [TestCase(new[] { 0 }, ExpectedResult = false)]
        public bool ForAll_Integer_CanWork(int[] array)
            => array.ForAll(i => i > 0);

        [TestCase(arg: new[] { "aaaa", "uuuu", "aaaaa", "nnnnnn" }, ExpectedResult = true)]
        [TestCase(arg: new[] { "a", "aaa", "", "aaaaa", "nnnnnn" }, ExpectedResult = false)]
        [TestCase(arg: new[] { "" }, ExpectedResult = false)]
        public bool ForAll_String_CanWork(string[] array)
            => array.ForAll<string>(s => s.Length > 3);

        [Test]
        public void ForAll_ArrayIsNull_Throw_ArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(
                () => Enumerable.ForAll<int>(null, i => i > 0));

        [Test]
        public void ForAll_PredicateIsNull_Throw_ArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(
                () => Enumerable.ForAll<int>(new int[0], null));

        #endregion

        #region CastTo

        [TestCase(arg: new object[] { 0, 1, 2 }, ExpectedResult = new int[] { 0, 1, 2 })]
        public IEnumerable<int> CastTo_Integer_CanWork(object[] array)
            => Enumerable.CastTo<int>(array);

        [TestCase(arg: new object[] { "0", "1", "2" }, ExpectedResult = new string[] { "0", "1", "2" })]
        public IEnumerable<string> CastTo_String_CanWork(object[] array)
            => Enumerable.CastTo<string>(array);

        [Test]
        public void CastTo_InvalidObjectType_Throw_InvalidCastException()
        {
            using (var iterator = Enumerable.CastTo<string>(new object[] { 1, 2, "3" }).GetEnumerator())
            {
                Assert.Throws<InvalidCastException>(
                    () => iterator.MoveNext());
            }
        }

        #endregion

        #region SortBy with default comparer

        [TestCase(arg: new string[] { "55555", "4444", "333", "aaa", "22", "1" },
            ExpectedResult = new string[] { "1", "22", "333", "aaa", "4444", "55555" })]
        [TestCase(arg: new string[] { "111", "2", "0000" }, ExpectedResult = new string[] { "2", "111", "0000" })]
        [TestCase(arg: new string[] { "1", "2", "3" }, ExpectedResult = new string[] { "1", "2", "3" })]
        [TestCase(arg: new string[] { "", "", "" }, ExpectedResult = new string[] { "", "", "" })]
        public IEnumerable<string> SortBy_String_KeyIsStringLength_DefaultComparer_CanSort(string[] array) =>
            Enumerable.SortBy(array, i => i.Length);

        [TestCase(arg: new[] { 0 }, ExpectedResult = new[] { 0 })]
        [TestCase(arg: new[] { 1, 2, 6, 7 }, ExpectedResult = new[] { 1, 2, 6, 7 })]
        [TestCase(arg: new[] { 10, 20, 60, 70 }, ExpectedResult = new[] { 10, 20, 60, 70 })]
        [TestCase(arg: new[] { 1000, 100, 10, 1 }, ExpectedResult = new[] { 1, 10, 100, 1000 })]
        [TestCase(arg: new[] { 60, 89090, 70, 8000, 800, 7 }, ExpectedResult = new[] { 7, 60, 70, 89090, 800, 8000 })]
        public IEnumerable<int> SortBy_Integer_KeyIsCountOfZeros_DefaultComparer_CanSort(int[] array) =>
            Enumerable.SortBy(array, i => i.ToString().Count(c => c == '0'));

        [Test]
        public void SortBy_ArrayIsNull_Throw_ArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(
                () => Enumerable.SortBy<string, int>(null, i => i.Length));

        #endregion

        #region SortBy with custom comparer

        [TestCase(arg: new string[] { "1", "22", "333", "aaa", "4444", "55555" },
            ExpectedResult = new string[] { "55555", "4444", "333", "aaa", "22", "1" })]
        [TestCase(arg: new string[] { "111", "2", "0000" }, ExpectedResult = new string[] { "0000", "111", "2" })]
        [TestCase(arg: new string[] { "1", "2", "3" }, ExpectedResult = new string[] { "1", "2", "3" })]
        [TestCase(arg: new string[] { "", "", "" }, ExpectedResult = new string[] { "", "", "" })]
        public IEnumerable<string> SortBy_String_KeyIsLength_CustomComparer_CanSort(string[] array)
            => Enumerable.SortBy(array, i => i.Length, new IntegerByDescendingComparer());

        #endregion

        #region Range

        [TestCase(int.MaxValue - 2, 2, ExpectedResult = new[] { int.MaxValue - 2, int.MaxValue - 1 })]
        [TestCase(5, 10, ExpectedResult = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 })]
        [TestCase(0, 0, ExpectedResult = new int[0])]
        [TestCase(5, 0, ExpectedResult = new int[0])]
        [TestCase(5, 1, ExpectedResult = new[] { 5 })]
        public IEnumerable<int> Range_Integers_CanGenerate(int start, int count)
            => Enumerable.Range(start, count);

        [Test]
        public void Range_CountIsNegative_Throw_ArgumentOutOfRangeException() =>
                Assert.Throws<ArgumentOutOfRangeException>(
                () => Enumerable.Range(5, -10));

        [Test]
        public void Range_ElementIsGreaterThanMaxValue_Throw_OverflowException() =>
            Assert.Throws<OverflowException>(
                () => Enumerable.Range(int.MaxValue, 10));

        #endregion
    }
}