namespace RedPhoenix.Infrastructure.BaccaratScore;
public enum GameResult
{
    Tie,
    BankerWin,
    PlayerWin,
    Undefined
}

public enum PairResult
{
    NoPair,
    BankerPair,
    PlayerPair,
    AllPair,
    Undefined
}

public class RoundResult(int order, int result, GameResult gameResult, PairResult pairResult)
{
    public int Order { get; } = order;
    public int Result { get; } = result;
    public GameResult GameResult { get; } = gameResult;
    public PairResult PairResult { get; } = pairResult;
}