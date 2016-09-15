using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MonsterLevel
{
	public int cost;
	public GameObject visualization;
	public GameObject bullet;
	public float fireRate;
}

public class MonsterData : MonoBehaviour
{
	public List<MonsterLevel> levels;
	private MonsterLevel currentLevel;

	public MonsterLevel CurrentLevel
	{
		get { return currentLevel; }
		set
		{
			currentLevel = value;
			int currentLevelIndex = levels.IndexOf(currentLevel);
			GameObject levelVisiualisation = levels[currentLevelIndex].visualization;
			for (int i = 0; i < levels.Count; i++)
			{
				if (levelVisiualisation != null)
				{
					if (i == currentLevelIndex)
					{
						levels[i].visualization.SetActive(true);
					}
					else
					{
						levels[i].visualization.SetActive(false);
					}
				}
			}
		}
	}

	void OnEnable()
	{
		CurrentLevel = levels[0];
	}

	public MonsterLevel getNextLevel()
	{
		int currentLevelIndex = levels.IndexOf(currentLevel);
		int maxLevelIndex = levels.Count - 1;

		if (currentLevelIndex < maxLevelIndex)
		{
			return levels[currentLevelIndex + 1];
		}
		else
		{
			return null;
		}
	}

	public void increaseLevel()
	{
		int currentLevelIndex = levels.IndexOf(currentLevel);
		if (currentLevelIndex < levels.Count - 1)
		{
			CurrentLevel = levels[currentLevelIndex + 1];
		}
	}
}
