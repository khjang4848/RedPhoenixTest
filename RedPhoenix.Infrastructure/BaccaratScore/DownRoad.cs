namespace RedPhoenix.Infrastructure.BaccaratScore;

public abstract class DownRoad : Road<DownRoadItem>
{
    private class IndexItem(int rowIndex, int columnIndex, int order)
    {
        public int RowIndex { get; } = rowIndex;
        public int ColumnIndex { get; } = columnIndex;
        public int Order { get; } = order;
    }



    protected DownRoad(int row, int column, IEnumerable<RoundResult> result) : base(row, column, result)
    {
    }

    public abstract DownRoadItem BankerPrediction();
    public abstract DownRoadItem PlayerPrediction();

    protected DownRoadItem GetPrediction(RoundResult fakeNextRound, DownRoadGap downRoadGap)
    {
        var fakeRoundResults = RoundResults.ToList();
        fakeRoundResults.Add(fakeNextRound);

        var fakeBigRoad = new SharedBigRoad(Row, Column, fakeRoundResults);

        var fakeDownRoadData = GenerateDownRoadData(fakeBigRoad.ResultItems,
            downRoadGap);

        var fakeNextDownRoadItem = fakeDownRoadData[^1];


        return fakeNextDownRoadItem;

    }

    public List<DownRoadItem> GenerateDownRoadData(
        List<List<BigRoadItem>> bigRoadGraph, DownRoadGap rowGap)
    {
        var maxColumnCount = Math.Max(0, bigRoadGraph.Max(x => x.Count));
        var downGraphArr = new List<DownRoadItem>();
        var indexArr = new List<IndexItem>();
        var lengthArr = new List<int>();

        for (var columnIndex = 0; columnIndex < maxColumnCount; columnIndex++)
        {
            var currentLength = 0;
            for (var rowIndex = 0; rowIndex < bigRoadGraph.Count; rowIndex++)
            {
                var item = bigRoadGraph[rowIndex][columnIndex];

                if (item == null)
                {
                    continue;
                }

                currentLength += 1;
                indexArr.Add(new IndexItem(rowIndex, columnIndex, item.Order));
            }

            lengthArr.Add(currentLength);
        }

        indexArr.Sort((a, b) => a.Order - b.Order);
        foreach (var item in indexArr.Where(item => item.Order >= GetBeginIndex(bigRoadGraph, rowGap)))
        {
            if (item.RowIndex == 0)
            {
                downGraphArr.Add(new DownRoadItem(item.Order,
                    lengthArr[item.ColumnIndex - (int)rowGap - 1] ==
                    lengthArr[item.ColumnIndex - 1]));
            }
            else
            {
                var lastItem = bigRoadGraph[item.RowIndex - 1][item.ColumnIndex - (int)rowGap];
                var targetItem = bigRoadGraph[item.RowIndex][item.ColumnIndex - (int)rowGap];

                downGraphArr.Add(new DownRoadItem(item.Order, lastItem == null || targetItem != null));
            }
        }

        return downGraphArr;
    }





    private int GetBeginIndex(List<List<BigRoadItem>> bigRoadGraph, DownRoadGap rowGap)
    {
        var item1 = bigRoadGraph[1][(int)rowGap];
        var item2 = bigRoadGraph[0][(int)rowGap + 1];
        var startIndex = 0;

        if (item1 != null)
        {
            startIndex = item1.Order;
        }
        else if (item2 != null)
        {
            startIndex = item2.Order;
        }

        return startIndex;
    }
}
