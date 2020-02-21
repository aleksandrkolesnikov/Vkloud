namespace VkApi

open Newtonsoft.Json


type Document =
    {
        [<JsonProperty "id">]
        Id: uint64

        [<JsonProperty "owner_id">]
        OwnerId: uint64

        [<JsonProperty "title">]
        Title: string

        [<JsonProperty "size">]
        Size: uint64

        [<JsonProperty "date">]
        Date: uint64
    }
