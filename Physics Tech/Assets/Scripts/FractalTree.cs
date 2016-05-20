using UnityEngine;
using System.Collections;
using GUtils;

public class fracPoint
{
	public float x;
	public float y;
	public float rad;
	
	public fracPoint(float _x, float _y, float _rad)
	{
		x = _x;
		y = _y;
		rad = _rad;
		
	}
	
	public void drawPoint()
	{
		Gizmos.DrawWireSphere(new Vector3(x,y,MouseScreenInfo.screenDim.z), rad);
	}
	
	public void drawLine(fracPoint p0)
	{
		Gizmos.DrawLine(new Vector3(x,y,MouseScreenInfo.screenDim.z),
		new Vector3(p0.x, p0.y, MouseScreenInfo.screenDim.z));
	}
}

public class fracTree
{
	public fracPoint p0, p1;
	public float branchAngleA, branchAngleB, trunkRatio;
	
	public fracTree(fracPoint _p0, fracPoint _p1, float _branchAngleA, float _branchAngleB, float _trunkRatio)
	{
		p0 = _p0;
		p1 = _p1;
		
		branchAngleA = _branchAngleA;
		branchAngleB = _branchAngleB;
		trunkRatio = _trunkRatio;
	}
	
	public void createTree(fracPoint p0, fracPoint p1, float limit)
	{
		float dx = p1.x - p0.x;
		float dy = p1.y - p0.y;
		float dist = Mathf.Sqrt(dx * dx + dy * dy);
		float angle = Mathf.Atan2(dy, dx);
		float branchLength = dist * (1 - trunkRatio);
		float radSize = p0.rad / limit;
		
		fracPoint pA = new fracPoint(p0.x + dx * trunkRatio, 
								     p0.y + dy * trunkRatio, radSize);
									 
		fracPoint pB = new fracPoint(pA.x + Mathf.Cos(angle + branchAngleA) * branchLength,
								 		pA.y + Mathf.Sin(angle + branchAngleA) * branchLength,
										radSize);
										 
		fracPoint pC = new fracPoint(pA.x + Mathf.Cos(angle + branchAngleB) * branchLength,
								 	 pA.y + Mathf.Sin(angle + branchAngleB) * branchLength,
								 radSize);
	
		if(limit > 0)
		{
			createTree(pA, pC, limit - 1);
			createTree(pA, pB, limit - 1);
		    p0.drawLine(pA);
		    pA.drawPoint();
		}
		else
		{
		  p0.drawLine(pA);
		  pA.drawPoint();
		  pA.drawLine(pB);
		  pB.drawPoint();
		  pC.drawLine(pA);
		  pC.drawPoint();
		}
		
		branchAngleA += g_utilities.randomRange(-0.02f, 0.02f);
		branchAngleB += g_utilities.randomRange(-0.02f, 0.02f);
		trunkRatio += g_utilities.randomRange(-0.02f, 0.02f);
	}
}

public class FractalTree : MonoBehaviour 
{
	public fracTree tree;
	public int iteration, rad;
	// Use this for initialization
	void Start () 
	{
		tree = new fracTree(new fracPoint(0, -MouseScreenInfo.screenDim.y, rad), 
							new fracPoint(0, -MouseScreenInfo.screenDim.y + 500, rad), 
							g_utilities.randomRange(-Mathf.PI / 2, Mathf.PI / 2), 
							g_utilities.randomRange(-Mathf.PI / 2, Mathf.PI / 2), 
							g_utilities.randomRange(0.25f, 0.75f));		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	if(Input.GetKeyDown(KeyCode.Space))
	{
		tree = null;
				tree = new fracTree(new fracPoint(0, -MouseScreenInfo.screenDim.y, rad), 
							new fracPoint(0, -MouseScreenInfo.screenDim.y + 500, rad), 
							g_utilities.randomRange(-Mathf.PI / 2, Mathf.PI / 2), 
							g_utilities.randomRange(-Mathf.PI / 2, Mathf.PI / 2), 
							g_utilities.randomRange(0.25f, 0.75f));		
	}
	}
	
	void OnDrawGizmos()
	{
		if(Application.isPlaying)
		{
			tree.createTree(tree.p0, tree.p1, iteration);
		}
	}
}
