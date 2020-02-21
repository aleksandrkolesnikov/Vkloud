namespace VkApi

open Newtonsoft.Json


type Error =
    {
        [<JsonProperty "error">]
        InnerError: InnerError
    }
and InnerError =
    {
        [<JsonProperty "error_code">]
        Code: int

        [<JsonProperty "error_msg">]
        Message: string
    }
