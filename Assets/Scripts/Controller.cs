using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 80, 8);

		BlockItem.Render("cobblestone", GameObject.Find("item0").transform, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
