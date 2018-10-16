using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {

	public int width;
	public int height;
	public string seed;
	public bool useRandomSeed;

	public GameObject player;
	public GameObject stairs;
	public GameObject exit;
	public GameObject chest;
	public GameObject currentStairs;
	GameObject currentPlayer;
	public GameObject exitStairs;
	public List<GameObject> ChestList;
	public List<Enemy> EnemyList;
	public Enemy enemyArcher;
	public Enemy enemyLight;
	public Enemy enemyHeavy;
	public Enemy boss;
//	List<Destroyable> DestroyableList;
	int smallest_size = 0;
	[Range(0,100)]
	public int randomFillPercent;
	bool treasureRoomSpawned = false;
	public int[,] map;
	bool isBossRoom = false;
	public GameObject torch;
	List<GameObject> torchList;
	public bool bossSpawned = false;
	public bool spawnEnemies = true;

	void Start() {
		EnemyList = new List<Enemy> ();
		ChestList = new List<GameObject> ();
		torchList = new List<GameObject> ();
		currentPlayer = FindObjectOfType<Player> ().gameObject;
		GenerateMap();
	}

	void Update() {
	}

	public void GenerateMap() {
		//Destroy everything remaining
		isBossRoom = false;
		Destroy (currentStairs);
		Destroy (exitStairs);
		Enemy[] list = FindObjectsOfType<Enemy> ();
		for (int i = 0; i < list.Length; i++) {
			Destroy(list[i].gameObject);
		}
		EnemyList = new List<Enemy>();
		Chest[] chestList = FindObjectsOfType<Chest> ();
		for (int i = 0; i < chestList.Length; i++) {
			Destroy (chestList [i].gameObject);
		}
		ChestList = new List<GameObject> ();
		for (int i = 0; i < torchList.Count; i++) {
			Destroy (torchList [i].gameObject);
		}
		torchList = new List<GameObject> ();
		if (currentPlayer.GetComponent<Player> ().level % 5 == 0) {
			width = 30;
			height = 30;
			randomFillPercent = 10;
			isBossRoom = true;
		}
		smallest_size = 0;
		map = new int[width,height];
		RandomFillMap();

		for (int i = 0; i < 5; i ++) {
			SmoothMap();
		}

		ProcessRoomsInMap ();

		int border = 25;
		int[,] borderMap = new int[width+border*2,height+border*2];
		for (int i = 0; i < borderMap.GetLength(0); i++) {
			for (int j = 0; j < borderMap.GetLength(1); j++) {
				if (i >= border && i < width + border && j >= border && j < height + border) {
					borderMap [i, j] = map [i - border, j - border];
				} else {
					borderMap [i, j] = 1;
				}
			
			}
		}

		MeshGenerator mesh = GetComponent<MeshGenerator> ();
		mesh.GenerateMesh (borderMap, 1);

		//EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D> ();
		//edgeCollider.isTrigger = true;
		//mesh.GenerateMesh (borderMap, 1);

	}


	void RandomFillMap() {
		if (useRandomSeed) {
			seed = System.DateTime.Now.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) {
					map[x,y] = 1;
				}
				else {
					map[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1: 0;
				}
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					map[x,y] = 1;
				else if (neighbourWallTiles < 4)
					map[x,y] = 0;

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (InMap(neighbourX, neighbourY)) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}


	List<Coordinates> GetRoomTiles(int startX, int startY){
		List<Coordinates> tileList = new List<Coordinates> ();
		int[,] mapPos = new int[width, height];
		int type = map [startX, startY];

		Queue<Coordinates> queue = new Queue<Coordinates> ();
		queue.Enqueue(new Coordinates(startX, startY));
		mapPos [startX, startY] = 1;
		while (queue.Count > 0) {
			Coordinates tile = queue.Dequeue ();
			tileList.Add (tile);
			for (int i = tile.tileX - 1; i <= tile.tileX + 1; i++) {
				for (int j = tile.tileY - 1; j <= tile.tileY + 1; j++) {
					if (InMap (i, j) && (j == tile.tileY || i == tile.tileX)) {
						if (mapPos [i, j] == 0 && map [i, j] == type) {
							mapPos [i, j] = 1;
							queue.Enqueue (new Coordinates (i, j));
						}
					}
				}
			}

		}
		return tileList;
	}

	//Returns whether or not a point is in the map or out of bounds
	bool InMap(int x, int y){
		return (x >= 0 && x < width && y >= 0 && y < height);
	}


	List<List<Coordinates>> GetRegions(int type){
		List<List<Coordinates>> regions = new List<List<Coordinates>> ();
		int[,] mapPos = new int[width, height];

		//Find the new regions in the map
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				if (mapPos [i, j] == 0 && map [i, j] == type) {
					List<Coordinates> newRegion = GetRoomTiles (i, j);
					regions.Add (newRegion);

					//Set each tile in the new region to visited
					foreach (Coordinates tile in newRegion) {
						mapPos [tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		return regions;
	}


	void ProcessRoomsInMap(){
		List<List<Coordinates>> wallRegion = GetRegions (1);
		int wallSize = 50;
		foreach (List<Coordinates> wall in wallRegion) {
			if (wall.Count < wallSize) {
				foreach (Coordinates tile in wall) {
					map [tile.tileX, tile.tileY] = 0;
				}
			}
		}

		List<List<Coordinates>> roomRegion = GetRegions (0);
		int roomSize = 50;
		List<Room> remainingRooms = new List<Room> ();

		foreach (List<Coordinates> room in roomRegion) {
			if (room.Count < roomSize) {
				foreach (Coordinates tile in room) {
					map [tile.tileX, tile.tileY] = 1;
				}
			} else {
				remainingRooms.Add (new Room (room, map));
			}
		}
		remainingRooms.Sort ();
		remainingRooms[0].isMainRoom = true;
		Vector2 spawn = findSpawn (remainingRooms [0], map, 0);
		bossSpawned = false;
		if (currentPlayer == null) {
			currentPlayer = Instantiate (player, spawn, transform.rotation);
		} else {
			currentPlayer.transform.position = spawn;
		}
		if (isBossRoom == false) {
			currentStairs = Instantiate (stairs, spawn, transform.rotation);
			spawn = findExit (remainingRooms [remainingRooms.Count - 1], map, 0);
			exitStairs = Instantiate (exit, spawn, transform.rotation);
			remainingRooms [remainingRooms.Count - 1].isExitRoom = true;
			smallest_size = remainingRooms [remainingRooms.Count - 1].size;
			remainingRooms [0].accesibleFromMainRoom = true;
			if (spawnEnemies == true) {
				PopulateDungeon (remainingRooms);
			}
			ConnectedClosestRooms (remainingRooms);
		} else {
			spawn = findBossSpawn (remainingRooms [remainingRooms.Count - 1], map, 0);
			Enemy spawnedBoss = Instantiate (boss, spawn, transform.rotation);
			bossSpawned = true;
			currentStairs = Instantiate (stairs, spawn, transform.rotation);
			spawn = findExit (remainingRooms [remainingRooms.Count - 1], map, 0);
			exitStairs = Instantiate (exit, spawn, transform.rotation);
		}
		AddTorches (remainingRooms);
	}


	void ConnectedClosestRooms(List<Room> roomList, bool forceAccessibilityFromMainRoom = false){
		List<Room> roomListA = new List<Room> ();
		List<Room> roomListB = new List<Room> ();
		if (forceAccessibilityFromMainRoom) {
			foreach (Room room in roomList) {
				if (room.accesibleFromMainRoom) {
					roomListB.Add (room);
				} else {
					roomListA.Add (room);
				}
			}
		} else {
			roomListA = roomList;
			roomListB = roomList;
		}

		int smallestDistance= 0;
		Coordinates bestA = new Coordinates ();
		Coordinates bestB = new Coordinates ();
		Room bestRoomA = new Room ();
		Room bestRoomB = new Room ();
		bool connected = false;


		foreach (Room roomA in roomListA) {
			if (!forceAccessibilityFromMainRoom) {
				connected = false;
				if(roomA.connectedRooms.Count > 0 ){
					continue;
				}
			}
			foreach (Room roomB in roomListB) {
				if (roomA == roomB || roomA.IsConnectedRoom(roomB)) {
					continue;
				}
				for (int i = 0; i < roomA.edges.Count; i++) {
					for (int j = 0; j < roomB.edges.Count;j++) {
						Coordinates tileA = roomA.edges [i];
						Coordinates tileB = roomB.edges [j];
						int roomDistance = (int)(Mathf.Pow ((tileA.tileX - tileB.tileX), 2) + Mathf.Pow ((tileA.tileY - tileB.tileY), 2));
						if (roomDistance < smallestDistance || !connected) {
							smallestDistance = roomDistance;
							connected = true;
							bestA = tileA;
							bestB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						} 

					}
				}
			}
			if(connected && !forceAccessibilityFromMainRoom){
				CreateConnection (bestRoomA, bestRoomB, bestA, bestB);	
			}
		}
		if (connected && forceAccessibilityFromMainRoom) {
			CreateConnection (bestRoomA, bestRoomB, bestA, bestB);
			ConnectedClosestRooms (roomList, true);

		}
		if (!forceAccessibilityFromMainRoom) {
			ConnectedClosestRooms (roomList, true);
		}

	}

	void CreateConnection(Room roomA, Room roomB, Coordinates tileA, Coordinates tileB){
		Room.ConnectRooms (roomA, roomB);
		Coordinates tile = new Coordinates ();
		List<Coordinates> line = GetLine (tileA, tileB);
		foreach (Coordinates c in line) {
			DrawCircle (c, 2);
			tile = c;
		}

		int random = UnityEngine.Random.Range (0, 101);
		if (random >= 0 && ((roomB.connectedRooms.Count <= 1 && (roomA.connectedRooms.Count <= 3 && roomA.connectedRooms.Count > 1)) || (roomA.connectedRooms.Count <= 1 && (roomB.connectedRooms.Count <= 3 && roomB.connectedRooms.Count > 1)))
			&& !roomB.isExitRoom && !roomB.isMainRoom && !roomA.isMainRoom && !roomA.isExitRoom && roomB.size <= 200 && roomA.size <= 200
			&& roomA.size > smallest_size && roomB.size > smallest_size) {
			Vector2 spawn = new Vector2 (tile.tileX - width / 2 + 0.5f, tile.tileY - height / 2 + 0.5f);
			if (roomA.connectedRooms.Count <= roomB.connectedRooms.Count) {
				spawn = findSpawn (roomA, map, 0);
			} else {
				spawn = findSpawn (roomB, map, 0);
			}
			GameObject new_chest = Instantiate (chest, spawn, transform.rotation);
			ChestList.Add (new_chest);
		}


	}

	void DrawCircle(Coordinates c, int r){
		for (int i = -r; i <= r; i++) {
			for (int j = -r; j <= r; j++) {
				if (i * i + j * j <= r * r) {
					int realX = c.tileX + i;
					int realY = c.tileY + j;
					if(InMap(realX, realY)){
						map [realX, realY] = 0;
					}
				}
			}
		}
	}

	Vector3 CoordToWorldPoint(Coordinates tile){
		return new Vector3 (-width / 2 + 0.5f + tile.tileX, -height / 2 + 0.5f + tile.tileY, 5);
	}

	public Vector2 findSpawn(Room room, int[,] map, int count){
		foreach (Coordinates tile in room.tiles) {
			if (map [tile.tileX, tile.tileY] == 0 && (count >= 100 || count >= room.size/2)) {
				return new Vector2 (tile.tileX - width/2 + 0.5f, tile.tileY - height/2 +0.5f);
			}
			count++;
		}
		return new Vector2();
	}

	public Vector2 findBossSpawn(Room room, int[,] map, int count){
		foreach (Coordinates tile in room.tiles) {
			if (map [tile.tileX, tile.tileY] == 0 && (count >= room.size/2)) {
				return new Vector2 (tile.tileX - width/2 + 0.5f, tile.tileY - height/2 +0.5f);
			}
			count++;
		}

		return new Vector2();
	}

	Vector2 findExit(Room room, int[,] map, int count){
		foreach (Coordinates tile in room.tiles) {
			if (map [tile.tileX, tile.tileY] == 0 && count >= 10) {
				return new Vector2 (tile.tileX - width/2 + 0.5f, tile.tileY - height/2 +0.5f);
			}
			count++;
		}
		return new Vector2();
	}
		
	List<Coordinates> GetLine(Coordinates from, Coordinates to){
		List<Coordinates> line = new List<Coordinates>();

		int x = from.tileX;
		int y = from.tileY;
		int dx = to.tileX - from.tileX;
		int dy = to.tileY - from.tileY;

		int step = Math.Sign (dx);
		int gradient = Math.Sign (dy);
		bool inverted = false;
		int longest = Mathf.Abs (dx);
		int shortest = Mathf.Abs (dy);
		if (longest < shortest) {
			inverted = true;
			longest = Mathf.Abs (dy);
			shortest = Mathf.Abs (dx);
			step = Math.Sign (dy);
			gradient = Math.Sign (dx);
		}

		int gradientAccumulaion = longest / 2;
		for (int i = 0; i < longest; i++) {
			line.Add(new Coordinates(x,y));
			if (inverted) {
				y += step;
			} else {
				x += step;
			}

			gradientAccumulaion += shortest;
			if (gradientAccumulaion >= longest) {
				if (inverted) {
					x += gradient;
				} else {
					y += gradient;
				}
				gradientAccumulaion -= longest;
			}
		}

		return line;
	}


	void PopulateDungeon(List<Room> roomList){
		int count = 0;
		int index = 0;
		bool first = false;
		foreach (Room room in roomList) {
			count = 0;
			foreach (Coordinates tile in room.tiles) {
				if (count >= 100 && map[tile.tileX, tile.tileY] == 0) {
					if (first != false) {
						Vector2 spawn = new Vector2 (tile.tileX - width / 2 + 0.5f, tile.tileY - height / 2 + 0.5f);
						int random = UnityEngine.Random.Range(0,3);
						Enemy enemy = new Enemy();
						if (random == 0) {
							enemy = Instantiate (enemyArcher, spawn, transform.rotation);
						} else if (random == 1) {
							enemy = Instantiate (enemyLight, spawn, transform.rotation);
						}else if (random == 2) {
							enemy = Instantiate (enemyHeavy, spawn, transform.rotation);
						}
						EnemyList.Add (enemy);
						enemy.index = index;
						index++;
						count = 0;
					} else {
						first = true;
					}
				}
				count++;
			}
		}
	}

	void AddTorches(List<Room> roomList){
		int count = 0;
		foreach (Room room in roomList) {
			count = 0;
			foreach (Coordinates tile in room.tiles) {
				if (count >= 80 && map[tile.tileX, tile.tileY] == 0) {
					Vector2 spawn = new Vector2 (tile.tileX - width / 2 + 0.5f, tile.tileY - height / 2 + 0.5f);
					GameObject newTorch = Instantiate (torch, spawn, transform.rotation);
					torchList.Add (newTorch);
					count = 0;
				}
				count++;
			}
		}
	}

	void AddChests(List<Room> roomList){
		int count = 0;
		foreach (Room room in roomList) {
			count = 0;
			foreach (Coordinates tile in room.tiles) {
				if (room.size >= 500 && count >= 250 && map [tile.tileX, tile.tileY] == 0) {
					Vector2 spawn = new Vector2 (tile.tileX - width / 2 + 0.5f, tile.tileY - height / 2 + 0.5f);
					GameObject new_chest = Instantiate (chest, spawn, transform.rotation);
					ChestList.Add (new_chest);
					count = 0;
					break;
				}
				count++;
			}
		}
	}

	void AddTreasureRoom(Room roomA, Room roomB, Coordinates tileA, Coordinates tileB){
		Coordinates tile = new Coordinates ();
		List<Coordinates> line = GetLine (tileA, tileB);
		foreach (Coordinates c in line) {
			DrawCircle (c, 2);
			tile = c;
		}
//		int random = UnityEngine.Random.Range (0, 101);
//		if (random >= 0 && ((roomB.connectedRooms.Count == 1 && roomA.connectedRooms.Count <= 2)) || ((roomA.connectedRooms.Count == 1 && roomB.connectedRooms.Count <= 2))
//			&& !roomB.isExitRoom && !roomB.isMainRoom && !roomA.isMainRoom && !roomA.isExitRoom && (roomB.size <= 200)) {
			Vector2 spawn = new Vector2 (tile.tileX - width / 2 + 0.5f, tile.tileY - height / 2 + 0.5f);
//			GameObject gate = Instantiate (wall, spawn, transform.rotation);
//			LockedDoorList.Add (gate);
			if (roomA.connectedRooms.Count < roomB.connectedRooms.Count) {
				spawn = findSpawn (roomA, map, 0);
			} else {
				spawn = findSpawn (roomB, map, 0);
			}
			GameObject new_chest = Instantiate (chest, spawn, transform.rotation);
			ChestList.Add (new_chest);
//			Debug.DrawLine (CoordToWorldPoint (tileA), CoordToWorldPoint (tileB), Color.green, 100);
//		}
	}
		

	public struct Coordinates{
		public int tileX;
		public int tileY;

		public Coordinates(int x, int y){
			tileX = x;
			tileY = y;
		}
	}

	public class Room: IComparable<Room>{
		public List<Coordinates> tiles;
		public List<Coordinates> edges;
		public List<Room> connectedRooms;
		public bool accesibleFromMainRoom;
		public bool isMainRoom;
		public int size;
		public bool isTreasureRoom = false;
		public bool isExitRoom = false;

		public Room(List<Coordinates> roomTiles, int[,] map){
			tiles = roomTiles;
			size = tiles.Count;
			connectedRooms = new List<Room>();

			edges = new List<Coordinates>();
			foreach(Coordinates tile in tiles){
				for(int i = tile.tileX - 1; i < tile.tileX + 1; i++){
					for(int j = tile.tileY - 1; j < tile.tileY + 1; j++){
						if(i == tile.tileX || j == tile.tileY){
							if(map[i,j] == 1){
								edges.Add(tile);
							}
						}
					}
				}

			}

		}

		public Room(){
			
		}

		public static void ConnectRooms(Room A, Room B){
			if (A.accesibleFromMainRoom) {
				B.SetAccessibleFromMainRoom ();
			} else if (B.accesibleFromMainRoom) {
				A.SetAccessibleFromMainRoom ();
			}
			A.connectedRooms.Add (B);
			B.connectedRooms.Add (A);

		}

		public bool IsConnectedRoom(Room other){
			return connectedRooms.Contains (other);
		}

		public int CompareTo(Room other){
			return other.size.CompareTo (size);
		}

		public void SetAccessibleFromMainRoom(){
			if (!accesibleFromMainRoom) {
				accesibleFromMainRoom = true;
				foreach (Room connected in connectedRooms) {
					connected.SetAccessibleFromMainRoom ();
				}
			}
		}
	}
}