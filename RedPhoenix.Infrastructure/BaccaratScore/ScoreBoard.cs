namespace RedPhoenix.Infrastructure.BaccaratScore;
public class ScoreBoard
{
    private GameResult GameResultParser(char gameResult)
        => gameResult switch
        {
            'B' => GameResult.BankerWin,
            'P' => GameResult.PlayerWin,
            'T' => GameResult.Tie,
            _ => GameResult.Undefined
        };

    private PairResult PairResultParser(char pairResult)
        => pairResult switch
        {
            'b' => PairResult.BankerPair,
            'p' => PairResult.PlayerPair,
            'n' => PairResult.NoPair,
            'a' => PairResult.AllPair,
            _ => PairResult.Undefined
        };

    private RoundResult StringToRoundResult(string stringData, int order)
    {
        var gameResult = GameResultParser(stringData[0]);
        var pairResult = PairResultParser(stringData[2]);
        var result = ConvertToInt32(stringData[1]);

        if (result < 0 || result > 9)
        {
            throw new Exception($"[Invalid] Winner not in range: {result}");
        }

        if (gameResult == GameResult.Undefined)
        {
            throw new Exception($"[Invalid] Winner not in range: {gameResult}");
        }

        if (pairResult == PairResult.Undefined)
        {
            throw new Exception($"[Invalid] Winner not in range: {pairResult}");
        }

        return new RoundResult(order, result, gameResult, pairResult);
    }

    public IEnumerable<RoundResult> FromRawData(IEnumerable<string> data)
    {
        var order = 0;
        var listRoundData = new List<RoundResult>();
        foreach (var item in data)
        {
            var result = StringToRoundResult(item, order);
            listRoundData.Add(result);
            order++;
        }

        return listRoundData;
    }

    private int ConvertToInt32(char result)
    {
        try
        {
            return Convert.ToInt32(result.ToString());
        }
        catch
        {
            return -1;
        }
    }

}