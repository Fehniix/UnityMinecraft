using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Block test = Blocks.Instantiate("cobblestone");
			Debug.Log(test);
		}
    }
	void printArray<T>(T[] arr)
	{
		string output = "[ ";
		foreach(T el in arr)
		{
			output += el + " ";
		}
		output += "]";
		Debug.Log(output);
	}
}
