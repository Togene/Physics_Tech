using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EndlessTerrain : MonoBehaviour 
{
    const float viewerMoveThresgholdForChunkUpdate = 25f;
    const float sqrviewerMoveThresgholdForChunkUpdate = viewerMoveThresgholdForChunkUpdate * viewerMoveThresgholdForChunkUpdate;
    
    public LODinfo[] detailLevels;
    public static float maxViewDst;
     
    public Transform viewer;
    public Material mapMaterial;
    
    public static Vector2 viewerPosition;
    Vector2 viewerPositionOld;
    static MapGenerator mapGenerator;
  
    int chunksize;
    int chunkVisibleViewDst;
    
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
    
    void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;
        chunksize = MapGenerator.mapChunkSize - 1;
        chunkVisibleViewDst = Mathf.RoundToInt(maxViewDst/chunksize);
        UpdateVisibleChunks();
    }
    
    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        
        if((viewerPositionOld -viewerPosition).sqrMagnitude > sqrviewerMoveThresgholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
        
    }
    
    void UpdateVisibleChunks()
    {
        
        for(int i = 0; i <terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();
        
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x/chunksize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y/chunksize);
        
        for(int yOffset = -chunkVisibleViewDst; yOffset <= chunkVisibleViewDst; yOffset++)
        {
             for(int xOffset = -chunkVisibleViewDst; xOffset <= chunkVisibleViewDst; xOffset++)
             {
                 Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, 
                                                        currentChunkCoordY + yOffset);
                                                        
              if(terrainChunkDictionary.ContainsKey(viewedChunkCoord))
              {
                 terrainChunkDictionary[viewedChunkCoord].UpdateTerrianChunk();        
              }
              else
              {
                  terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunksize, detailLevels, transform, mapMaterial));                
              }
              
             }
        }
    }
    
    public class TerrainChunk
    {
        Vector2 position;
        GameObject meshObject;
        Bounds bounds;
        
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        LODinfo[] detialLevels;
        LODMesh[] lodMeshes;
        
        MapData mapData;
        bool mapDataReceived;
        int previousLODIndex = -1;
        
        public TerrainChunk(Vector2 coord, int size, LODinfo[] detialLevels, Transform parent, Material material)
        {
            this.detialLevels = detialLevels;
            
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);
            
            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
            meshFilter = meshObject.AddComponent<MeshFilter>();
           
            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;
            SetVisible(false);
            
            lodMeshes = new LODMesh[detialLevels.Length];
            
            for(int i = 0; i < detialLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detialLevels[i].lod, UpdateTerrianChunk);
            }
            
            mapGenerator.RequestMapData(position, OnMapDataReceived);
        }
        
        void OnMapDataReceived(MapData mapData)
        {
			this.mapData = mapData;
            mapDataReceived = true;
            
            Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;
            UpdateTerrianChunk();
        }
        
        public void UpdateTerrianChunk()
        {
            if(mapDataReceived)
            {
            
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                bool visible = viewerDstFromNearestEdge <= maxViewDst;
                
                if(visible)
                {
                    int lodIndex = 0;
                    
                    for(int i = 0; i < detialLevels.Length - 1; i++)
                    {
                        if(viewerDstFromNearestEdge > detialLevels[i].visibleDistanceThreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    if(lodIndex != previousLODIndex)
                    {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        
                        if(lodMesh.hasMesh)
                        {
                            previousLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                        } 
                        else if(!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }
                    
                    terrainChunksVisibleLastUpdate.Add(this);
                }
                
                SetVisible(visible);
            }
        }
        
        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }
        
        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }
    
    class LODMesh{
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;
        
        
        public LODMesh(int lod, System.Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }
        
        void OnMeshDataReceived(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;
            
            updateCallback();
        }
        
        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataReceived);
        }
    }
    
    [System.SerializableAttribute]
    public struct LODinfo
    {
        public int lod;
        public float visibleDistanceThreshold;
    }
}
