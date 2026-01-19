
using UnityEngine;

public class GrassPortal : GrassInstancingTest
{
    /// <summary>
    /// Center the patch on this object's transform (XZ), but keep Y = groundY.
    /// If you want to use the object's Y instead, change to:
    /// return new Vector3(transform.position.x, transform.position.y, transform.position.z);
    /// </summary>
    protected override Vector3 GetCenter()
    {
        return new Vector3(transform.position.x, /* Y */ GetGroundY(), transform.position.z);
    }

    /// <summary>
    /// Helper to read groundY from the base class via GetCenter default or via reflection if needed.
    /// Since groundY is private in the base, we rely on the default GetCenter pattern.
    /// If you prefer, you can expose groundY as protected in the base and just return it directly.
    /// </summary>
    private float GetGroundY()
    {
        // Base GetCenter uses (0, groundY, 0) by default; mirror that here.
        // If you changed groundY to 'protected', simply 'return groundY;'.
        return base.GetCenter().y;
    }
}
