namespace VkApi

open Newtonsoft.Json


type AuthInfo =
    {
        [<JsonProperty "access_token">]
        AccessToken: string

        [<JsonProperty "user_id">]
        UserId: uint64
    }

