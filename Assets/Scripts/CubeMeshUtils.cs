using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

/// <summary>
/// Contains vertices information for each cube face.
/// </summary>
public struct CubeMeshFaces
{
	public static readonly Vector3[] front = {
		(0,0,0).ToVector3(),
		(0,1,0).ToVector3(),
		(1,1,0).ToVector3(),
		(1,0,0).ToVector3()
	};

	public static readonly Vector3[] back = {
		(1,0,1).ToVector3(),
		(1,1,1).ToVector3(),
		(0,1,1).ToVector3(),
		(0,0,1).ToVector3()
	};

	public static readonly Vector3[] top = {
		(0,1,0).ToVector3(),
		(0,1,1).ToVector3(),
		(1,1,1).ToVector3(),
		(1,1,0).ToVector3()
	};

	public static readonly Vector3[] bottom = {
		(1,0,0).ToVector3(),
		(1,0,1).ToVector3(),
		(0,0,1).ToVector3(),
		(0,0,0).ToVector3()
	};

	public static readonly Vector3[] west = {
		(0,0,1).ToVector3(),
		(0,1,1).ToVector3(),
		(0,1,0).ToVector3(),
		(0,0,0).ToVector3()
	};

	public static readonly Vector3[] east = {
		(1,0,0).ToVector3(),
		(1,1,0).ToVector3(),
		(1,1,1).ToVector3(),
		(1,0,1).ToVector3()
	};
}