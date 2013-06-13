using UnityEngine;
using System;


public class RXUtils
{
	public RXUtils ()
	{
		
	}
	
	public static Rect ExpandRect(Rect rect, float paddingX, float paddingY)
	{
		return new Rect(rect.x - paddingX, rect.y - paddingY, rect.width + paddingX*2, rect.height+paddingY*2);	
	}
	
	public static void LogRect(string name, Rect rect)
	{
		Debug.Log (name+": ("+rect.x+","+rect.y+","+rect.width+","+rect.height+")");	
	}
	
	public static void LogVectors(string name, params Vector2[] args)
	{
		string result = name + ": " + args.Length + " Vector2 "+ args[0].ToString()+"";

		for(int a = 1; a<args.Length; ++a)
		{
			Vector2 arg = args[a];
			result = result + ", "+ arg.ToString()+"";	
		}
		
		Debug.Log(result);
	}
	
	public static void LogVectors(string name, params Vector3[] args)
	{
		string result = name + ": " + args.Length + " Vector3 "+args[0].ToString()+"";

		for(int a = 1; a<args.Length; ++a)
		{
			Vector3 arg = args[a];
			result = result + ", "+ arg.ToString()+"";	
		}
		
		Debug.Log(result);
	}
	
	public static void LogVectorsDetailed(string name, params Vector2[] args)
	{
		string result = name + ": " + args.Length + " Vector2 "+ VectorDetailedToString(args[0])+"";
		
		for(int a = 1; a<args.Length; ++a)
		{
			Vector2 arg = args[a];
			result = result + ", "+ VectorDetailedToString(arg)+"";	
		}
		
		Debug.Log(result);
	}
	
	public static string VectorDetailedToString(Vector2 vector)
	{
		return "("+vector.x + "," + vector.y +")";
	}
	
	public static Color GetColorFromHex(uint hex)
	{
		uint red = hex >> 16;
		uint greenBlue = hex - (red<<16);
		uint green = greenBlue >> 8;
		uint blue = greenBlue - (green << 8);
		
		return new Color(red/255.0f, green/255.0f, blue/255.0f);
	}
	
}

public class RXColor
{
	public const float HUE_RED = 0.0f;
	public const float HUE_ORANGE = 0.1f;
	public const float HUE_YELLOW = 0.16f;
	public const float HUE_GREEN = 0.25f;
	public const float HUE_CYAN = 0.5f;
	public const float HUE_BLUE = 0.6f;
	public const float HUE_PURPLE = 0.8f;
	public const float HUE_PINK = 0.9f;
	
	public static Color ColorFromHSL(float hue, float sat, float lum)
	{
		return ColorFromHSL(hue,sat,lum,1.0f);	
	}
	
	//hue goes from 0 to 1
	public static Color ColorFromHSL(float hue, float sat, float lum, float alpha) //default - sat:1, lum:0.5
	{
		hue = (100000.0f+hue)%1f; //hue wraps around
		
		float r = lum;
		float g = lum;
		float b = lum;

        float v = (lum <= 0.5f) ? (lum * (1.0f + sat)) : (lum + sat - lum * sat);
		 
        if (v > 0)
        {
			float m = lum + lum - v;
			float sv = (v - m ) / v;
			
			hue *= 6.0f;
			
			int sextant = (int) hue;
			float fract = hue - sextant;
			float vsf = v * sv * fract;
			float mid1 = m + vsf;
			float mid2 = v - vsf;
			
			switch (sextant)
			{
				case 0:
				      r = v;
				      g = mid1;
				      b = m;
				      break;
				case 1:
				      r = mid2;
				      g = v;
				      b = m;
				      break;
				case 2:
				      r = m;
				      g = v;
				      b = mid1;
				      break;
				case 3:
				      r = m;
				      g = mid2;
				      b = v;
				      break;
				case 4:
				      r = mid1;
				      g = m;
				      b = v;
				      break;
				case 5:
				      r = v;
				      g = m;
				      b = mid2;
				      break;
              }
        }
		
		return new Color(r,g,b,alpha);
	}
	
	public static Color GetColorFromHex(uint hex)
	{
		uint red = hex >> 16;
		uint greenBlue = hex - (red<<16);
		uint green = greenBlue >> 8;
		uint blue = greenBlue - (green << 8);
		
		return new Color(red/255.0f, green/255.0f, blue/255.0f);
	}
}

public class RXMath
{
	public const float RTOD = 180.0f/Mathf.PI;
	public const float DTOR = Mathf.PI/180.0f;
	public const float DOUBLE_PI = Mathf.PI*2.0f;
	public const float HALF_PI = Mathf.PI/2.0f;
	public const float PI = Mathf.PI;
	public const float INVERSE_PI = 1.0f/Mathf.PI;
	public const float INVERSE_DOUBLE_PI = 1.0f/(Mathf.PI*2.0f);
	
	public static int Wrap(int input, int range)
	{
		return (input + (range*1000000)) % range;	
	}
	
	public static float Wrap(float input, float range)
	{
		return (input + (range*1000000)) % range;	
	}
	
	
	public static float getDegreeDelta(float startAngle, float endAngle) //chooses the shortest angular distance
	{
		float delta = (endAngle - startAngle) % 360.0f;
		
		if (delta != delta % 180.0f) 
		{
			delta = (delta < 0) ? delta + 360.0f : delta - 360.0f;
		}	
		
		return delta;
	}
	
	public static float getRadianDelta(float startAngle, float endAngle) //chooses the shortest angular distance
	{
		float delta = (endAngle - startAngle) % DOUBLE_PI;
		
		if (delta != delta % Mathf.PI) 
		{
			delta = (delta < 0) ? delta + DOUBLE_PI : delta - DOUBLE_PI;
		}	
		
		return delta;
	}
	
	//normalized ping pong (apparently Unity has this built in... so yeah) - Mathf.PingPong()
	public static float PingPong(float input, float range)
	{
		float first = ((input + (range*1000000.0f)) % range)/range; //0 to 1
		if(first < 0.5f) return first*2.0f;
		else return 1.0f - ((first - 0.5f)*2.0f); 
	}
	
}

public class RXRandom
{
	private static System.Random _randomSource = new System.Random();
	
	public static float Float()
	{
		return (float)_randomSource.NextDouble();
	}
	
	public static float Float(int seed)
	{
		return (float)new System.Random(seed).NextDouble();
	}
	
	public static double Double()
	{
		return _randomSource.NextDouble();
	}
	
	public static float Float(float max)
	{
		return (float)_randomSource.NextDouble() * max;
	}
	
	public static int Int()
	{
		return _randomSource.Next();
	}
	
	public static int Int(int max)
	{
		return _randomSource.Next() % max;
	}
	
	public static float Range(float low, float high)
	{
		return low + (high-low)*(float)_randomSource.NextDouble();
	}
	
	public static int Range(int low, int high)
	{
		return low + _randomSource.Next() % (high-low); 
	}
	
	public static bool Bool()
	{
		return _randomSource.NextDouble() < 0.5;	
	}
	
	public static object Select(params object[] objects)
	{
		return objects[_randomSource.Next() % objects.Length];
	}
	
	//this isn't really perfectly randomized, but good enough for most purposes
	public static Vector2 Vector2Normalized()
	{
		return new Vector2(RXRandom.Range(-1.0f,1.0f),RXRandom.Range(-1.0f,1.0f)).normalized;
	}
	
	public static Vector3 Vector3Normalized()
	{
		return new Vector3(RXRandom.Range(-1.0f,1.0f),RXRandom.Range(-1.0f,1.0f),RXRandom.Range(-1.0f,1.0f)).normalized;
	}
}

public class RXCircle
{
	public Vector2 center;
	public float radius;
	public float radiusSquared;
	
	public RXCircle(Vector2 center, float radius)
	{
		this.center = center;
		this.radius = radius;
		this.radiusSquared = radius * radius;
	}
	
	public bool CheckIntersectWithRect(Rect rect)
	{
		return rect.CheckIntersectWithCircle(this);
	}
	
	public bool CheckIntersectWithCircle(RXCircle circle)
	{
		Vector2 delta = circle.center - this.center;
		float radiusSumSquared = (circle.radius + this.radius) * (circle.radius + this.radius);
		return (delta.sqrMagnitude <= radiusSumSquared);
	}
}


//public static class ArrayExtensions
//{
//	public static string toJson( this Array obj )
//	{
//		return MiniJSON.jsonEncode( obj );
//	}
//	
//}

