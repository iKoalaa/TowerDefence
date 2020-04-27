using UnityEngine;

public class ResourceLoader : MonoBehaviour
{
    private static ResourceLoader _resource;

    public static ResourceLoader resource
    {
        get
        {
            if (_resource == null)
            {
                _resource = Instantiate(Resources.Load<ResourceLoader>("ResourceLoader"));
               
            }
            return _resource;
        }
    }

    public Transform trBullet;
}