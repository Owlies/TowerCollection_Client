using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectController : MonoBehaviour {
    public MeshRenderer renderer;
    public bool submit;
    public bool remove;
    public bool submitValue;

    // for test use
    public string MaterialName;
    public int renderOrder;
    public float value;
    public string valueName;
    //

    public string[] effectNames;
    public Material[] materials;

    

    void Start()
    {
        if (effectNames.Length != materials.Length)
        {
            Debug.LogError("the 2 arrays must have same numbers of element and match one to one to one.");
        }
    }

    void Update()
    {
        if(submit)
        {
            submit = false;
            ApplyEffect(MaterialName, renderOrder);
        }

        if (submitValue)
        {
            submitValue = false;
            ChangeEffectValue(MaterialName, valueName, value);
        }

        if (remove)
        {
            remove = false;
            RemoveEffect(MaterialName);
        }
    }

    void ApplyEffect(string effectName, int renderOrder = -1)
    {
        for (int i=0; i < effectNames.Length; i++)
        {
            if (effectName == effectNames[i])
            {
                Material material = materials[i];
                ArrayList rendererMaterials = new ArrayList();
                rendererMaterials.AddRange(renderer.materials);

                if (renderOrder == -1)
                {
                    rendererMaterials.Add(material);
                }
                else
                {
                    rendererMaterials.Insert(renderOrder, material);
                }

                renderer.materials = rendererMaterials.ToArray(typeof(Material)) as Material[];
                break;
            }
        }
    }

    void RemoveEffect(string effectName)
    {
        for (int i = 0; i < effectNames.Length; i++)
        {
            if (effectName == effectNames[i])
            {
                Material material = materials[i];

                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    if (renderer.materials[j].name.Contains(material.name))
                    {
                        ArrayList materialsArray = new ArrayList();
                        materialsArray.AddRange(renderer.materials);
                        materialsArray.Remove(materialsArray[j]);
                        renderer.materials = materialsArray.ToArray(typeof(Material)) as Material[];
                        break;
                    }
                }

                break;
            }
        }
    }

    void ChangeEffectValue(string effectName, string valueName, float newValue)
    {
        for (int i = 0; i < effectNames.Length; i++)
        {
            if (effectName == effectNames[i])
            {
                Material material = materials[i];

                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    if (renderer.materials[j].name.Contains(material.name))
                    {
                        renderer.materials[j].SetFloat(valueName, newValue);
                        break;
                    }
                }

                break;
            }
        }
    }
}
