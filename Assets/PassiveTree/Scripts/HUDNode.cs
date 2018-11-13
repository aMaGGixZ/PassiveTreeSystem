using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum logic {or,and}

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class HUDNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Node node;

	[Header ("Requirements")]
	public HUDNode[] requiredNodes;
	public logic mode;

	[Header ("Tooltip")]
	public bool hasTooltip;

	[HideInInspector]
	public bool active, pickable;

	RectTransform rect;
	Button btn;
	Image img;
	Color notPicked = new Color (1, 1, 1, 0.5f);
	Color picked = new Color (1, 1, 1, 1);

	void Start () {
		rect = GetComponent<RectTransform> ();
		img = GetComponent<Image> ();
		btn = GetComponent<Button> ();

		if (active) {
			img.color = picked;
			addStats ();
		}
		else {img.color = notPicked;}

		btn.onClick.AddListener (pickNode);
	}

	void pickNode(){
		switch (mode) {
		case logic.or:
			foreach (HUDNode node in requiredNodes) {
				if (node.active) {
					pickable = true;
				}
			}
			break;
		case logic.and:
			int counter = 0;
			foreach (HUDNode node in requiredNodes) {
				if (node.active) {
					counter++;
				}
			}
			if (counter == requiredNodes.Length) {
				pickable = true;
			}
			break;
		}

		if (pickable && !active) {
			active = true;
			addStats ();
			img.color = picked;
		}
	}

	void addStats(){
		TreeManager.st.strBase += node.strenghtBase;
		TreeManager.st.intBase += node.intelligenceBase;
		TreeManager.st.dexBase += node.dexterityBase;
		TreeManager.st.strMult += node.strMulti;
		TreeManager.st.intMult += node.intMulti;
		TreeManager.st.dexMult += node.dexMulti;

		TreeManager.st.updateStats ();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (hasTooltip) {
			TreeManager.st.activateTooltip (true, new Vector3 (rect.position.x, rect.position.y - rect.rect.height/2 , rect.position.z), node.description);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		TreeManager.st.activateTooltip (false, Input.mousePosition, "nada");
	}
}
