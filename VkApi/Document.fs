namespace VkApi

open FSharp.Json


type Document = {
    
    [<JsonField("id")>]
    Id: uint64

    [<JsonField("owner_id")>]
    OwnerId: uint64

    [<JsonField("title")>]
    Title: string

    [<JsonField("size")>]
    Size: uint64

    [<JsonField("date")>]
    Date: uint64
}
