public static class SubTreeLibrary
{
    public static Node FireBurst(Brain brain, ShootAt shootAt, int burstAmount, float waitBetween)
    {
        Sequence root = new Sequence();
        for(int i = 0; i < burstAmount; i++)
        {
            root.Add(shootAt);
            root.Add(new Wait(brain, waitBetween));
        }
        return root;
    }
}
