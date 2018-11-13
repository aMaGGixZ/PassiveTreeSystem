using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIZoom : MonoBehaviour, IScrollHandler {

	Vector3 sScale;
	float zoomSpeed = 0.1f;
	public float maxZoom = 1f;
	public float minZoom = -0.1f;

	void Awake () {
		sScale = transform.localScale;
	}
	
	public void OnScroll (PointerEventData data) {
		Vector3 delta = Vector3.one * (data.scrollDelta.y * zoomSpeed);
		Vector3 desiredScale = transform.localScale + delta;

		desiredScale = clampScale (desiredScale);
		transform.localScale = desiredScale;
	}

	Vector3 clampScale (Vector3 scale){
		scale = Vector3.Max (sScale * minZoom, scale);
		scale = Vector3.Min (sScale * maxZoom, scale);

		return scale;
	}
}
