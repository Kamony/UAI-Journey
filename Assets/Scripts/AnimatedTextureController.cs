using UnityEngine;
using System.Collections;

public class AnimatedTextureController : MonoBehaviour
{
    [SerializeField] private bool changeTextures = true;
    [SerializeField] private float Delay = 0.5f;
    [SerializeField] private Texture2D[] textures = new Texture2D[1];

    void Start()
    {
        StartCoroutine(change());
    }

    IEnumerator change()
    {
        while (changeTextures)
        {
            int i = 0;
            do
            {
                yield return new WaitForSeconds(Delay);
                GetComponent<Renderer>().material.mainTexture = textures[i];

                i++;
            } while (i < textures.Length);
            if (i > textures.Length)
            {
                i = 0;
            }
        }
    }
}﻿