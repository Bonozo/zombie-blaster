using UnityEngine;
using System.Collections;

public class RaycastTest : MonoBehaviour {
	
	Vector3 pos = new Vector3(0,0,0);
	
	void Update () 
	{
		if(!Input.GetMouseButtonDown(0)) return;
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (GameEnvironment.lastInput);	
		RaycastHit hit;
		Physics.Raycast(ray.origin,ray.direction,out hit);
		pos = hit.point;
		//Debug.Log("destinsation = " + pos);
	}
	
	Vector3 RaycastsTargetPosition(Camera mainCamera,Ray ray,RaycastHit hit)
	{
		float distfromcamera = hit.collider == null ? 100f : Vector3.Distance(mainCamera.transform.position,hit.collider.gameObject.transform.position);
		return mainCamera.transform.position + distfromcamera*ray.direction;
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawSphere(pos,0.05f);
	}
}
