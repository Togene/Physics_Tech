using UnityEngine;
using System.Collections;
using System;
using GUtils;

[SerializableAttribute]
public class cPoint
{
    public float[][] trMatrix;
    public float x;
    public float y;
    
    public cPoint(float _x, float _y)
    {
        x = _x;
        y = _y;
        
        trMatrix = new float[][]
        {
            new float[]{_x},
            new float[]{_y},
            new float[]{1}
        };
    }
    
    public void updatePoint(float[][] Matrix)
    {
        x = (Matrix[0][0] * trMatrix[0][0]) + 
            (Matrix[0][1] * trMatrix[1][0]) + 
            (Matrix[0][2] * trMatrix[2][0]);
                           
        y = (Matrix[1][0] * trMatrix[0][0]) + 
            (Matrix[1][1] * trMatrix[1][0]) + 
            (Matrix[1][2] * trMatrix[2][0]);
    }
    
    public void drawPoint()
    {
        Gizmos.DrawWireSphere(new Vector3 (x, y, MouseScreenInfo.screenDim.z), 5);
    }
    
    public void drawLine(cPoint p)
    {
         Gizmos.DrawLine(new Vector3 (x, y, MouseScreenInfo.screenDim.z), 
                         new Vector3 (p.x, p.y, MouseScreenInfo.screenDim.z));      
    }
}

[SerializableAttribute]
public class g_Matrix
{
    public float[][] matrix;
    
    public g_Matrix(float _a, float _b, float _c, float _d, float _tx, float _ty)
    {
        matrix = new float[][] 
        {
           new float[] {_a, _c, _tx},
           new float[] {_b, _d, _ty},
        };
    }  
}

[SerializableAttribute]
public class cRect
{
    public float x;
    public float y;
    public float angle;
    public float[][] tMatrix;
    
    public Vector2 RotationDis, ScaleDis, PositionDis;
     float width, height;
     cPoint p0, p1, p2, p3, c0;
    
    public cRect(float _x, float _y, float _width, float _height)
    {
        x = _x;
        y = _y;
       //Position 
        p0 = new cPoint(_x - _width, _y - _height);
        p1 = new cPoint(_x + _width, _y - _height);
        p2 = new cPoint(_x + _width, _y + _height);
        p3 = new cPoint(_x - _width, _y + _height);
        c0 = new cPoint(_x, _y);
        
        unitMatrix();
        
        width = _width;
        height = _height;

    }
    
    public void unitMatrix()
    {
         tMatrix = new float[][]{
           new float[] {1, 0, 0},
           new float[] {0, 1, 0},
           new float[] {0, 0, 1}
       };
       updateRectPoints();
    }
    
    public void drawRect()
    {
        c0.drawPoint();
        
        p0.drawPoint();
        p0.drawLine(p1);
        p1.drawPoint();
        p1.drawLine(p2);
        p2.drawPoint();
        p2.drawLine(p3);
        p3.drawPoint();
        p3.drawLine(p0);
    }

    public void setTransform(float _a, float _b, float _c, float _d, float _tx, float _ty)
    {    
           tMatrix = new float[][]{
           new float[] {_a, _c, _tx},
           new float[] {_b, _d, _ty},
           new float[] {0, 0, 1}
       };
         updateRectPoints();
    }
    
 
        public void transform(float _a, float _b, float _c, float _d, float _tx, float _ty)
    {    
    ///<summary>
    ///Its gana Rock your tits.
    ///</summery>
    
        _a = g_utilities.ZeroCheck(_a);
        _b = g_utilities.ZeroCheck(_b);
        _c = g_utilities.ZeroCheck(_c);
        _d = g_utilities.ZeroCheck(_d);
         
        tMatrix[0][0] = g_utilities.ZeroCheck(tMatrix[0][0]);
        tMatrix[1][0] = g_utilities.ZeroCheck(tMatrix[1][0]);
        tMatrix[0][1] = g_utilities.ZeroCheck(tMatrix[0][1]);
        tMatrix[1][1] = g_utilities.ZeroCheck(tMatrix[1][1]);
        
           tMatrix = new float[][]{
           new float[] {tMatrix[0][0] * _a, tMatrix[0][1] * _c, tMatrix[0][2] + _tx},
           new float[] {tMatrix[1][0] * _b, tMatrix[1][1] * _d, tMatrix[1][2] + _ty},
           new float[] {tMatrix[2][0], tMatrix[2][1], tMatrix[2][2]},
           
       };
         updateRectPoints();
    }
    
    public void Translate(float _tx, float _ty)
    {
        tMatrix[0][2] = _tx;
        tMatrix[1][2] = _ty;
        PositionDis = new Vector2( tMatrix[0][2],  tMatrix[1][2]);
    }
    
    public void Scale(float _x, float _y)
    {
        tMatrix[0][0] *= _x;
        tMatrix[0][1] *= _y;
        tMatrix[1][0] *= _x;
        tMatrix[1][1] *= _y;
        tMatrix[0][2] *= _x;
        tMatrix[1][2] *= _y;
        
        ScaleDis = new Vector2(_x,_y);
    }

    public void Rotate(float _angle)
    {
        float sin = Mathf.Sin(_angle);
        float cos = Mathf.Cos(_angle);
        
        float a = tMatrix[0][0];
        float b = tMatrix[0][1];
        float c = tMatrix[1][0];
        float d = tMatrix[1][1];
        float tx = tMatrix[0][2];
        float ty = tMatrix[1][2];
        
        tMatrix[0][0] = a*cos - b*sin;
        tMatrix[0][1] = a*sin + b*cos;
        tMatrix[1][0] = c*cos - d*sin;
        tMatrix[1][1] = c*sin + d*cos;
        tMatrix[0][2] = tx*cos - ty * sin;
        tMatrix[1][2] = tx*sin + ty*cos;
        
    }
    
    public void updateRectPoints()
    {
        p0.updatePoint(tMatrix);
        p1.updatePoint(tMatrix);
        p2.updatePoint(tMatrix);
        p3.updatePoint(tMatrix);
        c0.updatePoint(tMatrix);
    }
    
}

public class Matrices : MonoBehaviour 
{
    public cRect rect0, rect1;
    public float angle;
    public float Sx, Sy;
     float cos, sin;
     public float a, b, c, d, tx, ty;
    void Awake ()
    {
    
    }
	// Use this for initialization
	void Start () 
    {
        rect0 = new cRect(0, 0, 100, 100);       
       
       angle = Mathf.PI/4;
       
       cos = Mathf.Cos(angle);
       sin = Mathf.Sin(angle);
       
       rect0.setTransform(cos * Sx ,sin * Sy, -sin * Sx, cos * Sy, 0,0);
       rect1 = new cRect(0, 0, 100, 100);       
    }
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rect0.transform( a, b, c , d, tx, ty);
        }
        
         if(Input.GetKeyDown(KeyCode.Escape))
        {
            rect0.unitMatrix();
        }
	}
    
    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
           rect0.drawRect();
           rect1.drawRect();
        }
    }
}
