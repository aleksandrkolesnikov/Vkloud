namespace VkApi

open FSharp.Json


type AuthInfo = {

    [<JsonField("access_token")>]
    AccessToken: string

    [<JsonField("user_id")>]
    UserId: uint64

}

