using UnityEngine;
using System.Collections;
using System;

public class Fk_Arm 
{
    public float x = 0,
    y = 0,
    length = 100,
    angle = 0,
    centreAngle = 0,
    rotationRange = Mathf.PI / 4,
    phaseOffet = 0;
  
    public Fk_Arm parent;
    
    public static Fk_Arm create (float _length, float _CentreAngle, float _rotationRange, float _phaseOffset)
    {
        Fk_Arm arm = new Fk_Arm();
        arm.init(_length, _CentreAngle, _rotationRange, _phaseOffset);
        return arm;
    }
    
    void init(float _length, float _CentreAngle, float _rotationRange, float _phaseOffset)
    {
        this.length = _length;
        this.centreAngle = _CentreAngle;
        this.rotationRange = _rotationRange;
        this.phaseOffet = _phaseOffset;
    }
    
    public void setPhase(float _phase)
    {
        this.angle = this.centreAngle + Mathf.Sin(_phase + this.phaseOffet) * this.rotationRange;
    }
    
    public float getEndX()
    {
        float angle = this.angle;
        Fk_Arm parent = this.parent;
        
        while(parent != null)
        {
            angle += parent.angle;
            parent = parent.parent;
        }
        return this.x + Mathf.Cos(angle) * this.length;
    }
    
    public float getEndY()
    {
        float angle = this.angle;
        Fk_Arm parent = this.parent;
        
        while(parent != null)
        {
            angle += parent.angle;
            parent = parent.parent;
        }
        
        return this.y + Mathf.Sin(angle) * this.length;
    }
    
    public void drawArm()
    {
        Vector3 baseP = new Vector3(this.x, this.y, MouseScreenInfo.screenDim.z);
        Vector3 endP = new Vector3(this.getEndX(), this.getEndY(), MouseScreenInfo.screenDim.z);
        
        Gizmos.DrawLine(baseP, endP);
    }
}
