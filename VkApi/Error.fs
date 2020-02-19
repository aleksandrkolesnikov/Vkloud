namespace VkApi

open FSharp.Json


type Error = {

    [<JsonField("error")>]
    InnerError: InnerError
}
and
    InnerError = {

        [<JsonField("error_code")>]
        Code: int

        [<JsonField("error_msg")>]
        Message: string
    }
