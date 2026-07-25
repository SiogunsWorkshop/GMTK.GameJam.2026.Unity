public class PlaythroughSummarySnapshotService : ISnapshot<object, PlaythroughSummarySnapshot>
{
    private PlaythroughSummarySnapshot _snapshot;

    public PlaythroughSummarySnapshot ReadSnapshot()
    {
        return _snapshot;
    }

    public void SaveSnapshot(object source)
    {
        throw new System.NotImplementedException();
    }
}

public struct PlaythroughSummarySnapshot
{
    public int Score { get; set; }
    public float Time { get; set; }
}