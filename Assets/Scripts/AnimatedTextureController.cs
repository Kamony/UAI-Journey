using UnityEngine;
using System.Collections;

//Apply to the mesh whose material's texture you're gonna change
//Variable ammount of textures can be set, default is 1 
//Single delay timing between textures for simpler inspector
//you can make an array of delays, one for each texture if you really need that. 
//or create some sort of "special delay" that overrides the default for certain textures. 
public class AnimatedTextureController : MonoBehaviour
{
    public bool changeTextures = true;
    public float Delay = 0.5f;
    public Texture2D[] textures = new Texture2D[1];

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