using UnityEngine;
using System.Collections;
using System;


public class catchBall
{
    public float x = 100;
    public float y = 100;
    float vx = 5;
    float vy = 5;
    float rad = 10;
    float gravity = 0.25f;
    float bounce = -1f;
    
    public catchBall()
    {

    }
    
    public void update()
    {
        this.x += this.vx;
        this.y += this.vy;
        this.vy -= this.gravity;
        
        if(this.x + this.rad > MouseScreenInfo.screenDim.x)
        {
         this.x = MouseScreenInfo.screenDim.x - this.rad;
         this.vx *= this.bounce;
            
        }
        else if(this.x < this.rad+ -MouseScreenInfo.screenDim.x)
        {
            this.x = this.rad + -MouseScreenInfo.screenDim.x;
            this.vx *= this.bounce;
        }
        
         if(this.y + this.rad > MouseScreenInfo.screenDim.y)
        {
         this.y = MouseScreenInfo.screenDim.y - this.rad;
         this.vy *= this.bounce;
            
        }
        else if(this.y < this.rad + -MouseScreenInfo.screenDim.y)
        {
            this.y = this.rad + -MouseScreenInfo.screenDim.y;
            this.vy *= this.bounce;
        }
    }
    
    public void drawBall()
    {
        Gizmos.DrawWireSphere(new Vector3(x,y, MouseScreenInfo.screenDim.z), rad);
    }
}

public class Kinematic : MonoBehaviour 
{
    FKSystem leg0, leg1;
    IKSystem iks0, iks1, iks2;
    
    public catchBall cBall;
    
    public Ik_Arm ik_arm0, ik_arm1, ik_arm2;
    public bool isIKSystem, isFKSystem, single, drag, reach, playBall;
    float angle;
    public int numArms;
    void Awake ()
    {
       
    }
	// Use this for initialization
	void Start () 
    {
	 if(isIKSystem)
        {
            if(single)
            {
                ik_arm0 = Ik_Arm.create(0, 0, 100, 0);
                ik_arm1 = Ik_Arm.create(ik_arm0.getEndX(), ik_arm0.getEndY(), 100, 0);
                ik_arm2 = Ik_Arm.create(ik_arm1.getEndX(), ik_arm1.getEndY(), 100, 0);
                
                ik_arm1.parent = ik_arm0;
                ik_arm2.parent = ik_arm1;
            }
            else
            {
                cBall = new catchBall();
                
                iks0 = IKSystem.create(MouseScreenInfo.screenDim.x - 250 ,-MouseScreenInfo.screenDim.y);
                iks1 = IKSystem.create(-MouseScreenInfo.screenDim.x + 250,-MouseScreenInfo.screenDim.y);
                
                iks0.addArm(120);
                iks0.addArm(90);
                iks0.addArm(60);
                
                iks1.addArm(120);
                iks1.addArm(90);
                iks1.addArm(60);
                
                for(int i = 0; i < numArms; i++)
                {
                    //iks.addArm(50); 
                }
            }

        }
        else if(isFKSystem)
        {
            leg0 = FKSystem.create(0, 0);
            leg1 = FKSystem.create(0, 0);
            leg1.phase = Mathf.PI;
            
            leg0.addArm(100, -Mathf.PI / 2, Mathf.PI / 4, 0);
            leg0.addArm(90, -0.87f, 0.87f, -1.5f);
            
            leg1.addArm(100, -Mathf.PI / 2, Mathf.PI / 4, 0);
            leg1.addArm(90, -0.87f, 0.87f, -1.5f);
        }
        else
        {
            
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
     
        cBall.update();
        
        if(isIKSystem)
        {
           if(single)
            {
               ik_arm2.drag(MouseScreenInfo.mouseWorldPos.x, MouseScreenInfo.mouseWorldPos.y); 
            }
            else
            {
                if(drag)
                {
                iks1.dragSystem(MouseScreenInfo.mouseWorldPos.x, MouseScreenInfo.mouseWorldPos.y);
                }
                else if(reach)
                {
                    if(!playBall)
                    {
                        iks0.reachSystem(MouseScreenInfo.mouseWorldPos.x, MouseScreenInfo.mouseWorldPos.y);
                        iks1.reachSystem(MouseScreenInfo.mouseWorldPos.x, MouseScreenInfo.mouseWorldPos.y);
                    }
                    else
                    {
                        iks0.reachSystem(cBall.x, cBall.y);
                        iks1.reachSystem(cBall.x, cBall.y);
                    }
                }
            }
        }
        
        else if(isFKSystem)
        {
            if(single)
            {
                
            }else
            {
            leg0.update();
            leg1.update();
            }
        }
        else
        {
            
        }
	}
    
    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            cBall.drawBall();
            if(isIKSystem)
            {
                if(single)
                {
                    ik_arm0.drawArm();
                    ik_arm1.drawArm();
                    ik_arm2.drawArm();
                }
                else
                {
                    iks0.drawSystem();
                    iks1.drawSystem();
                }
            }
            else if(isFKSystem)
            {
                if(single)
                {
                    
                }
                else
                {
                    leg0.drawSystem();
                    leg1.drawSystem(); 
                }
  
            }
            else
            {
                
            }
        }
    }
}
