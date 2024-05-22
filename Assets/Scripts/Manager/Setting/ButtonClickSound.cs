using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;

public class ButtonClickSound : MonoBehaviour
{
    [SerializeField] private SoundName soundName = SoundName.SFX_UI_Click;

    private void Start()
    {
        if ( this.GetComponent<Button>() != null ) {
            Button button = this.GetComponent<Button>();
            button.OnClickAsObservable()
            .Subscribe(_ => 
            {
                AudioManager.Inst.PlayOneShot(soundName);
            })
            .AddTo(gameObject);
        }
        // else if (this.GetComponent<Toggle>() != null) {
        //     Toggle toggle = this.GetComponent<Toggle>();
        //     // toggle.OnValueChangedAsObservable()
        //     // .Subscribe(_ =>{
        //     //     AudioManager.Inst.PlayOneShot(soundName);
        //     // })
        //     // .AddTo(gameObject);
        //     toggle.onValueChanged.AddListener();
        // }  
    }
        
    
}
