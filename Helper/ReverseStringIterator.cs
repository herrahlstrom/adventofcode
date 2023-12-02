using System;
using System.Collections;
using System.Linq;

namespace AdventOfCode.Helper
{
   internal static class ReverseStringExtensions
   {
      public static IEnumerable<char> ReadFromEnd(this string source)
      {
         return new ReverseStringIterator(source);
      }
   }

   internal sealed class ReverseStringEnumerator(string source) : IEnumerator<char>
   {
      private int _index = source.Length;

      object IEnumerator.Current => source[_index];

      public char Current => source[_index];

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
         if (_index <= 0)
         {
            return false;
         }

         _index--;
         return true;
      }

      public void Reset()
      {
         _index = source.Length;
      }
   }

   internal sealed class ReverseStringIterator(string source) : IEnumerable<char>
   {
      IEnumerator IEnumerable.GetEnumerator()
      {
         return new ReverseStringEnumerator(_source);
      }

      public IEnumerator<char> GetEnumerator()
      {
         return new ReverseStringEnumerator(_source);
      }

      private readonly string _source = source;
   }
}
