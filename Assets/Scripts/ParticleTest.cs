using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.UI;

public class ParticleTest : MonoBehaviour
{
    public GameObject[] particles;

    // [System.NonSerialized] public GameObject currentEffect;
    // GameObject[] effectsList;

    void Awake()
    {

        // var list = new List<GameObject>();
        foreach (GameObject obj in particles)
        {
            obj.SetActive(false);
        }
        // effectsList = list.ToArray();

        // // play at index()
        // if (currentEffect != null)
        // {
        //     currentEffect.SetActive(false);
        // }

        // currentEffect = effectsList[0];
        // currentEffect.SetActive(true);
    }

    public void PlayParticle_1()
    {
        var effect = particles[0];
        effect.SetActive(true);
        // var cfxrEffect= effect.GetComponent<CFXR_Effect>();
        
        var ps = effect.GetComponent<ParticleSystem>();
        if (ps.isEmitting)
        {
            ps.Stop(true);
        }
        else
        {
            if (!effect.gameObject.activeSelf)
            {
                effect.SetActive(true);
            }
            else
            {
                ps.Play(true);
                var cfxrEffects = effect.GetComponentsInChildren<CFXR_Effect>();
                foreach (var cfxr in cfxrEffects)
                {
                    cfxr.ResetState();
                }
            }
        }
        
}


}
