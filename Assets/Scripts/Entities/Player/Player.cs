using UnityEngine;

public class Player : MonoBehaviour
{
    private void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterPlayer(this);
    }
}
