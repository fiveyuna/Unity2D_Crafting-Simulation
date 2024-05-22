using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CartoonFX;

public class ParticlePlay : MonoBehaviour
{
    [SerializeField] private GameObject[] particles;
    public static ParticlePlay instance;
    
    private void Awake()
    {
        instance = this;

        foreach (GameObject obj in particles)
        {
            obj.SetActive(false);
        }
    }

    public void PlayParticle(int index)
    {
        // Debug.Log("play");
        var effect = particles[index];
        effect.SetActive(true);
        
        var ps = effect.GetComponent<ParticleSystem>();
        if (ps.isEmitting) {
            ps.Stop(true);
        }
        else
        {
            if (!effect.gameObject.activeSelf) {
                effect.SetActive(true);
            }
            else {
                ps.Play(true);
                var cfxrEffects = effect.GetComponentsInChildren<CFXR_Effect>();

                foreach (var cfxr in cfxrEffects) {
                    cfxr.ResetState();
                }
            }
        }

        // yield return new WaitForSeconds(1);
    }

}
