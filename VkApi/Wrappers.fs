namespace VkApi.Wrappers

open FSharp.Json


type Response<'T> = {

    [<JsonField("response")>]
    Response: 'T
}

type Collection<'T> = {
    
    [<JsonField("items")>]
    Items: 'T list
}

type Doc<'T> = {

    [<JsonField("doc")>]
    Document: 'T
}
