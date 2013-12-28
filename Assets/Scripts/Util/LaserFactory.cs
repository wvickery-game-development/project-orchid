using UnityEngine;
using System.Collections;

public class LaserFactory: MonoBehaviour{

    public IEnumerator Fade(GameObject laserGameObject, LineRenderer laserRenderer, Color colour)
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = colour;
            c.a = f;
            laserRenderer.SetColors(c, c);
            yield return 0;
        }
        GameObject.Destroy(laserGameObject);
    }

    
    public void ShootLaser(Vector2 start, Vector2 end, Color colour)
    {
        GameObject laserGameObject = new GameObject();
        LineRenderer laserRenderer = laserGameObject.AddComponent<LineRenderer>();
        laserRenderer.material = Resources.Load("Line") as Material;   

        laserRenderer.SetPosition(0, start);
        laserRenderer.SetPosition(1, end);

        laserRenderer.SetWidth(1f, 1f);

        laserRenderer.SetColors(colour, colour);

        StartCoroutine(Fade(laserGameObject, laserRenderer, colour));
    }


}
