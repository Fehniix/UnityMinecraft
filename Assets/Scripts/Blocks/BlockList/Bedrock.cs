using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bedrock : Block
{

	public Bedrock(): base()
	{
		this.blockName = "bedrock";
		this.breakable = false;
	}
}
