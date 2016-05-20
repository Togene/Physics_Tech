using UnityEngine;
using System.Collections;
using System;

[SerializableAttribute]
public class loot
{
    public string name;
    public float chance;
    
    public loot(string _name, float _chance)
    {
        name = _name;
        chance = _chance;
    }
}

public class WeightedRandom : MonoBehaviour 
{
    public Vector2[] pList;
    public float chanceVal;
    public bool diceRoll, ifPlayGame;
    public string[] treasures = new string[] {"sword", "axe", "wand", "staff", "daggers", "shield", "gem"};
    
    public loot[] prizes = new loot[]
    {
      new loot("nothing", 8),
      new loot("a gold piece", 5),
      new loot("a treasure chest" , 2),
      new loot("poison", 1),
      new loot("food", 3)
    };
   
    void Awake ()
    {
    
    }
	// Use this for initialization
	void Start () 
    {
        if(diceRoll)
        {
        pList = new Vector2[100];
            
            for(int i = 0; i < 100; i++)
            {
                Vector2 p = pList[i];
                
                    bool heads = UnityEngine.Random.value < chanceVal;
                    
                    pList[i].y =  UnityEngine.Random.value * -MouseScreenInfo.screenDim.y * 2 + MouseScreenInfo.screenDim.y;
                    pList[i].x = UnityEngine.Random.value * MouseScreenInfo.screenDim.x;
                    
                    if(heads)
                    {
                        pList[i].x -= MouseScreenInfo.screenDim.x;
                    }
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(diceRoll)
        {
	        if(Input.GetKeyDown(KeyCode.Space))
            {
                reRoll();
            }
        }
        else if(ifPlayGame)
        {
            if(Input.GetMouseButtonDown(0))
            {
                float rand = UnityEngine.Random.value;
                string prize = getPrize();
                Debug.Log("You winned " + prize);
            }
        }
	}
    
    string getPrize()
    {
        float total = 0;
        
        for(int i = 0; i < prizes.Length; i++)
        {
            total += prizes[i].chance;
        }
        
        float rand = UnityEngine.Random.value * total;
        
        
       for(int i = 0; i < prizes.Length -1; i++)
       {
           if(rand < prizes[i].chance)
           {
               return prizes[i].name;
           }
           
           rand -= prizes[i].chance;
       }
       
       return prizes[prizes.Length - 1].name;
    }
    
    void reRoll()
    {
         for(int i = 0; i < 100; i++)
        {
            Vector2 p = pList[i];
            
                bool heads = UnityEngine.Random.value < chanceVal;
                
                pList[i].y = UnityEngine.Random.value * -MouseScreenInfo.screenDim.y * 2 + MouseScreenInfo.screenDim.y;
                pList[i].x = UnityEngine.Random.value * MouseScreenInfo.screenDim.x;
                
                if(heads)
                {
                    pList[i].x -= MouseScreenInfo.screenDim.x;
                }
        }
    }
    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            if(diceRoll)
                {
                    Gizmos.DrawLine(new Vector3(0,-MouseScreenInfo.screenDim.y, MouseScreenInfo.screenDim.z),
                                    new Vector3(0, MouseScreenInfo.screenDim.y, MouseScreenInfo.screenDim.z));
                                    
                for(int i = 0; i < 100; i++)
                    {
                    Gizmos.DrawSphere(new Vector3(pList[i].x, pList[i].y, MouseScreenInfo.screenDim.z), 10);
                    }
                }
        }
    }
}
