using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Dialogue
{
	[TextArea(3,5)]
	public string[] sentences;
}
