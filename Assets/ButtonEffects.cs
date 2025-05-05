using UnityEngine;
public class ButtonEffects : MonoBehaviour
{
    public AudioSource myEffects;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void PlayHoverSound()
    {
        myEffects.PlayOneShot(hoverSound);
    }
    public void PlayClickSound()
    {
        myEffects.PlayOneShot(clickSound);
    }
}