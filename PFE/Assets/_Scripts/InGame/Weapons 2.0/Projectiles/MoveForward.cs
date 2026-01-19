using Unity.Netcode;
using UnityEngine;

public class MoveForward : NetworkBehaviour
{
    [SerializeField] float _speed = 5f;

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        print("suu");

        if(collider.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage();
        }
        
    }
}
