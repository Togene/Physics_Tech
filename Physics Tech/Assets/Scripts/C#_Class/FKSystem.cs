using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FKSystem 
{
   public List<Fk_Arm> arms;
   public Fk_Arm lastArm;
   public float x;
   public float y;
   public float phase = 0;
   public float speed = 0.05f;
   public static FKSystem create(float _x, float _y)
   {
       FKSystem obj = new FKSystem();
       obj.init(_x, _y);
       return obj;
   }
   
   void init(float _x, float _y)
   {
       this.x = _x;
       this.y = _y;
       this.arms = new List<Fk_Arm>();
   }
   
   public void addArm(float _length, float _centreAngle, float _rotationRange, float _phaseOffset)
   {
       Fk_Arm arm = Fk_Arm.create(_length, _centreAngle, _rotationRange, _phaseOffset);
       this.arms.Add(arm);
       
       if(this.lastArm != null)
       {
           arm.parent = this.lastArm;
       }
       
       this.lastArm = arm;
       this.update();
   }
   
   public void update()
   {
       for(int i = 0; i < this.arms.Count; i++)
       {
           Fk_Arm arm = this.arms[i];
           arm.setPhase(this.phase);
           
           if(arm.parent != null)
           {
               arm.x = arm.parent.getEndX();
               arm.y = arm.parent.getEndY();
           }
           else
           {
               arm.x = this.x;
               arm.y = this.y;
           }
       }
       
       this.phase += this.speed;
   }
   
   public void drawSystem()
   {
       for(int i = 0; i < this.arms.Count; i++)
       {
           arms[i].drawArm();
       }
   }
   
   public void rotateArm(int _index, float _angle)
    {
        this.arms[_index].angle = _angle;
    }
   
}
