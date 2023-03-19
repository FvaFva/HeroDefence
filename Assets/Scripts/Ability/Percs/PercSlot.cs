public struct PercSlot
{
    public Perc Perc { get; private set; }
    public IPercSource Source { get; private set; }

    public PercSlot(IPercSource source, Perc perc)
    {
        Perc = perc;
        Source = source;
    }
}
