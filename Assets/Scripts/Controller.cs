using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 80, 8);

		Block b = Registry.Instantiate("dirt") as Dirt;
		Debug.Log(b?.blockName);
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
