using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameServer.Types.Item
{

    [JsonConverter(typeof(StringEnumConverter))]
    enum WeaponTypes
    {
        OneHand,
        TwoHand
    }
}
