public class PlaythroughSummarySnapshotService : ISnapshot<(int score, float time), PlaythroughSummarySnapshot>
{
    private PlaythroughSummarySnapshot _snapshot;

    public PlaythroughSummarySnapshot ReadSnapshot()
    {
        return _snapshot;
    }

    public void SaveSnapshot((int score, float time) data)
    {
        _snapshot = new PlaythroughSummarySnapshot
        {
            Score = data.score,
            Time = data.time
        };
    }
}

public struct PlaythroughSummarySnapshot
{
    public int Score { get; set; }
    public float Time { get; set; }
}