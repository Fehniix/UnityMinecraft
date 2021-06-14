using System.Collections;
using System.Linq;
using UnityEngine;

public class RainbowGenerator: Block
{
    public RainbowGenerator(): base()
	{
		this.blockName 			= "rainbowGenerator";
		this.textureName 		= "rainbow_generator";
		this.hardness 			= 1 * 20;
		this.toolTypeRequired	= ToolType.PICKAXE;
		this.miningLevel		= MiningLevel.DIAMOND;
	}

	public override Vector3? Place()
	{
		Vector3? placementCoords = base.Place();

		if (placementCoords == null)
			return null;

		ChunkPosition position 		= Player.instance.GetVoxelChunk();
		Chunk chunk 				= PCTerrain.GetInstance().chunks[position];

		GameObject _particleSystem 				= Resources.Load<GameObject>("Prefabs/SmokeExplosionParticles");
		GameObject particleSystem				= GameObject.Instantiate<GameObject>(_particleSystem);
		particleSystem.transform.parent 		= chunk.chunkGameObject.transform;
		particleSystem.transform.localPosition 	= placementCoords.Value;

		AudioSource c_complete	= AudioManager.Create3DSound("challenge_complete");
		c_complete.gameObject.transform.parent 			= chunk.chunkGameObject.transform;
		c_complete.gameObject.transform.localPosition 	= placementCoords.Value;
		c_complete.Play();

		AudioSource launch = AudioManager.Create3DSound("launch1");
		launch.gameObject.transform.parent 			= chunk.chunkGameObject.transform;
		launch.gameObject.transform.localPosition 	= placementCoords.Value;

		AudioSource explosion = AudioManager.Create3DSound("twinkle1");
		explosion.gameObject.transform.parent 			= chunk.chunkGameObject.transform;
		explosion.gameObject.transform.localPosition 	= placementCoords.Value;

		float[] launchTimes = new float[2] { Random.Range(0.4f, 0.5f), Random.Range(0.4f, 0.8f) };
		launch.GetComponent<DelayedAudio>().PlayDelayed(launch, launchTimes[0]);
		launch.GetComponent<DelayedAudio>().PlayDelayed(launch, launchTimes[1]);

		explosion.GetComponent<DelayedAudio>().PlayDelayed(explosion, launchTimes[0] + launch.clip.length);
		explosion.GetComponent<DelayedAudio>().PlayDelayed(explosion, launchTimes[1] + launch.clip.length);

		GameObject.Find("Controller").GetComponent<Controller>().RunAfterDelay(GUI.ShowGameWonUI, 1.5f);

		return placementCoords;
	}
}
