namespace RedPhoenix.Infrastructure.BaccaratScore;
public class DownRoadItem : RoadBase
{
    public DownRoadItem()
    {

    }
    public DownRoadItem(int order, bool repetition)
    {
        Order = order;
        Repetition = repetition;
        IsDecide = true;
    }

    public int Order { get; }
    public bool Repetition { get; }
}