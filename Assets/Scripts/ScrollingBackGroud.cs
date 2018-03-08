using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackGroud : MonoBehaviour
{

	[SerializeField] private bool Parallax;
	
	[SerializeField] private float backgroundSize;
	[SerializeField] private float paralaxSpeed;
	
	
	private Transform cameraTransform;
	private Transform[] layers;

	private float viewZone = 10;

	private int leftIndex;
	private int rightIndex;

	private float lastCameraX;
	
	// Use this for initialization
	void Start ()
	{
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		layers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			layers[i] = transform.GetChild(i);
		}

		leftIndex = 0;
		rightIndex = layers.Length - 1;
	}

	private void scrollLeft()
	{
		layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0)
		{
			rightIndex = layers.Length - 1;
		}
	}
	
	private void scrollRight()
	{
		layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length)
		{
			leftIndex = 0;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Parallax)
		{
			float deltaX = cameraTransform.position.x - lastCameraX;
			//transform.position += Vector3.right * (deltaX * paralaxSpeed);
			Vector3 targetPos = transform.position + Vector3.right * (deltaX * paralaxSpeed);
			transform.position = Vector3.Lerp(transform.position,targetPos, Time.deltaTime * 1.5f);
			lastCameraX = cameraTransform.position.x;
		}
		
		
		if (cameraTransform.position.x < (layers[leftIndex].transform.position.x+viewZone))
		{
			scrollLeft();
		}	
		
		if (cameraTransform.position.x > (layers[rightIndex].transform.position.x-viewZone))
		{
			scrollRight();
		}	
	}
}
