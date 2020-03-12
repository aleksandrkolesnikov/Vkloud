namespace VkApi

open System
open Newtonsoft.Json


type Document =
    struct
        val Id: uint64
        val OwnerId: uint64
        val Title: string
        val Size: uint64
        val private UnixDate: uint64

        [<JsonConstructor>]
        new (id, owner_id, title, size, date) = {
            Id = id
            OwnerId = owner_id
            Title = title
            Size = size
            UnixDate = date
        }

        member self.Date =
            let unixEpoch = DateTime (1970, 1, 1, 0, 0, 0, 0)
            self.UnixDate |> float |> unixEpoch.AddSeconds
    end