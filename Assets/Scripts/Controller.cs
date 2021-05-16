using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject block = CachedResources.Load<GameObject>("Prefabs/Blocks/Block.Dirt");
		GameObject b2 = CachedResources.Load<GameObject>("Prefabs/Blocks/Block.Cobblestone");
		GameObject instantiatedBlock = Instantiate<GameObject>(block, new Vector3(2,2,2), Quaternion.identity);

		for (int i = 0; i < 3; i++)
			for (int k = 0; k < 3; k++)
				Instantiate<GameObject>(b2, new Vector3(i,0,k), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
