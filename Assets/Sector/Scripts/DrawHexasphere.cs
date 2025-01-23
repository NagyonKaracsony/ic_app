using System;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawHexasphere : MonoBehaviour
{
    [Min(0.5f)]
    [SerializeField] public float radius = 2f;
    [Range(1, 15)]
    [SerializeField] public int divisions = 4;
    [Range(0.1f, 1f)]
    [SerializeField] public float hexSize = 1f;

    private Hexasphere hexSphere;
    private GameObject sectors;
    public Material sectorMaterial;
    private void Start()
    {
        sectors = new GameObject("Sectors");
        sectors.transform.parent = transform;

        hexSphere = new Hexasphere(radius, divisions, hexSize, sectorMaterial, sectors);
    }
    private void Update()
    {
        /*
        if (sectors != null)
        {
            foreach (Transform sector in sectors.transform)
            {
                GameObject sectorGameObject = sector.gameObject;

                Debug.Log($"Found sector: {sectorGameObject.name}");

                Renderer renderer = sectorGameObject.GetComponent<Renderer>();

            }
        }
        else Debug.LogError("Sectors GameObject reference is not set.");
        */
    }
}