using UnityEngine;
using System.Collections;
using System;
using GUtils;
using System.Collections.Generic;

public class ifsPoint
{
    public float x;
    public float y;
    public Color pCol;
    
    public ifsPoint(float _x, float _y, Color _col)
    {
        x = _x;
        y = _y;
        pCol = _col;
    }
    
    public void drawPoint()
    {
        Gizmos.color = pCol;
        Gizmos.DrawCube(new Vector3((float)x,(float) y, MouseScreenInfo.screenDim.z), new Vector3(1.5f, 1.5f, 1.5f));
    }
}
public class rule
{
    public float a,b,c,d,tx,ty;
    public float weight;
    public Color col;
    
    public rule()
    {
    }
    
    public rule(float _a, float _b, float _c, float _d, float _tx, float _ty, float _weight, Color _col)
    {
        a = _a;
        b = _b;
        c = _c;
        d = _d;
        tx = _tx;
        ty = _ty;
        weight = _weight;
        col = _col;
    }
}

public class IFSFractal : MonoBehaviour 
{
    public rule[] LeafRules, TreeRules;
    public List<ifsPoint> ifsPointList = new List<ifsPoint>();
    public float X;
    public float Y;
    public int listSize;
    public bool tree, leaf;
    public float Sx, Sy;
    void Awake ()
    {
        LeafRules = new rule[]
        {
            new rule(0.85f, 0.040f, 0.0f, 0.85f, 0f, 1.60f, 0.85f, Color.red),
            new rule(-0.15f, 0.28f, 0.26f, 0.24f, 0f, 0.44f, 0.07f, Color.green),
            new rule(0.20f, -0.26f, 0.23f, 0.22f, 0f, 1.6f, 0.07f, Color.blue),
            new rule(0f, 0f, 0f, 0.16f, 0f, 0f, 0.01f, Color.yellow)
        };
        
        TreeRules = new rule[]
        {
            new rule(0.05f, 0.0f, 0.0f, 0.6f, 0f, 0f, 0.17f, Color.red),
            new rule(0.05f, 0f, 0f, -0.5f, 0f, 1f, 0.17f, Color.green),
            new rule(0.46f, -0.321f, 0.386f, 0.383f, 0f, 0.6f, 0.17f, Color.blue),
            new rule(0.47f, -0.154f, 0.171f, 0.423f, 0f, 1.1f, 0.17f, Color.yellow),
            new rule(0.433f, 0.275f, -0.25f, 0.476f, 0f, 1f, 0.16f, Color.yellow),
            new rule(0.421f, 0.257f, -0.353f, 0.306f, 0f, 0.7f, 0.16f, Color.yellow)
        };
        
    }
    
	// Use this for initialization
	void Start () 
    {       
	}
	
    void iterate()
    {  
 
        if(listSize < 10000)
        {
         X = UnityEngine.Random.value;
         Y = UnityEngine.Random.value; 
         
         rule r = new rule();
         
            for(int i = 0; i < 100; i++)
            {
                if(leaf)
                {
                     r = getRule(LeafRules);
                }
                else if(tree)
                {
                     r = getRule(TreeRules);
                }
                 
                
                float x1 = 0;
                float y1 = 0;
                
                x1 = (X * r.a) + (Y * r.b) + r.tx;
                y1 = (X * r.c) + (Y * r.d) + r.ty;
                  
                X = x1;
                Y = y1; 
                
                plot(X, Y, g_utilities.RandomColor());  
            } 
        }
        else
        {
            return;
        }
    }
    
    void plot(float x, float y, Color _col)
    {
        if(tree)
        {
            Sx = 300;
            Sy = 300;
        }
        else if(leaf)
        {
            Sx = 35;
            Sy = 35;  
        }
        ifsPoint ifs =  new ifsPoint((x * Sx), -MouseScreenInfo.screenDim.y + (y * Sy), _col); 
        ifsPointList.Add(ifs);
    }
    
    rule getRule(rule[] rules)
    {
        float rand = UnityEngine.Random.value;
                
        for(int i = 0; i < rules.Length; i++)
        {
             rule r = rules[i];
             
             if(rand < r.weight)
            {
                return r;
            } 
              rand -= r.weight;
        }
          return rules[rules.Length - 1];      
    }
    
	// Update is called once per frame
	void FixedUpdate () 
    {
        listSize = ifsPointList.Count -1;
        InvokeRepeating("iterate", .1f, .2f);  
	}
    
    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            for(int i = 0; i < ifsPointList.Count - 1; i++)
            {
                ifsPointList[i].drawPoint();
            }
        }
    }
}
