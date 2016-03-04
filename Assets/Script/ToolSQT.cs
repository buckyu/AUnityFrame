using UnityEngine;
using System.Collections;

public class ToolSQT 
{
	static SQT result = new SQT();
	public static SQT Lerp(SQT from, SQT to, float t)
	{
		if(from.Position_Weight==0||to.Position_Weight==0)
		{
			if(from.Position_Weight==0)
			{
				result.Position = to.Position;
				result.Position_Weight = to.Position_Weight;
			}
			else
			{
				result.Position = from.Position;
				result.Position_Weight = from.Position_Weight;
			}
		}
		else
		{
			result.Position = Vector3.Lerp(from.Position, to.Position, t);
			result.Position_Weight = 1;
		}

		if(from.Rotation_Weight==0||to.Rotation_Weight==0)
		{
			if(from.Rotation_Weight==0)
			{
				result.Rotation = to.Rotation;
				result.Rotation_Weight = to.Rotation_Weight;
			}
			else
			{
				result.Rotation = from.Rotation;
				result.Rotation_Weight = from.Rotation_Weight;
			}
		}
		else
		{
			result.Rotation = Quaternion.Slerp(from.Rotation, to.Rotation,t);
			result.Rotation_Weight = 1;
		}

		if(from.Scale_Weight==0||to.Scale_Weight==0)
		{
			if(from.Scale_Weight==0)
			{
				result.Scale = to.Scale;
				result.Scale_Weight  =to.Scale_Weight;
			}
			else
			{
				result.Scale = from.Scale;
				result.Scale_Weight = from.Scale_Weight;
			}
		}
		else
		{
			result.Scale = Vector3.Lerp(from.Scale, to.Scale, t);
			result.Scale_Weight  =1;
		}
		return result;
	}
}
