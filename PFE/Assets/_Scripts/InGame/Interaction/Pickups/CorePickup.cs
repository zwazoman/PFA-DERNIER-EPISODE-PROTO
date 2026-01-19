public class CorePickup : Pickup<Core>
{
    protected override void Awake()
    {
        base.Awake();

        TryGetComponent(out linkedObject);
    }

    protected override async void TryPickup(PlayerInteraction interaction)
    {
        bool coreLinked = await interaction.main.playerWeaponHandler.LinkCore(linkedObject);

        if (coreLinked)
        {
            base.TryPickup(interaction);
        }
    }
}
