using UnityEngine;
using System.Collections;
using System;

[SerializableAttribute]
public class Ik_Arm 
{
    public float x = 0,
                 y = 0,
                 length = 0,
                 angle = 0;
                 
    public Ik_Arm parent;
    
    public static Ik_Arm create(float _x, float _y, float _length, float _angle)
    {
        Ik_Arm arm = new Ik_Arm();
        arm.init(_x, _y, _length, _angle);
        return arm;
    }
    
    void init(float _x, float _y, float _length, float _angle)
    {
        this.x = _x;
        this.y = _y;
        this.length = _length;
        this.angle = _angle;
    }
    
    public float getEndX()
    {
        return this.x + Mathf.Cos(this.angle) * this.length;
    }
    
    public float getEndY()
    {
        return this.y + Mathf.Sin(this.angle) * this.length;
    }
    
    
    public void pointAt(float _x, float _y)
    {
        float dx = _x - this.x;
        float dy = _y - this.y;
        
        this.angle = Mathf.Atan2(dy, dx);
    }
    
    public void drag(float _x, float _y)
    {
        this.pointAt(_x, _y);
        this.x = _x - Mathf.Cos(this.angle) * this.length;
        this.y = _y - Mathf.Sin(this.angle) * this.length;
        
        if(parent != null)
        {
            this.parent.drag(this.x, this.y);
        }
        
    }
    public void drawArm()
    {
        Vector3 baseP = new Vector3(this.x, this.y, MouseScreenInfo.screenDim.z);
        Vector3 endP = new Vector3(this.getEndX(), this.getEndY(), MouseScreenInfo.screenDim.z);
        
        Gizmos.DrawLine(baseP, endP);
    }
    
}
