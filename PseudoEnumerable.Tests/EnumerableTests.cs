using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PseudoEnumerable.Tests
{
    [TestFixture]
    public class EnumerableTests
    {
        [TestCase(new[] { 7, 1, 2, 3, 4, 5, 6, 7, 68, 69, 70, 15, 17 }, ExpectedResult = new[] { 68, 69, 70 })]
        [TestCase(new[] { 0 }, ExpectedResult = new int[0])]
        public IEnumerable<int> Filter_Integer_CanFilter(int[] array)
            => array.Filter((i) => i > 50);

        [TestCase(arg: new[] { "a", "aaa", "", "aaaaa", "nnnnnn" }, ExpectedResult = new[] { "aaaaa", "nnnnnn" })]
        [TestCase(arg: new[] { "gg" }, ExpectedResult = new string[0])]
        public IEnumerable<string> Filter_String_CanFilter(string[] array)
            => array.Filter((s) => s.Length > 3);

        public void Filter_ArrayIsNull_Throw_ArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(
                () => Enumerable.Filter<int>(null, i => i > 0));

        [TestCase(new[] { 7, 1, 2, 3, 4, 5, 6, 7, 68, 69, 70, 15, 17 }, ExpectedResult = true)]
        [TestCase(new[] { 7, 1, 2, 3, 4, 5, 6, 7, 68, 69, 70, 15, -1 }, ExpectedResult = false)]
        [TestCase(new[] { 0 }, ExpectedResult = false)]
        public bool ForAll_Integer_CanWork(int[] array)
            => array.ForAll(i => i > 0);

        [TestCase(arg:new[] { "aaaa", "uuuu", "aaaaa", "nnnnnn" }, ExpectedResult = true)]
        [TestCase(arg:new[] { "a", "aaa", "", "aaaaa", "nnnnnn" }, ExpectedResult = false)]
        [TestCase(arg:new[] { "" }, ExpectedResult = false)]
        public bool ForAll_String_CanWork(string[] array)
            => array.ForAll<string>(s => s.Length > 3);

        public void ForAll_ArrayIsNull_Throw_ArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(
                () => Enumerable.ForAll<int>(null, i => i > 0));

        public void ForAll_PredicateIsNull_Throw_ArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(
                () => Enumerable.ForAll<int>(new int[0], null));

        [TestCase(arg:new object[] { 0, 1, 2 }, ExpectedResult = new int[]{0, 1, 2})]
        public IEnumerable<int> CastTo_Integer_CanWork(object[] array)
            => Enumerable.CastTo<int>(array);

        [TestCase(arg: new object[] { "0", "1", "2" }, ExpectedResult = new string[] { "0", "1", "2" })]
        public IEnumerable<string> CastTo_String_CanWork(object[] array)
            => Enumerable.CastTo<string>(array);

        public void CastTo_OneObjectCannotBeCasted_Throw_InvalidCastException() =>
            Assert.Throws<InvalidCastException>(
                () => Enumerable.CastTo<int>(new object[]{1, 2, "3"}));
    }
}