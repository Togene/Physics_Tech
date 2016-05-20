using UnityEngine;
using System.Collections;

namespace GUtils
{
	public class g_utilities 
	{
		public static float normalize(float val, float min, float max)
		{
			return (val - min) / (max - min);
		}
		
		public static float lerp(float norm, float min, float max)
		{
			return (max - min) * norm + min;
		}
		
		public static float map(float val, float sourceMin, float sourceMax, float destMin, float destMax)
		{
			return lerp(normalize(val, sourceMin, sourceMax), destMin, destMax);
		}
		
		public static Color RandomColor()
		{
			return new Color(randomRange(0, 1), randomRange(0,1), randomRange(0,1), 1f);
		}
		
		public static float randomRange(float min, float max)
		{
			return min + Random.value * (max - min);
		}
		
		public static Sprite getSprite(Texture2D _set ,int _index, int tileWidth, int tileHeight)
		{	
			Sprite spr =  Sprite.Create(_set, new Rect(tileWidth * _index, 0, _set.width /((_set.width)/tileWidth), _set.height), new Vector2(0f, 0f), 1, 
			1, SpriteMeshType.Tight, new Vector4(4,4,4,4));
			return (Sprite)spr;
		}
		
		public static Texture2D loadImage(string _path)
		{
				return Resources.Load(_path, typeof(Texture2D)) as Texture2D;	
		}
		
		public static float dagreesToRads(float _dag)
		{
			return _dag / 180 * Mathf.PI;
		}
		
		public static float radsToDagrees(float _rad)
		{
			return _rad * 180 / Mathf.PI;
		}
		
		public static float ZeroCheck(float val)
		{
			return (val == 0) ? 1 : val;
		}
	}
}
