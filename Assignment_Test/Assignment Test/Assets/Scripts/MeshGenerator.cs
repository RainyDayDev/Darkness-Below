using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	public SquareGrid squareGrid;
	public MeshFilter walls;
	List<Vector3> vertices;
	List<int> triangles;
	Dictionary<int, List<Triangle>> triangleDict = new Dictionary<int, List<Triangle>>();

	List<List<int>> outlines = new List<List<int>>();
	HashSet<int> checkedVertices = new HashSet<int> ();

	public MeshFilter cave;

	public bool is2D;

	public void GenerateMesh(int[,] map, float squareSize){
		triangleDict.Clear ();
		outlines.Clear ();
		checkedVertices.Clear ();


		squareGrid = new SquareGrid (map, squareSize);
		vertices = new List<Vector3> ();
		triangles = new List<int> ();
		for (int i = 0; i < squareGrid.squares.GetLength (0); i++) {
			for (int j = 0; j < squareGrid.squares.GetLength (1); j++) {
				TriangulateSquare(squareGrid.squares[i,j]);
			}
		}


		Mesh mesh = new Mesh ();
		cave.mesh = mesh;

		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();
		int tileAmount = 10;
		Vector2[] uvs = new Vector2[vertices.Count];
		for (int i = 0; i < vertices.Count; i++) {
			float percentX = Mathf.InverseLerp (-map.GetLength (0) / 2 * squareSize, map.GetLength (0) / 2 * squareSize, vertices [i].x) * tileAmount;
			float percentY = Mathf.InverseLerp (-map.GetLength (0) / 2 * squareSize, map.GetLength (0) / 2 * squareSize, vertices [i].y) * tileAmount;
			uvs [i] = new Vector2 (percentX, percentY);
		}
		mesh.uv = uvs;

		if (is2D) {
			Generate2DColliders ();
		} else {
			CreateWalls ();
		}
	}

	void CreateWalls(){
		MeshOutline ();
		List<Vector3> wallList = new List<Vector3> ();
		List<int> wallTriangle = new List<int> ();
		Mesh wallMesh = new Mesh ();
		float wallHeight = 5;
		foreach (List<int> outline in outlines) {
			for (int i = 0; i < outline.Count - 1; i++) {
				int startIndex = wallList.Count;
				wallList.Add (vertices[outline[i]]); //Left vertex
				wallList.Add (vertices[outline[i+1]]); //Right vertex
				wallList.Add (vertices[outline[i]] + Vector3.forward * wallHeight); //Bottom Left vertex
				wallList.Add (vertices[outline[i+1]] + Vector3.forward * wallHeight); //Bottom Right vertex

				wallTriangle.Add (startIndex);
				wallTriangle.Add (startIndex + 2);
				wallTriangle.Add (startIndex + 3);

				wallTriangle.Add (startIndex + 3);
				wallTriangle.Add (startIndex + 1);
				wallTriangle.Add (startIndex);


			} 
		}

		wallMesh.vertices = wallList.ToArray ();
		wallMesh.triangles = wallTriangle.ToArray ();
		walls.mesh = wallMesh;

		MeshCollider collider = walls.gameObject.AddComponent<MeshCollider> ();
		collider.sharedMesh = wallMesh;
	}


	void TriangulateSquare(Square square){
		switch (square.config) {
		case 0:
			break;
		//1 point
		case 1:
			MeshFromPoints (square.midLeft, square.midBot, square.botLeft);
			break;
		case 2:
			MeshFromPoints (square.botRight, square.midBot, square.midRight);
			break;
		case 4:
			MeshFromPoints (square.topRight, square.midRight, square.midTop);
			break;
		case 8:
			MeshFromPoints (square.topLeft, square.midTop, square.midLeft);
			break;


		//2 point
		case 3:
			MeshFromPoints (square.midRight, square.botRight, square.botLeft, square.midLeft);
			break;
		case 6:
			MeshFromPoints (square.midTop, square.topRight, square.botRight, square.midBot);
			break;
		case 9:
			MeshFromPoints (square.topLeft, square.midTop, square.midBot, square.botLeft);
			break;
		case 12:
			MeshFromPoints (square.topLeft, square.topRight, square.midRight, square.midLeft);
			break;
		case 5:
			MeshFromPoints (square.midTop, square.topRight, square.midRight, square.midBot, square.botLeft, square.midLeft);
			break;
		case 10:
			MeshFromPoints (square.topLeft, square.midTop, square.midRight, square.botRight, square.midBot, square.midLeft);
			break;

		//3 point
		case 7:
			MeshFromPoints (square.midTop, square.topRight, square.botRight, square.botLeft, square.midLeft);
			break;
		case 11:
			MeshFromPoints (square.topLeft, square.midTop, square.midRight, square.botRight, square.botLeft);
			break;
		case 13:
			MeshFromPoints (square.topLeft, square.topRight, square.midRight, square.midBot, square.botLeft);
			break;
		case 14:
			MeshFromPoints (square.topLeft, square.topRight, square.botRight, square.midBot, square.midLeft);
			break;

		case 15:
			MeshFromPoints (square.topLeft, square.topRight, square.botRight, square.botLeft);
			checkedVertices.Add (square.topLeft.vertexIndex);
			checkedVertices.Add (square.topRight.vertexIndex);
			checkedVertices.Add (square.botRight.vertexIndex);
			checkedVertices.Add (square.botLeft.vertexIndex);
			break;
		}
	}

	void MeshFromPoints(params Node[] points){
		AssignVertices (points);

		if (points.Length >= 3) {
			CreateTriangle (points[0], points[1], points[2]);
		}
		if (points.Length >= 4) {
			CreateTriangle (points [0], points [2], points [3]);
		}
		if (points.Length >= 5) {
			CreateTriangle (points [0], points [3], points [4]);
		}
		if (points.Length >= 6) {
			CreateTriangle (points [0], points [4], points [5]);
		}

	}

	void AssignVertices(Node[] points){
		for (int i = 0; i < points.Length; i++) {
			if (points [i].vertexIndex == -1) {
				points [i].vertexIndex = vertices.Count;
				vertices.Add (points [i].position);
			}
		}


	}

	void CreateTriangle(Node a, Node b, Node c){
		triangles.Add (a.vertexIndex);
		triangles.Add (b.vertexIndex);
		triangles.Add (c.vertexIndex);

		Triangle triangle = new Triangle (a.vertexIndex, b.vertexIndex, c.vertexIndex);
		AddTriangleToDict (triangle.vertexIndexA, triangle);
		AddTriangleToDict (triangle.vertexIndexB, triangle);
		AddTriangleToDict (triangle.vertexIndexC, triangle);

	}

	void AddTriangleToDict(int key, Triangle triangle){
		if (triangleDict.ContainsKey (key)) {
			triangleDict [key].Add (triangle);
		} else {
			List<Triangle> list = new List<Triangle> ();
			list.Add (triangle);
			triangleDict.Add (key, list);
		}
	}

	bool isEdge(int a, int b){
		List<Triangle> listA = triangleDict [a];
		int sharedTriangles = 0;
		for (int i = 0; i < listA.Count; i++) {
			if (listA [i].doesContain (b)) {
				sharedTriangles++;
				if (sharedTriangles > 1) {
					break;
				}
			} 
		}

		return sharedTriangles == 1;
		
	}

	int GetConnectedVertex(int vertex){
		List<Triangle> list = triangleDict [vertex];
		for (int i = 0; i < list.Count; i++) {
			Triangle triangle = list [i];
			for (int j = 0; j < 3; j++) {
				int vertexB = triangle.getVertex(j);
				if (vertexB != vertex && !checkedVertices.Contains(vertexB)) {
					if(isEdge(vertex, vertexB)){
						return vertexB;
					}
				}
			}
		}

		return -1;
	}

	void MeshOutline(){
		for (int i = 0; i < vertices.Count; i++) {
			if (!checkedVertices.Contains (i)) {
				int outline = GetConnectedVertex (i);
				if (outline != -1) {
					checkedVertices.Add (i);

					List<int> newOutline = new List<int> ();
					newOutline.Add (i);
					outlines.Add (newOutline);
					FollowOutline (outline, outlines.Count -1);
					outlines [outlines.Count - 1].Add (i);

				}
			}
		}
	}

	void FollowOutline(int index, int outline){
		outlines [outline].Add (index);
		checkedVertices.Add (index);
		int next = GetConnectedVertex (index);
		if (next != -1) {
			FollowOutline (next, outline);
		}
	}

	void Generate2DColliders(){
		EdgeCollider2D[] remainingEdges = gameObject.GetComponents<EdgeCollider2D> ();
		for (int i = 0; i < remainingEdges.Length; i++) {
			Destroy (remainingEdges [i]);
		}

		MeshOutline ();
		foreach (List<int> outline in outlines) {
			EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
			Vector2[] points = new Vector2[outline.Count];

			for (int i = 0; i < outline.Count; i++) {
				points [i] = vertices [outline [i]];
			}
			edge.points = points;
		}
	
	}



	public class Node{
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 pos){
			position = pos;
		}

	}

	public class ControlNode: Node{
		public bool isActive;
		public Node above;
		public Node right;
		public ControlNode(Vector3 pos, bool active, float size): base(pos) {
			isActive = active;
			above = new Node(position + Vector3.up * size/2f);
			right = new Node(position + Vector3.right * size/2f);


		}

	}


	public class Square{
		public ControlNode topLeft, topRight, botLeft, botRight;
		public Node midTop, midBot, midLeft, midRight;

		public int config;

		public Square(ControlNode topleft, ControlNode topright, ControlNode botleft, ControlNode botright){
			topLeft = topleft;
			topRight = topright;
			botLeft = botleft;
			botRight = botright;

			midTop = topLeft.right;
			midBot = botLeft.right;
			midLeft = botLeft.above;
			midRight = botRight.above;

			if(topLeft.isActive){
				config += 8;
			}
			if(topRight.isActive){
				config += 4;
			}
			if(botRight.isActive){
				config += 2;
			}
			if(botLeft.isActive){
				config += 1;
			}
		}

	}

	public class SquareGrid{
		public Square[,] squares;

		public SquareGrid(int[,] map, float squareSize){
			int xNode = map.GetLength(0);
			int yNode = map.GetLength(1);
			float mapWidth = xNode * squareSize;
			float mapHeight = yNode * squareSize;
			ControlNode[,] controlNodes = new ControlNode[xNode, yNode];


			for(int i = 0; i < xNode; i++){
				for(int j = 0; j < yNode; j++){
					Vector3 position = new Vector3(-mapWidth/2 + i * squareSize + squareSize/2, -mapHeight/2 + j * squareSize + squareSize/2, 0);
					controlNodes[i,j] = new ControlNode(position, map[i,j] == 1, squareSize);

				}
			}

			squares = new Square[xNode-1,yNode-1];
			for(int i = 0; i < xNode-1; i++){
				for(int j = 0; j < yNode-1; j++){
					squares[i,j] = new Square(controlNodes[i, j+1], controlNodes[i+1,j+1],controlNodes[i,j], controlNodes[i+1,j] );
				
				}
			}
		}

	}

	struct Triangle{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int[] vertices;
		public Triangle(int a, int b, int c){
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;

			vertices = new int[3];
			vertices[0] = a;
			vertices[1] = b;
			vertices[2] = c;
		}

		public int getVertex(int i){
			return vertices [i];
		}
		public bool doesContain(int Vertex){
			return Vertex == vertexIndexA || Vertex == vertexIndexB || Vertex == vertexIndexC;
		}
	}


}
