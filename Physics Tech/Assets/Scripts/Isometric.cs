using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUtils;

[ExecuteInEditMode]
[SerializeField]
public class isoPoint
{
	public float x,y,rad;
	public Color col;
	
	public isoPoint(float _x, float _y, Color _col, float _rad)
	{
		x = _x;
		y = _y;
		col = _col;
		rad = _rad;
	}
	
	public void drawPoint()
	{
		Gizmos.color = col;
		Gizmos.DrawSphere(new Vector3(x, y, MouseScreenInfo.screenDim.z), rad);
		Gizmos.color = default(Color);
	}
	
	public void drawLine(isoPoint p)
	{
		Gizmos.color = col;
		Gizmos.DrawLine(new Vector3(x, y, MouseScreenInfo.screenDim.z),
						new Vector3(p.x, p.y, MouseScreenInfo.screenDim.z));
		Gizmos.color = col;
	}
	
	public void updatePoint(float _x, float _y)
	{
		x = _x;
		y = _y;
	}
}
[ExecuteInEditMode]
[SerializeField]
public class isoTile
{
	isoPoint p0, p1, p2, p3;
	float x, y;
	float tileWidth, tileHeight;
	Color col;
	
	public isoTile(float _tileWidth, float _tileHeight, Color _col, float _x, float _y, float _pRad)
	{
		x = _x;
		y = _y;
		tileWidth = _tileWidth;
		tileHeight = _tileHeight;
		col = _col;
		
		float xCoord = (_x - _y) * tileWidth / 2;
		float yCoord = (_x + _y) * tileHeight / 2;
		
		p0 = new isoPoint(xCoord, yCoord, _col, _pRad);
		p1 = new isoPoint(xCoord + tileWidth /2 , yCoord + tileHeight / 2 , _col, _pRad);
		p2 = new isoPoint(xCoord, yCoord + tileHeight, _col, _pRad);
		p3 = new isoPoint(xCoord - tileWidth / 2, yCoord + tileHeight / 2, _col, _pRad);
	}
	
	public void drawTile()
	{
		p0.drawPoint();
		p0.drawLine(p1);
		p1.drawPoint();
		p1.drawLine(p2);
		p2.drawPoint();
		p2.drawLine(p3);
		p3.drawPoint();
		p3.drawLine(p0);
	}
}
[ExecuteInEditMode]
[SerializeField]
public class isoBlock
{
	//top Points
	public isoPoint p0, p1, p2, p3;
	
	//left Points
	public isoPoint p4, p5, p6, p7;
	
	//right Points
	public isoPoint p8, p9, p10, p11;
	
	public float x, y, z;
	float tileWidth, tileHeight;
	Color col;
	public isoBlock(float _tileWidth, float _tileHeight, Color _col, float _x, float _y, float _z, float _pRad)
	{
		x = _x;
		y = _y;
		z = _z;
		
		tileWidth = _tileWidth;
		tileHeight = _tileHeight;
		col = _col;
		
		float xCoord = (_x - _y) * tileWidth / 2;
		float yCoord = (_x + _y) * tileHeight / 2;
		
		float zNormR = Mathf.Abs(g_utilities.map(z, 0, 4, 0.137f, 0.137f));
		
		float zNormG = Mathf.Abs(g_utilities.map(z, 0, 4, 0.243f, 0.984f));
		
		float zNormB = Mathf.Abs(g_utilities.map(z, 0, 4, 0.984f, 0.884f));
		
		float zNormA = Mathf.Abs(g_utilities.map(z, 0, 4, 0, 1));
		
		Color top = new Color(zNormR, zNormG, zNormB, .1f + zNormA);
		Color left = new Color(zNormR, zNormG, zNormB, .1f + zNormA);
		Color right = new Color(zNormR, zNormG, zNormB, .1f + zNormA);
		
		//draw Top
		p0 = new isoPoint(xCoord, yCoord + z * tileHeight, top, _pRad);
		p1 = new isoPoint(xCoord + tileWidth /2 , yCoord + tileHeight / 2 + z * tileHeight , top, _pRad);
		p2 = new isoPoint(xCoord, yCoord + tileHeight + z * tileHeight, top, _pRad);
		p3 = new isoPoint(xCoord - tileWidth / 2, yCoord + tileHeight / 2 + z * tileHeight, top, _pRad);
		
		//draw left
		p4 = new isoPoint(xCoord - tileWidth / 2, yCoord + tileHeight / 2 + z * tileHeight, left, _pRad);
		p5 = new isoPoint(xCoord, yCoord + z * tileHeight , left, _pRad);
		p6 = new isoPoint(xCoord, yCoord, left, _pRad);
		p7 = new isoPoint(xCoord - tileWidth / 2, yCoord + tileHeight / 2, left, _pRad);
		
		//draw right
		p8 = new isoPoint(xCoord + tileWidth /2, yCoord + tileHeight / 2 + z * tileHeight, right, _pRad);		
		p9 = new isoPoint(xCoord, yCoord + z * tileHeight, right, _pRad);	
		p10 = new isoPoint(xCoord, yCoord, right, _pRad);	
		p11 = new isoPoint(xCoord + tileWidth / 2, yCoord + tileHeight / 2, right, _pRad);
	}
	
	public void drawBlock()
	{
		
		
		// //Right Tile
		p8.drawPoint();
		p8.drawLine(p9);
		p9.drawPoint();
		p9.drawLine(p10);
		p10.drawPoint();
		p10.drawLine(p11);
		p11.drawPoint();
		p11.drawLine(p8);
		
		p8.drawLine(p10);
		p9.drawLine(p11);
		
		// //Left Tile
		p4.drawPoint();
		p4.drawLine(p5);
		p5.drawPoint();
		p5.drawLine(p6);
		p6.drawPoint();
		p6.drawLine(p7);
		p7.drawPoint();
		p7.drawLine(p4);
		
		p4.drawLine(p6);
		p5.drawLine(p7);
		
		//Top Tile
		p0.drawPoint();
		p0.drawLine(p1);
		p1.drawPoint();
		p1.drawLine(p2);
		p2.drawPoint();
		p2.drawLine(p3);
		p3.drawPoint();
		p3.drawLine(p0);
		
		p0.drawLine(p2);
		p3.drawLine(p1);
	}
	
	public void updateBlock()
	{

		float xCoord = (x - y) * tileWidth / 2;
		float yCoord = (x + y) * tileHeight / 2;
		
		float zNormR = g_utilities.map(z, 0, 4, 0.137f, 0.137f);
		
		float zNormG = g_utilities.map(z, 0, 4, 0.243f, 0.984f);
		
		float zNormB = g_utilities.map(z, 0, 4, 0.984f, 0.884f);
		
		float zNormA = g_utilities.map(z, 0, 4, 0, 1);
		
		Color top = new Color(zNormR, zNormG, zNormB, .1f + zNormA);
		Color left = new Color(zNormR, zNormG, zNormB, .1f + zNormA);
		Color right = new Color(zNormR, zNormG, zNormB, .1f + zNormA);
		
		p0.updatePoint(xCoord, yCoord + z * tileHeight);
		p0.col = top;
		p1.updatePoint(xCoord + tileWidth /2 , yCoord + tileHeight / 2 + z * tileHeight);
		p1.col = top;
		p2.updatePoint(xCoord, yCoord + tileHeight + z * tileHeight);
		p2.col = top;
		p3.updatePoint(xCoord - tileWidth / 2, yCoord + tileHeight / 2 + z * tileHeight);
		p3.col = top;
		//draw left
		p4.updatePoint(xCoord - tileWidth / 2, yCoord + tileHeight / 2 + z * tileHeight);
		p4.col = left;
		p5.updatePoint(xCoord, yCoord + z * tileHeight);
		p5.col = left;
		p6.updatePoint(xCoord, yCoord);
		p6.col = left;
		p7.updatePoint(xCoord - tileWidth / 2, yCoord + tileHeight / 2);
		p7.col = left;
		
		//draw right
		p8.updatePoint(xCoord + tileWidth /2, yCoord + tileHeight / 2 + z * tileHeight);
		p8.col = right;
		p9.updatePoint(xCoord, yCoord + z * tileHeight);	
		p9.col = right;
		p10.updatePoint(xCoord, yCoord);	
		p10.col = right;
		p11.updatePoint(xCoord + tileWidth / 2, yCoord + tileHeight / 2);
		p11.col = right;

	}
}
[ExecuteInEditMode]
[SerializeField]
public class isoImageTile
{
	public GameObject tileSP;
	public int index;
	float x, y, z;
	float tileWidth, tileHeight;
	public bool debugMode;
	public SpriteRenderer tileSpr;
	
	public isoImageTile(Sprite _img, float _tileWidth, float _tileHeight, float _x, float _y, int _ind)
	{
		x = _x;
		y = _y;
		index = _ind;
		
		tileWidth = _tileWidth;
		tileHeight = _tileHeight;
		
		tileSP = new GameObject();
		tileSP.name = "tile" + _ind;
		
		
		tileSP.AddComponent<SpriteRenderer>();
		
		//GameObject textObj = new GameObject();
		//textObj.AddComponent<GUIText>();
		//GUIText text = textObj.GetComponent<GUIText>();
		//text.text = "X : " + x.ToString() + "Y : " + y.ToString();
		//text.fontSize = 5;
		
		tileSpr = tileSP.GetComponent<SpriteRenderer>();
		tileSpr.sprite = _img;
		
		float xCoord = (_x - _y) * tileWidth / 2;
		float yCoord = ((_x + _y) * tileHeight / 2) - (_ind < 4 ? 8 : 0);
		
		//text.pixelOffset = new Vector2(20, 20);
		//text.color = Color.red;
		
		tileSP.transform.position = (new Vector3(xCoord, yCoord, MouseScreenInfo.screenDim.z));
		//textObj.transform.position = Camera.main.WorldToViewportPoint(new Vector3(xCoord, yCoord, MouseScreenInfo.screenDim.z));
		
	}
	public void zSort(int z, int max)
	{ 
		Color tilecol = tileSP.GetComponent<SpriteRenderer>().color ;
		
		float zNormR = g_utilities.map((float)z, 0, max, .2f, tilecol.r);
		float zNormG = g_utilities.map((float)z, 0, max, .2f, tilecol.g);
		float zNormB = g_utilities.map((float)z, 0, max, .2f, tilecol.b);
		float zNormA = g_utilities.map((float)z, 0, max, .2f, 1);
		
		
		
		tileSP.GetComponent<SpriteRenderer>().sortingOrder = (int) z;
		
		if(index != 0 && index != 1 && index != 2 && index != 3)
		{
			tileSP.GetComponent<SpriteRenderer>().color = new Color(zNormR, zNormG, zNormB, 1);
		}
		else
		{
			float indexNormA = g_utilities.map((float)index, 0, 4, .2f, 1);
			
			tileSP.GetComponent<SpriteRenderer>().color = new Color(zNormR, zNormG, zNormB, indexNormA);
		}
	}
	
	public void setParent(GameObject obj)
	{
		tileSP.transform.parent = obj.transform;
	}
	public void drawDebugImageTile()
	{
		if(debugMode) Gizmos.DrawSphere(new Vector3(x, y, MouseScreenInfo.screenDim.z), 5);
	}
}

[ExecuteInEditMode]
[SerializeField]
public class isoChar
{
	public GameObject charObj;
	Sprite charSpr;
	public SpriteRenderer sprRend;
	int index;
	float x, y, z;
	public float tileWidth, tileHeight;
	public Texture2D imageSet;
	
	public isoChar(Texture2D _imgSet, float _tileWidth, float _tileHeight, float _x, float _y, int _ind, GameObject _plyr)
	{
		x = _x;
		y = _y;
		index = _ind;
		imageSet = _imgSet;
		
		tileWidth = _tileWidth;
		tileHeight = _tileHeight;
		
		charObj = _plyr;
		charObj = new GameObject();
		charObj.name = "Char";
		
		charObj.AddComponent<SpriteRenderer>();
		sprRend = charObj.GetComponent<SpriteRenderer>();
		
		sprRend.sprite = g_utilities.getSprite(_imgSet, _ind, (int)tileWidth, (int)tileHeight);
		
		sprRend.sortingOrder = 1000;
	
		
		float xCoord = (_x - _y) * tileWidth / 2;
		float yCoord = (_x + _y) * tileHeight / 2;

		charObj.transform.position = (new Vector3(xCoord, yCoord + 10, MouseScreenInfo.screenDim.z));
	}
	
	public void destroyPlyr()
	{
		charObj = null;
	}
	
	public void animSprite(int _ind)
	{
		sprRend.sprite = g_utilities.getSprite(imageSet, _ind, (int)tileWidth, (int)tileHeight);
	}
	
	public void moveChar(int _x, int _y)
	{
		float xCoord = (_x - _y) * tileWidth / 2;
		float yCoord = (_x + _y) * tileHeight / 2;
				
		charObj.transform.position = (new Vector3(xCoord, yCoord + 10, MouseScreenInfo.screenDim.z));
		
	}
}

[SerializeField]
[ExecuteInEditMode]
public class Isometric : MonoBehaviour 
{
	public int tileHeight, tileWidth, charX, charY;
	public int gridHeight, gridWidth, size, i;
	
	public List<isoTile> isoGrid = null;
	public List<isoImageTile> isoImgGrid = null;
	public List<isoBlock> isoBlockGrid = null;
	
	public string inputString;
	public bool isWave, isImage, isBlock, isDebug, animate, isPredifined, isGen;
	public Texture2D tileSet;
	public Texture2D plyrSet;
	public GameObject isoGridObj;
	public GridModel gridModel01;
	public isoChar plyr;
	public GameObject player0;
	public Camera charCam;
	
            public int[][] GridModel = new int[][]
        {
           new int[]  {15, 15, 15, 14, 13, 10, 3, 2, 1, 0},
           new int[]  {15, 15, 14, 13, 10, 10, 3, 2, 1, 0},
           new int[]  {15, 14, 13, 10, 10, 3, 3, 2, 1, 0},
           new int[]  {14, 13, 10, 9, 3, 3, 2, 1, 0, 0},
           new int[]  {13, 10, 9, 7, 3, 2, 1, 0, 0, 0},
           new int[]  {10, 9, 7, 6, 3, 2, 1, 0, 0, 0},
           new int[]  {9, 7, 6, 5, 3, 2, 1, 1, 1, 1},
           new int[]  {7, 6, 5, 3, 3, 2, 2, 2, 2, 2},
           new int[]  {6, 5, 5, 3, 3, 3, 3, 3, 3, 3},
           new int[]  {5, 5, 5, 5, 5, 5, 5, 5, 5, 3},
        };
		
	void Start () 
	{	
		//"TileSet01"
		// 12 points
		// 18 lines 
		tileSet =	g_utilities.loadImage(inputString);
		plyrSet = g_utilities.loadImage("CharSpr");
		charX = 9;
		charY = 0;
		if(!isImage && !isBlock && !isDebug)
		{
			isDebug = true;
		}
		
		size = gridHeight * gridWidth;
	}
	
	public void Delete()
	{
		i = 0;
		isGen = false;
		
		isoGrid = null;
		isoBlockGrid = null;
		isoImgGrid = null;
		
		isoImgGrid = new List<isoImageTile>();				
		isoGrid = new List<isoTile>();
		isoBlockGrid = new List<isoBlock>();
		
		
		if(isoGridObj != null)
		{
			DestroyImmediate(isoGridObj);
		
		}
	}
	
	public void DeletePlayer()
	{
	
		DestroyImmediate(plyr.charObj);
		DestroyImmediate(player0);
		plyr = null;
	}
	public void createGrid()
	{
		
			Delete();
			tileHeight = 50;
			tileWidth = 100;
			
			isGen = true;
			
			if(isoGridObj != null)
			{
				DestroyImmediate(isoGridObj);
			}
			
			if(isImage)
			{
				isoGridObj = new GameObject();
				isoGridObj.name = "isoGrid";
				isoGridObj.transform.position = Vector3.zero;
			}
		
		
			createPlayer(charX, charY, 0);
			
			if(isPredifined)
			{
				for(int x = 0; x < GridModel[0].Length; x++)
				{
					int[] row = GridModel[x];
					
					for(int y = 0; y < row.Length; y++)
					{	
						if(isBlock)
						{
							createBlockGrid(x,y, row[y]);
						}
						else if(isDebug)
						{
							createDebugGrid(x,y, parseModel(row[y]));
						}
						else if(isImage)
						{
							createSpriteGrid(x,y, row[y]);
						}
						
					}
				}
			}		
			else
			{
				for(int x = 0; x < gridWidth; x++)
				{
					for(int y = 0; y < gridHeight; y++)
					{
						if(isBlock)
						{
							createBlockGrid(x, y, (int)g_utilities.randomRange(0,5));
						}
						else if(isDebug)
						{
							createDebugGrid(x, y, g_utilities.RandomColor());
						}
						else if(isImage)
						{
							int _index = (int)g_utilities.randomRange(0, ((tileSet.width) / tileWidth));
		
							createSpriteGrid(x,y, _index);
						}
				
					}
				}
			}
			
			if(isImage)
			{	
				for(int i = 0; i < isoImgGrid.Count; i++)
				{
					isoImgGrid[i].zSort(i, isoImgGrid.Count);
				}	
			}
		
	}
	
	public Color parseModel(int i)
	{
		Color col = new Color();
			if(i == 1)
			{
				col = Color.blue;
			}
			else if(i == 2)
			{
				col = Color.red;
			}
			else if (i == 3)
			{
				col = Color.yellow;
			}
		return col;
	}
	
	void createDebugGrid(int x, int y, Color _col)
	{
		isoGrid.Add(new isoTile(tileWidth, tileHeight, _col,
		x, y, 1));
	}
	
	void createBlockGrid(int x, int y, int _ind)
	{
		if(isWave)
		{
			i++;
			float dx = gridWidth/2 - x;
			float dy = gridHeight/2 - y;	
			float dist = Mathf.Sqrt(dx * dx + dy * dy);
			float z = Mathf.Cos(dist * 0.75f) * 2 + 2;
			isoBlockGrid.Add(new isoBlock(tileWidth, tileHeight, g_utilities.RandomColor(),
			-x, -y, z, 1));
		}
		else
		{
			isoBlockGrid.Add(new isoBlock(tileWidth, tileHeight, g_utilities.RandomColor() 
			, -x, -y, _ind, 1));
		}

	}
	void createSpriteGrid(int x, int y, int _ind)
	{

		tileHeight = 32;
		tileWidth = 64;	
	
		isoImageTile isoImg = new isoImageTile(g_utilities.getSprite(tileSet, _ind, tileWidth, tileHeight), tileWidth, tileHeight, 
		-x, -y, _ind);
		
		isoImg.setParent(isoGridObj);
		isoImgGrid.Add(isoImg);	

	}
 	void createPlayer(int x, int y, int _ind)
	{
		tileHeight = 32;
		tileWidth = 64;
		
		plyr = new isoChar(plyrSet, tileWidth, tileHeight, -x, -y, _ind, player0);
		player0 = plyr.charObj;
	}
		
	void animateGrid()
	{
		if(isoBlockGrid != null && isWave)
		{
			for(int i = 0; i < isoBlockGrid.Count; i++)
			{	
						float dx = gridWidth/2 - isoBlockGrid[i].x;
						float dy = gridHeight/2 - isoBlockGrid[i].y;
						
						float dist = Mathf.Sqrt(dx * dx + dy * dy);
						
						float z = Mathf.Cos((dist/4) * (0.75f) + Time.time) * 2 + 2;
						isoBlockGrid[i].z = z;
						isoBlockGrid[i].updateBlock();
			}
		}
	}
	void LateUpdate()
	{
		if(player0 != null)
		{
			charCam.transform.position = new Vector3(player0.transform.position.x + plyr.tileWidth/2 ,player0.transform.position.y + plyr.tileHeight, 0);
		}
	}
	// Update is called once per frame
	void Update () 
	{		
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			//Debug.Log("Pressing Up");
			plyr.animSprite(3);
			if(CanMove(charX, charY - 1))
			{
			charY--;
			plyr.moveChar(-charX, -charY);
			}
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			//Debug.Log("Pressing Right");
			plyr.animSprite(2);
			if(CanMove(charX - 1, charY))
			{
			charX--;
			plyr.moveChar(-charX, -charY);
			}
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			//Debug.Log("Pressing Down");
			plyr.animSprite(0);
			if(CanMove(charX, charY + 1))
			{
			charY++;
			plyr.moveChar(-charX, -charY);
			}
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			//Debug.Log("Pressing Left");
			plyr.animSprite(1);
			if(CanMove(charX + 1, charY))
			{
			charX++;
			plyr.moveChar(-charX, -charY);
			}
		}
		
		if(isBlock && isWave && animate)
		{
			animateGrid();
		}

	}
	
	bool CanMove(int _x, int _y)
	{
		int x = _x;
		int y = _y;

		if(y < 0 || y >= GridModel[0].Length)
		{
			return false;
		}
		if(x < 0 || x >= GridModel[y].Length)
		{
			return false;
		}
		 int tileRef = GridModel[x][y];

		 Debug.Log(GridModel[x][y]);

			if(tileRef < 4 || tileRef > 14)
			{
				return false;
			}
	
		return true;
	}
	
	void OnApplicationQuit()
	{
	
	}
	void OnDrawGizmos()
	{
		if(!Application.isPlaying && plyr != null || !Application.isPlaying && player0 != null )
		{
		DeletePlayer();
		Debug.Log("Quitted");
		}
		
		float size;
		
			if(isPredifined)
			{
				size = GridModel[0].Length * GridModel[1].Length;
	
			}
			else
			{
				 size =  gridHeight * gridWidth;
			}	
			
				if(isGen)
				{
					for(int i = 0; i < size; i++)
					{
						if (isDebug)
						{
							isoGrid[i].drawTile();
						}
						else if(isBlock)
						{
							isoBlockGrid[i].drawBlock();
						}
						else if(isImage)
						{
							//isoImgGrid[i].drawDebugImageTile();
						}
					} 
				}
			}
	}
