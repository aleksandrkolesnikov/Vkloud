namespace VkApi.Wrappers

open System.Collections.Generic
open Newtonsoft.Json


type Response<'T> =
    {
        [<JsonProperty "response">]
        Response: 'T
    }

[<NoComparison>]
type Collection<'T> =
    {
        [<JsonProperty "items">]
        Items: List<'T>
    }

type Doc<'T> =
    {
        [<JsonProperty "doc">]
        Document: 'T
    }
