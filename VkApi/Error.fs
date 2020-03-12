namespace VkApi

open Newtonsoft.Json


type internal Error =
    struct
        val InnerError: InnerError

        [<JsonConstructor>]
        new error = { InnerError = error }
    end
and InnerError =
    struct
        val Code: int
        val Message: string

        [<JsonConstructor>]
        new (error_code, error_msg) = {
            Code = error_code
            Message = error_msg
        }
    end
