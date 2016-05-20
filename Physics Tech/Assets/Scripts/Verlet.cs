using UnityEngine;
using System.Collections;
using System;

[SerializableAttribute]
public class vPoint
{
	
	public float x;
	public float y;
	public float oldx;
	public float oldy;	
	public bool isPinned, isEngine;
	public Color pointColor = Color.white;
	float bounce = 0.98f;
	float gravity = 0.98f;
	float friction = 0.98f;
	public vPoint(float _x, float _y, float _oldx, float _oldy)
	{
		x = _x;
		y = _y;

		oldx = _oldx;
		oldy = _oldy;
	}
	
	public void updatePoint(int _iterate)
	{

		float vx = (x - oldx) * friction;
		float vy = (y - oldy) * friction;
		
		if(x != -x && y != -y)
		{

		}
		else
		{

		}			
		
		if(!isPinned || !isEngine)
		{
			oldx = x;
			oldy = y;
			x += vx;
			y += vy;
			y -= gravity;
			
		}
		
			for(int i = 0; i < _iterate; i++)
			{
				constrainPoints(vx, vy);
			}
		
	}

	public void drawPoint()
	{
		if(isEngine)
		{
			Gizmos.color = pointColor;
			Gizmos.DrawSphere(new Vector3(x, y, MouseScreenInfo.screenDim.z), 20);
		}
		else
		{
			Gizmos.color = pointColor;
			Gizmos.DrawWireSphere(new Vector3(x, y, MouseScreenInfo.screenDim.z), 10);
		}
	}
	
	public void constrainPoints(float vx, float vy)
	{
		if(x > MouseScreenInfo.screenDim.x)
		{
			x = MouseScreenInfo.screenDim.x;
			oldx = x + vx * bounce;
		}
		else if (x < -MouseScreenInfo.screenDim.x)
		{
			x = -MouseScreenInfo.screenDim.x;
			oldx = x + vx * bounce;
		}
		else if(y > MouseScreenInfo.screenDim.y)
		{
			y = MouseScreenInfo.screenDim.y;
			oldy = y + vy * bounce;
		}
		else if (y < -MouseScreenInfo.screenDim.y)
		{
			y = -MouseScreenInfo.screenDim.y;
			oldy = y + vy * bounce;
		}
	}
}
public class vEngine : vPoint
{
	float baseX;
	float baseY;
	float range;
	float angle;
	float speed;
	public bool sphereEngine;
	
	public vEngine(float _baseX, float _baseY, float _range, float _speed,
	float _x, float _y, float _oldx, float _oldy) : base( _x, _y, _oldx, _oldy)
	{
		baseX = _baseX;
		baseY = _baseY;
		
		range = _range;
		speed = _speed;
		
		this.isEngine = true; //
		this.isPinned = true; //it moves on its own
		
		this.pointColor = Color.green;
	}
	
	public void updateEngine()
	{
		this.x = baseX + (float)(Math.Cos(angle) * range);
		
		if(sphereEngine)
		{
			this.y = baseY + (float)(Math.Sin(angle) * range);
		}
	
		angle += speed;
	}
	
	public void drawTrajectory()
	{
		if(!sphereEngine)
		{
		Gizmos.DrawWireCube(new Vector3(baseX/2, baseY, MouseScreenInfo.screenDim.z) , new Vector2(range + 80, 40));
		}
		else
		{
		Gizmos.DrawWireSphere(new Vector3(baseX/2, baseY, MouseScreenInfo.screenDim.z), range + 20);
		Gizmos.DrawWireSphere(new Vector3(baseX/2, baseY, MouseScreenInfo.screenDim.z), 20);	
		}
	}
}
[SerializableAttribute]
public class vStick
{
	public vPoint p0, p1;
	public float length;
	
	public vStick(vPoint _p0, vPoint _p1)
	{
		p0 = _p0;
		p1 = _p1;
		length = distance(_p0, _p1);
	}
	
	public float distance(vPoint _p0, vPoint _p1)
	{
		float dx = _p1.x - _p0.x;
		float dy = _p1.y - _p0.y;
		
		return Mathf.Sqrt(dx * dx + dy * dy);
	}
	
	public void drawStick()
	{
		Vector3 p0draw = new Vector3(p0.x, p0.y, MouseScreenInfo.screenDim.z);
		Vector3 p1draw = new Vector3(p1.x, p1.y, MouseScreenInfo.screenDim.z);		
		p0.drawPoint();
		p1.drawPoint();
		Gizmos.DrawLine(p0draw, p1draw); 
	}
	
	public void updateStick()
	{						
			float dx = p1.x - p0.x;
			float dy = p1.y - p0.y;
			
			float distance = Mathf.Sqrt(dx * dx + dy * dy);
			
			float diff = length - distance;
			float percent = diff / distance / 2;
			
			float offsetX = dx * percent;
			float offsetY = dy * percent;
			
			if(!p0.isPinned || !p0.isEngine)
			{
				p0.x -= offsetX;
				p0.y -= offsetY;
			}
			
			if(!p1.isPinned || !p1.isEngine)
			{		
				p1.x += offsetX;
				p1.y += offsetY;
			}		
	}
}
public class vSpring : vStick
{
	float k;
	
	public vSpring(float _k, vPoint _p0, vPoint _p1) : base (_p0, _p1)
	{
		k = _k;
	}
	
	public void updateSpring()
	{
		
	}
}
[SerializableAttribute]
public class vBox
{
	public vStick s0, s1, s2, s3, cS;
	public vPoint p0, p1, p2, p3;
	public float width, height;
	
	public vBox(float _x, float _y, float _width, float _height)
	{
		p0 = new vPoint(_x, _y, _x, _y);
		p1 = new vPoint(_x + _width, _y, _x + _width, _y);
		p2 = new vPoint(_x + _width, _y + _height, _x + _width, _y + _height);
		p3 = new vPoint(_x, _y + _height, _x, _y + height);
		
		width = _width;
		height = _height;
		
		s0 = new vStick(p0, p1);
		s1 = new vStick(p1, p2);
		s2 = new vStick(p2, p3);
		s3 = new vStick(p3, p0);
		
		cS = new vStick(p0, p2);
	}
	
	public void updateBox(int _iterate)
	{
		for(int i = 0; i < _iterate; i++)
		{		
			s0.updateStick();
			s1.updateStick();
			s2.updateStick();
			s3.updateStick();			
			cS.updateStick();
		}
		p0.updatePoint(_iterate);
		p1.updatePoint(_iterate);
		p2.updatePoint(_iterate);
		p3.updatePoint(_iterate);	
	}
	
	public void drawBox()
	{
		p0.drawPoint();
		p1.drawPoint();
		p2.drawPoint();
		p3.drawPoint();
		
		s0.drawStick();
		s1.drawStick();
		s2.drawStick();
		s3.drawStick();
		
		cS.drawStick();
	}
}
[SerializableAttribute]
public class vChain
{
	public vStick[] chainSticks;
	public vPoint[] chainPoints;
	
	public vChain(int _numChains, float _linkLength, vPoint connector = null)
	{
		chainSticks = new vStick[_numChains];
		chainPoints = new vPoint[_numChains + 1];
	

		for (int i = 0; i < chainPoints.Length; i++)
		{
			float length = i * -_linkLength;		
			chainPoints[i] = new vPoint(0 , MouseScreenInfo.screenDim.y + length , 0 , MouseScreenInfo.screenDim.y + length );
		}

		for (int i = 0; i < chainSticks.Length; i++)
		{
			chainSticks[i] = new vStick(chainPoints[i], chainPoints[(i + 1)]);
		}			
		
		if(connector != null)
		{
			Debug.Log("Is Connecting");
			chainPoints[chainPoints.Length - 1] = connector;
			chainSticks[chainSticks.Length - 1].p1 = connector;
		}
	}
	public void pinPoint(int pointNum, bool pin)
	{
		chainPoints[pointNum].isPinned = pin;
	}
	public void updateChain(int _iterate)
	{	
		for (int i = 0; i < chainPoints.Length; i++)
		{
			chainPoints[i].updatePoint(_iterate);
		}
		for (int j = 0; j < _iterate; j++)
		{		
			for (int i = 0; i < chainSticks.Length; i++)
			{
					chainSticks[i].updateStick();
			}
		}
	}
	public void drawChain()
	{
		for (int i = 0; i < chainPoints.Length; i++)
		{
			chainPoints[i].drawPoint();
		}
		
		for (int i = 0; i < chainSticks.Length; i++)
		{
			chainSticks[i].drawStick();
		}
	}
}

[SerializableAttribute]
public class Verlet : MonoBehaviour 
{
	vChain chain0;
	vBox box0;
	public int iteration;
	public vEngine engine0;
	public bool isSpherical;
	// Use this for initialization
	void Start () 
	{
		engine0 = new vEngine(0, MouseScreenInfo.screenDim.y, 50, 0.05f,
		0, MouseScreenInfo.screenDim.y, 0, MouseScreenInfo.screenDim.y);
		box0 = new vBox(0, 0, 150f, 150f);
		
		chain0 = new vChain(5, 75, box0.p0);
		
		chain0.chainSticks[0].p0 = engine0;
		chain0.chainPoints[0] = engine0;
			
		box0.p0.pointColor = Color.blue;
		box0.p1.pointColor = Color.green;
		
		chain0.pinPoint(0, true);
	}
	
	
	void FixedUpdate () 
	{
		box0.updateBox(iteration);
		chain0.updateChain(iteration);
		engine0.updateEngine();
		engine0.sphereEngine = isSpherical;
	}
	
	void OnDrawGizmos()
	{
		if(Application.isPlaying)
		{
		box0.drawBox();
		chain0.drawChain();
		engine0.drawPoint();
		engine0.drawTrajectory();
		}
	}
}
