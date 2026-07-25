public interface ISnapshot<TSource, TSnapshot> where TSnapshot : struct
{
    public void SaveSnapshot(TSource source);
    public TSnapshot ReadSnapshot();
}
