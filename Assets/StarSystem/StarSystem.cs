using Assets;
using UnityEngine;
public class StarSystem : MonoBehaviour
{
    public GameObject StarPrefab;
    private GameObject SystemSectors;
    private GameObject star;
    void Start()
    {
        MaterialsHolder materialsHolder = FindObjectOfType<MaterialsHolder>();
        SystemSectors = new("SystemSectors");
        SystemSectors SystemSectorsComponent = SystemSectors.AddComponent<SystemSectors>();
        SystemSectorsComponent.CreateNew(new Material(materialsHolder.sectorMaterial), 8);

        Instantiate(StarPrefab, transform.transform);

        /*
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
         
        GameObject starSurface = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);

        ParticleSystem starSurface = new ParticleSystem();

        var starSurfaceShape = starSurface.shape;
        var starSurfaceMain = starSurface.main;

        starSurfaceShape.shapeType = ParticleSystemShapeType.Sphere;
        starSurfaceShape.radius = 10;
        starSurfaceShape.alignToDirection = true;

        starSurfaceMain.startSpeed = 0;
        starSurfaceMain.startSize = new ParticleSystem.MinMaxCurve(10, 25);
        starSurfaceShape.radius = 10;
        starSurfaceMain.startColor = Color.yellow;

        starSurface.transform.position = new Vector3();
        starSurface.transform.parent = transform;
        */
    }
    void Update()
    {

    }
    private StarSystem LoadStarSystemFromSave(string saveId)
    {
        return null;
    }
    private StarSystem CreateSystem()
    {
        return null;
    }
}