using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueNumberGenerator 
{
	private HashSet<int> GeneratedNumbers = new HashSet<int>();

    
	public int GenerateUniqueNum(int min, int max)
	{
		if (GeneratedNumbers.Count >= max - min)
		{
			// All unique numbers have been used up
			Debug.Log("It seems the you've exhausted the number of unique numbers");

			ResetHashSet();
			return 0;
		}


		int randNum;
		do
		{
			// generate random numbers while the number isn't unique
			randNum = Random.Range(min, max);

		} while (GeneratedNumbers.Contains(randNum) == true);

		// Add the generate number to the HashSet so it doesn't get repeated on future calls
		GeneratedNumbers.Add(randNum);

		return randNum;

	}

	public void ResetHashSet()
	{
		GeneratedNumbers.Clear();
	}

	public Vector3 GenerateRandomPosition(int z)
	{
		var r_x = Random.Range(100, 300);
		var r_y = Random.Range(100, 300);
		return new Vector3(r_x, r_y, 10*z);
	}
}

