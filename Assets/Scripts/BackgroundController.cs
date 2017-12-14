using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
	public float BackgroundSize;
	public float ParallaxSpeed;
	public bool Paralax, Scrolling;
	
	private Transform cameraTransform;
	private Transform[] layers;
	[SerializeField]
	private float viewZone = 9;
	private int leftIndex, rightIndex;

	private float lastCameraX;
	
	private void Start()
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
		layers[rightIndex].position = new Vector3((layers[leftIndex].position.x - BackgroundSize), cameraTransform.position.y); 
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0)
		{
			rightIndex = layers.Length - 1;
		}
	}
	
	private void scrollRight()
	{

		layers[leftIndex].position = new Vector3((layers[rightIndex].position.x + BackgroundSize), cameraTransform.position.y);
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length)
		{
			leftIndex = 0;
		}
	}
	
	// v kazdem cyklu uprav pozadi vrstev pozadi
	private void Update()
	{
		if (Paralax)
			parallaxEffect();
		if (Scrolling)
			scrollingEffect();	
	}

	private void LateUpdate()
	{
		// updatujeme y souradnici, aby pozadi zustavalo v zornem poli kamery
		for (int i = 0; i < transform.childCount; i++)
		{
			layers[i].position = new Vector3(layers[i].position.x,cameraTransform.position.y, 0);
		}
	}

	private void parallaxEffect()
	{
		float deltaX = cameraTransform.position.x - lastCameraX;
		transform.position += Vector3.right * (deltaX * ParallaxSpeed);
		lastCameraX = cameraTransform.position.x;
	}

	private void scrollingEffect()
	{
		if (cameraTransform.position.x < (layers[leftIndex].position.x + viewZone))
			scrollLeft();
		
		if (cameraTransform.position.x > (layers[rightIndex].position.x - viewZone))
			scrollRight();
	}
	
	
}
