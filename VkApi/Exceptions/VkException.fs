namespace VkApi.Exceptions

open System
open VkApi


type VkException (error) =
    inherit Exception (error.Message)

    member self.Code = error.Code
