
public class GameConfig
{
    public int SecondsOfAutoPoi = 60;
    public int[] PoiHPs = new[] { 10, 15, 30 };

    //1ステージの中に各難易度の魚が何種類いるか
    public int EasyFishTypesInStage = 3;
    public int NormalFishTypesInStage = 2;
    public int HardFishTypesInStage = 2;

    //各難易度、各種類の魚を何匹ずつ生成するか（= クリア条件の数）
    public int NumberOfEasyFish = 8;
    public int NumberOfNormalFish = 5;
    public int NumberOfHardFish = 3;

    //クリア条件として各難易度から何種類選出するか
    public int NumberOfEasyClearConditionTypes = 2;
    public int NumberOfNormalClearConditionTypes = 1;
    public int NumberOfHardClearConditionTypes = 1;
}