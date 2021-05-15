using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
	bool IsSideFace()
	{
		return !(this.GetComponent<TopFace>() == null || this.GetComponent<BottomFace>() == null);
	}
}
