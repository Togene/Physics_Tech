﻿using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour 
{
   public enum DrawMode{NoiseMap, ColorMap, Mesh};
   public DrawMode drawMode;
   
   public Noise.NormalizeMode normalizedMode;
   
   public const int mapChunkSize = 241;
   [Range(0, 6)]
   public int EditorPreviewLevelOfDetial;
   public float noiseScale;
   
   public int octaves;
   [Range(0, 1)]
   public float persistance;
   public float lacunarity;
   
   public int seed;
   public Vector2 offset;
   
   public float meshHeightMultiplier;
   public AnimationCurve meshHeightCurve;
   public bool autoUpdate;
   
   public TerrainType[] regions;
   
   Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
   Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();
   
   public void DrawMapInEditor()
   {
       MapDisplay display = FindObjectOfType<MapDisplay>();
       MapData mapData = GenerateMapData(Vector2.zero);
       
       if(drawMode == DrawMode.NoiseMap)
       {
         display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
       }
       else if( drawMode == DrawMode.ColorMap)
       {
       display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
       }
       else if( drawMode == DrawMode.Mesh)
       {
       display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, EditorPreviewLevelOfDetial), TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
       }
   }
   
   public void RequestMapData(Vector2 centre, Action<MapData> callBack)
   {
        ThreadStart threadStart = delegate{
            MapDataThread(centre, callBack);
        };
        
        new Thread(threadStart).Start();
   }
   
   void MapDataThread(Vector2 centre, Action<MapData> callback)
   {
       MapData mapData = GenerateMapData(centre);
       
       lock (mapDataThreadInfoQueue)
       {
       mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
       }
       
   }
   
   public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
   {
       ThreadStart threadStart = delegate{
           MeshDataThread(mapData, lod, callback);
       };
       
       new Thread(threadStart).Start();
   }
   
   
   void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
   {
       MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
   
        lock(meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData> (callback, meshData));
        }
   }
   
   void Update()
   {
       if(mapDataThreadInfoQueue.Count > 0)
       {
           for(int i = 0; i < mapDataThreadInfoQueue.Count; i++)
           {
               MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
               threadInfo.callback(threadInfo.parameter);
           }
       }
       
       if(meshDataThreadInfoQueue.Count > 0)
       {
           for(int i = 0; i < meshDataThreadInfoQueue.Count; i++)
           {
               MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
               threadInfo.callback(threadInfo.parameter);
           }
       }
   }
   
    MapData GenerateMapData(Vector2 centre)
   {
       float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizedMode);
       
       Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
       
       for(int y = 0; y < mapChunkSize; y++)
       {
           for(int x = 0; x < mapChunkSize; x++)
           {
               float currentHeight = noiseMap[x,y];
               
               for(int i = 0; i < regions.Length; i++)
               {
                   if(currentHeight >= regions[i].height)
                   {
                       colorMap[y * mapChunkSize + x] = regions[i].color;
                   }
                   else
                   {
                       break;
                   }
               }
           }
       }
      
    return new MapData(noiseMap, colorMap);
   }
   
   void OnValidate()
   {    
       lacunarity = (lacunarity < 1) ? 1 : lacunarity;
       octaves = (octaves < 0) ? 0 : octaves;
   }
}


struct MapThreadInfo<T>
{
	public readonly Action<T> callback;
	public readonly T parameter;

	public MapThreadInfo(Action<T> callback, T parameter)
	{
		this.callback = callback;
		this.parameter = parameter;
	}   
}

[System.SerializableAttribute]
public struct TerrainType
{
	public string name;
	public float height;
	public Color color;
}

public struct MapData
{
	public readonly float[,] heightMap;
	public readonly Color[] colorMap;

	public MapData(float[,] heightMap, Color[] colorMap)
	{
		this.heightMap = heightMap;
		this.colorMap = colorMap;
	}
}
