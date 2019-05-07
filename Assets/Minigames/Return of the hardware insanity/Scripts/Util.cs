using System;
using System.Collections;
using UnityEngine;

namespace HardwareInsanity
{
	public static class Util
	{
		/// <summary>
		/// Runs a function after n seconds. Start this as a coroutine.
		/// </summary>
		public static IEnumerator RunAfterSeconds(float seconds, Action lambda)
		{
			yield return new WaitForSeconds(seconds);
			lambda();
		}

		/// <summary>
		/// Runs a function atfer n realtime seconds, ignoring Time.timeScale. Start this as a coroutine.
		/// </summary>
		/// <param name="realSeconds"></param>
		/// <param name="lambda"></param>
		/// <returns></returns>
		public static IEnumerator RunAfterRealSeconds(float realSeconds, Action lambda)
		{
			yield return new WaitForSecondsRealtime(realSeconds);
			lambda();
		}

		/// <summary>
		/// Runs a function after n frames. Start this as a coroutine.
		/// </summary>
		public static IEnumerator RunAfterFrames(int frames, Action lambda)
		{
			for (; frames > 1; frames--)
			{
				yield return null;
			}
			lambda();
		}

		/// <summary>
		/// Runs a function after n seconds. Start this as a coroutine.
		/// </summary>
		public static IEnumerator RunNextFrame(Action lambda)
		{
			yield return null;
			lambda();
		}

		/// <summary>
		/// Given a function that returns a bool, this function will execute another function when the first function returns true. Start this as a coroutine.
		/// </summary>
		public static IEnumerator RunAfterCondition(Func<bool> condition, Action lambda)
		{
			yield return new WaitUntil(condition);
			lambda();
		}

		/// <summary>
		/// Given a list of types (which should be Components), this will return the first instance of one of these components on the given GameObject.
		/// </summary>
		/// <returns>null if no components were found.</returns>
		public static Component GetBehaviourFromObject(Type[] types, GameObject obj)
		{
			foreach (Type type in types)
			{
				Component behavior = obj.GetComponent(type);
				if (behavior != null)
					return behavior;
			}
			return null;
		}

		/// <summary>
		/// Checks if an array is sorted with the logic given.
		/// </summary>
		/// <param name="func">This function will be called for every item in the array, with that item as the first argument, and the item after it as the second. If the function returns false anywhere, the list is not sorted.</param>
		public static bool IsSorted<T>(T[] array, Func<T, T, bool> func)
		{
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (!func(array[i], array[i + 1]))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Checks if an array is sorted with the default logic for that type.
		/// </summary>
		public static bool IsSorted<T>(T[] array, bool ascending = true) where T : IComparable<T>
		{
			if (ascending)
			{
				for (int i = 0; i < array.Length - 1; i++)
				{
					if (array[i].CompareTo(array[i + 1]) > 0)
						return false;
				}
			}
			else
			{
				for (int i = 0; i < array.Length - 1; i++)
				{
					if (array[i].CompareTo(array[i + 1]) < 0)
						return false;
				}
			}
			return true;
		}
	}
}
