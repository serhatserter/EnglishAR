using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectItem : MonoBehaviour {
	bool click = true;
	public Material[] normal ,degisen;
	public List<GameObject> secList;
	GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit)
			{
				GameObject obje = hitInfo.transform.gameObject;
				Material select = Resources.Load("Material", typeof(Material)) as Material;
				Material bos = null;

				

				degisen = obje.GetComponent<MeshRenderer>().materials;
				normal = obje.GetComponent<MeshRenderer>().materials;

				obje.transform.DOShakeScale(1f, 0.01f, 5 ,5,false);

				if (degisen[degisen.Length-1].name != "Material (Instance)") { 

					degisen[degisen.Length - 1] = select;
					obje.GetComponent<MeshRenderer>().materials = degisen;
					secList.Add(obje);

				
				}
				else
				{
					
					normal[normal.Length - 1] = bos;
					obje.GetComponent<MeshRenderer>().materials = normal;
					secList.Remove(obje);
				}

			}

		}
	}
}
