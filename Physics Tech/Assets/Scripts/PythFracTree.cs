using UnityEngine;
using System.Collections;
using GUtils;

public class pythPoint
{
	public float x, y, rad;
	public float origx, origy;
	public Color col;
	
	public pythPoint(float _x, float _y, float _rad, Color _col)
	{
		x = _x;
		y = _y;
		
		origx = _x;
		origy = _y;
		
		rad = _rad;
		col = _col;
	}
	
	public void drawPoint()
	{
		Gizmos.color = col;
		Gizmos.DrawWireSphere(new Vector3(x,y,MouseScreenInfo.screenDim.z), rad);
	}
	
	public void drawLine(pythPoint p)
	{
		Gizmos.color = col;
		Gizmos.DrawLine(new Vector3(x,y,MouseScreenInfo.screenDim.z),
						new Vector3(p.x, p.y, MouseScreenInfo.screenDim.z));
	}
	
	public void rotatePoint(float angle, pythPoint p)
	{	
		float distance = GetDistance(p);
		float curAngle = GetAngle(p) + angle;
		
		x = p.x + Mathf.Cos(curAngle) * distance;
		y = p.y + Mathf.Sin(curAngle) * distance;
	}
	
	float GetAngle(pythPoint p)
	{
		float dx = x - p.x;
		float dy = y - p.y;
		return Mathf.Atan2(dy, dx);
	}
	
	float GetDistance(pythPoint p)
	{
		float dx = x - p.x;
		float dy = y - p.y;
		return Mathf.Sqrt(dx * dx + dy * dy);
	}
}

public class pythblock
{
	public float x, y, size, pSize;
	public pythPoint p0, p1, p2, p3, C;
	float cx, cy;
	public Color blockCol;
	
	public pythblock(float _x, float _y, float _size, float _pSize, Color _col)
	{
		x = _x;
		y = _y;
		size = _size;
		pSize = _pSize;
		C =  new pythPoint(_size/2 + _x, _size/2 + _y, _pSize, _col);
		p0 = new pythPoint(_x, _y, _pSize, _col);
		p1 = new pythPoint(_x + _size, _y, _pSize, _col);
		p2 = new pythPoint(_x + _size, _y + _size, _pSize, _col);
		p3 = new pythPoint(_x , _y + _size, _pSize, _col);
		blockCol = _col;
	}
	
	public void drawBlock()
	{
		C.drawPoint();
		p0.drawPoint();
		p0.drawLine(p1);
		p1.drawPoint();
		p1.drawLine(p2);
		p2.drawPoint();
		p2.drawLine(p3);
		p3.drawPoint();
		p3.drawLine(p0);
	}
	
	public void rotateBlock(float angle, pythPoint p)
	{
		p0.rotatePoint(angle, p);
		p1.rotatePoint(angle, p);
		p2.rotatePoint(angle, p);
		p3.rotatePoint(angle, p);
		C.rotatePoint(angle, p);
			
	}
}

public class pythTree
{
	public float x, y, size, angle, limit;
	public float branchAngleA;
	
	public pythTree(float _x, float _y, float _size, float _angle, float _limit, float _branchAngleA)
	{
		x = _x;
		y = _y;
		size = _size;
		angle = _angle;
		limit = _limit;
		branchAngleA = _branchAngleA;
	}
	
	public void createPythTree(float _x, float _y, float _size, float _angle, float _limit)
	{
		
		float rad = 1 + (_limit);
		pythblock block = new pythblock(_x, _y, _size, rad, Color.white);
		block.rotateBlock(_angle, block.p0);
		block.drawBlock();
		
		//Left Branch
		float size0 = Mathf.Abs(Mathf.Cos(branchAngleA) * _size);
		float x0 = block.p3.x;
		float y0 = block.p3.y;
		float angle0 = _angle + branchAngleA;
		
		if(_limit > 0)
		{
			createPythTree(x0, y0, size0, angle0, _limit - 1);
		}
		else
		{
			pythblock block0 = new pythblock(x0, y0, size0, rad, Color.blue);
			block0.rotateBlock(angle0, block0.p0);
			block0.drawBlock();
		}
		
		//Right Branch
		float size1 = Mathf.Abs(Mathf.Sin(branchAngleA) * _size);
		float x1 =  x0 + Mathf.Cos(angle0) * size0;
		float y1 =  y0 + Mathf.Sin(angle0) * size0;
		float angle1 = angle0 - Mathf.PI / 2;
		
		if(_limit > 0)
		{
			createPythTree(x1, y1, size1, angle1, _limit - 1);
		}
		else
		{
			pythblock block1 = new pythblock(x1, y1, size1, rad, Color.green);
			block1.rotateBlock(angle1, block1.p0);
			block1.drawBlock();
		}
	}
}

public class PythFracTree : MonoBehaviour 
{
	pythblock block0;
	pythTree pTree;
	public bool random;
	public float angle;
	public int limit;
	// Use this for initialization
	void Start () 
	{
		if(random)
		{
			angle = g_utilities.randomRange(0, -Mathf.PI / 2); 
		}
		else
		{
			angle =  Mathf.PI / 4;
		}
		
		block0 = new pythblock(0, -MouseScreenInfo.screenDim.y, 100, 10, Color.white);
		pTree = new pythTree(0, -MouseScreenInfo.screenDim.y, 100, 0, limit,angle);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnDrawGizmos()
	{
		if(Application.isPlaying)
		{
			//block0.drawBlock();
			pTree.createPythTree(pTree.x, pTree.y, pTree.size, pTree.angle, pTree.limit);
		}
	}
}
