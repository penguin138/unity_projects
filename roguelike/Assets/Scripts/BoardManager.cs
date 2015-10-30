using UnityEngine;
using System.Collections.Generic
	;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable] 
	public class RandomRange {
		public int min;
		public int max;

		public RandomRange(int newMin, int newMax) {
			min = newMin;
			max = newMax;
		}
		public int getRand() {
			return Random.Range(min, max + 1);
		}
	}
	public int columns = 8;
	public int rows = 8;
	public RandomRange innerWallsRange = new RandomRange(5,7);
	public RandomRange sodaRange = new RandomRange(1,3);
	public RandomRange foodRange = new RandomRange(1,3);
	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] innerWallTiles;
	public GameObject[] enemies;
	public GameObject exit;
	public GameObject[] foodTiles;
	public GameObject[] sodaTiles;

	private Transform boardHolder;
	
	List<Vector3> layout;

	void initLayout() {
		layout = new List<Vector3>();
		for (int i = 2; i < columns - 2; i++) {
			for (int j = 2; j < rows - 2; j++) {
				layout.Add(new Vector3(i, j, 0));
			}
		}
	}

	void fillFloor() {
		RandomRange floorRange = new RandomRange(0, floorTiles.Length - 1);
		RandomRange outerWallsRange = new RandomRange(0, outerWallTiles.Length - 1);
		GameObject instance;
		for (int i = 0; i < columns; i++) {
			for (int j = 0; j < rows; j++) {
				if ((i == 0 || i == columns - 1) || (j == 0 || j == rows - 1)) {
					instance = Instantiate(outerWallTiles[outerWallsRange.getRand()], new Vector3(i, j, 0), Quaternion.identity) as GameObject; 
					instance.transform.SetParent(boardHolder);
				} else {
					instance = Instantiate(floorTiles[floorRange.getRand()], new Vector3(i, j, 0), Quaternion.identity) as GameObject; 
					instance.transform.SetParent(boardHolder);
				}
			}
		}
	} 
	void fillItems(int level) {
		fillItem(innerWallTiles, innerWallsRange);
		fillItem(foodTiles, foodRange);
		fillItem(sodaTiles, sodaRange);
		GameObject instance = Instantiate(exit, new Vector3(columns - 2, rows - 2, 0), Quaternion.identity) as GameObject;
		instance.transform.SetParent(boardHolder);
		fillItem(enemies, new RandomRange((int)Mathf.Log(level), (int)Mathf.Log(level)));
	}

	void fillItem(GameObject[] itemRange, RandomRange itemCountRange) {
		int numberOfItems = itemCountRange.getRand();
		GameObject instance;
		for (int i = 0; i < numberOfItems; i++) {
			int itemPositionIndex = new RandomRange(0, layout.Count - 1).getRand();
			Vector3 itemPosition = layout[itemPositionIndex];
			GameObject itemType = itemRange[new RandomRange(0, itemRange.Length - 1).getRand()];
			instance = Instantiate(itemType,itemPosition,Quaternion.identity) as GameObject;
			instance.transform.SetParent(boardHolder);
			layout.RemoveAt(itemPositionIndex);
		}

	}
	public void createBoard(int level) {
		boardHolder = new GameObject().transform;
		initLayout();
		fillFloor();
		fillItems(level);
	}
}
