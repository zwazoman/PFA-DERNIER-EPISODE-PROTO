using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShootingWeapon : Item
{
    [Header("References")]
    [SerializeField] Transform _shootSocket;

    [Header("Weapon Parameters")]
    [SerializeField] GameObject _projectile;
    [SerializeField] float tmpDelay;
    [SerializeField] float rateOfFire;

    protected bool canShoot = true;
    protected bool isWaiting = false;

    float _timer = 0;

    public override void StartUsing()
    {
        base.StartUsing();

        TryShoot();
        ShootDelay();

    }

    public override void UseUpdate()
    {
        TryShoot();
    }

    public async void ShootDelay()
    {
        if (isWaiting)
            return;

        isWaiting = true;

        while (_timer < tmpDelay)
        {
            _timer += Time.deltaTime;
            await UniTask.Yield();
        }

        canShoot = true;
        isWaiting = false;
    }

    public override void StopUsing()
    {
        base.StopUsing();
    }

    public virtual void TryShoot()
    {
        if (canShoot)
        {
            print("shoot");
        }
        //spawn le projectile
    }

}
