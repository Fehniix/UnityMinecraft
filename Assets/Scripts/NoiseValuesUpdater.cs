using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseValuesUpdater : MonoBehaviour
{
	public float landSimplex_x_mult = 0.8f;
	public float landSimplex_z_mult = 0.8f;

	public float landSimplex_mult = 10f;

	public float landSimplex2_x_mult = 1f;
	public float landSimplex2_z_mult = 1f;

	public float landSimplex2_mult = 10f;

	public static NoiseValuesUpdater instance;

	void Awake()
	{
		NoiseValuesUpdater.instance = this;
	}
}
