using Assets;
using System.Collections.Generic;
using UnityEngine;
public class Sector : MonoBehaviour
{
    public byte OwnerID = 255;
    public Dictionary<Civilization, float> CaptureProgresses = new();
    public Sector()
    {
        foreach (var Civilization in GameManager.Civilizations) CaptureProgresses.Add(Civilization, 0f);
    }
    public void IncreaseCaptureStatus(byte ownerID, float statusAmount)
    {
        Civilization civilization = GameManager.Civilizations[ownerID];
        CaptureProgresses[civilization] += statusAmount;
        if (CaptureProgresses[civilization] >= 100f)
        {
            CaptureProgresses[civilization] = 100f;
            OwnerID = ownerID;
            switch (OwnerID)
            {
                case 0:
                    gameObject.GetComponent<Renderer>().material = new(ReferenceHolder.Instance.friendlySectorMaterial);
                    break;
                case 1:
                    gameObject.GetComponent<Renderer>().material = new(ReferenceHolder.Instance.hostileSectorMaterial);
                    break;
                default:
                    break;
            }
            // GameManager.Civilizations[ownerID].Sectors.Add(this.gameObject);
        }
    }
}
