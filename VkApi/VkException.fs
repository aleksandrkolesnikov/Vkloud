namespace VkApi

open System
open VkApi


type VkException (error: InnerError) =
    inherit Exception (error.Message)

    member self.Code = error.Code
