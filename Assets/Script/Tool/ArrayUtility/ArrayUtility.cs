using System;
using System.Collections.Generic;

namespace BoardGame
{
	public static class ArrayUtility
	{
		public static int Wrap<T>(this IList<T> list, int index)
		{
			if (index < 0)
			{
				index = Math.Abs((list.Count - Math.Abs(index)) % list.Count);
			}
			else
			{
				index %= list.Count;
			}

			return index;
		}

		public static T[] Get<T>(this IList<T> list, int start, int end)
		{
			bool forward = end > start;

			int steps = Math.Abs(end - start);

			T[] items = new T[steps + 1];

			int position = 0;

			if (forward)
			{
				for (int i = start; i <= start + steps; i++)
				{
					items[position] = list[list.Wrap(i)];

					position++;
				}
			}
			else
			{
				for (int i = start; i >= start - steps; i--)
				{
					items[position] = list[list.Wrap(i)];

					position++;
				}
			}

			return items;
		}

		public static IEnumerable<int> EnumeratorIndices<T>(this IList<T> list, int start, int end)
		{
			bool forward = end > start;

			int steps = Math.Abs(end - start);

			if (forward)
			{
				for (int i = start; i <= start + steps; i++)
				{
					yield return list.Wrap(i);
				}
			}
			else
			{
				for (int i = start; i >= start - steps; i--)
				{
					yield return list.Wrap(i);
				}
			}
		}
	}
}