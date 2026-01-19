public class WC_Test : WC
{
    public override void Trigger(CoreEventContext context)
    {
        base.Trigger(context);

        print(WCData.name);
    }
}
