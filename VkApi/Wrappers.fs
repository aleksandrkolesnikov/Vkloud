namespace VkApi.Wrappers

open Newtonsoft.Json


type Response<'T> =
    {
        [<JsonProperty "response">]
        Response: 'T
    }

type Collection<'T> =
    {
        [<JsonProperty "items">]
        Items: 'T list
    }

type Doc<'T> =
    {
        [<JsonProperty "doc">]
        Document: 'T
    }
