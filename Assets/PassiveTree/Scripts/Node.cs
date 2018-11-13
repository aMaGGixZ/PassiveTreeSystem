using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Node", menuName = "Node")]
public class Node : ScriptableObject {
	[Header("Stats")]
	public int strenghtBase;
	public int intelligenceBase;
	public int dexterityBase;

	public int strMulti;
	public int intMulti;
	public int dexMulti;

	[TextArea]
	public string description;
}
