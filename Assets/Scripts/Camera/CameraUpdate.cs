using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpdate : MonoBehaviour
{
	Vector3 offset = new Vector3(0, 0, -10);
	Vector3 targetPos;
	public bool isLerping = false;
	private Panning panning;

	private void Awake() {
		panning = GetComponent<Panning>();
	}
	public void CameraMoving(GameObject other)
	{
		targetPos = other.transform.position + offset;
		//isLerping = true;
	}
	private void LateUpdate()
	{
		if (isLerping == true)
		{
			Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
			transform.position = smoothPos;

			panning.LateUpdate();
		}
	}

}
