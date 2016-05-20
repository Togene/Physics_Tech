using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class IKSystem 
{
   public List<Ik_Arm> arms;
   public Ik_Arm lastArm;
   public float x;
   public float y;
   
   public static IKSystem create(float _x, float _y)
   {
       IKSystem obj = new IKSystem();
       obj.init(_x, _y);
       return obj;
   }
   
   void init(float _x, float _y)
   {
       this.x = _x;
       this.y = _y;
       this.arms = new List<Ik_Arm>();
   }
   
   public void addArm(float _length)
   {
        Ik_Arm arm = Ik_Arm.create(0,0,_length, 0);
        
        if(this.lastArm != null)
        {
            arm.x = this.lastArm.getEndX();
            arm.y = this.lastArm.getEndY();
            arm.parent = this.lastArm;
        }   
        else
        {
            arm.x = this.x;
            arm.y = this.y;
        }
       this.arms.Add(arm);
       this.lastArm = arm;
   }
 
      public void drawSystem()
   {
       for(int i = 0; i < this.arms.Count; i++)
       {
           arms[i].drawArm();
       }
   }
   
   public void dragSystem(float _x, float _y)
   {
       this.lastArm.drag(_x, _y);
   }
   
   public void reachSystem(float _x, float _y)
   {
       this.dragSystem(_x, _y);
       this.update();
   }
   
    public void update()
   {
       for(int i = 0; i < this.arms.Count; i++)
       {
           Ik_Arm arm = this.arms[i];
           
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
   }
}
