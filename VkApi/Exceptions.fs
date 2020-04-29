namespace VkApi

open System


[<Sealed>]
type TooManyRequestsPerSecond (message) =
    inherit Exception (message)