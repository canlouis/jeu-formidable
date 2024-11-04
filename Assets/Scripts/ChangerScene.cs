using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangerScene : MonoBehaviour
{
    [SerializeField] private AudioClip _sonBouton;
    void Start()
    {
        
    }

   public void Aller(string nomScene)
   {
    SoundManager.instance.Jouer(_sonBouton);
    SceneManager.LoadScene(nomScene);
   }
}

