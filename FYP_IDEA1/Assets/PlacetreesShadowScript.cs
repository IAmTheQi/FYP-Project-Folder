﻿using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlaceTreeShadowCasters 
{

	[@MenuItem ("Terrain/Place Tree Shadow Casters")]
	static void Run()
	{
		Terrain terrain = Terrain.activeTerrain;
		if (terrain == null ) 
		{
			return;
		}

		TerrainData td = terrain.terrainData;

		GameObject parent = new GameObject("Tree Shadow Casters");
		foreach (TreeInstance tree in td.treeInstances) 
		{
			Vector3 pos = Vector3.Scale(tree.position, td.size) + terrain.transform.position;

			TreePrototype treeProt = td.treePrototypes[tree.prototypeIndex];
			GameObject prefab = treeProt.prefab;

			GameObject obj = GameObject.Instantiate(prefab, pos, Quaternion.AngleAxis(tree.rotation, Vector3.up)) as GameObject;
			MeshRenderer renderer = obj.GetComponentInChildren<MeshRenderer>();
			renderer.receiveShadows = false;
			renderer.shadowCastingMode = ShadowCastingMode.On;
			GameObjectUtility.SetStaticEditorFlags(obj, StaticEditorFlags.LightmapStatic);

			Transform t = obj.transform;
			t.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
			t.parent = parent.transform;
		}
	}  
}