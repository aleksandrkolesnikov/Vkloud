namespace VkApi.Wrappers

open System.Collections.Generic
open Newtonsoft.Json


type internal Response<'T> =
    struct
        val Response: 'T

        [<JsonConstructor>]
        new response = { Response = response }
    end

[<NoComparison>]
type internal Collection<'T> =
    struct
        val Items: List<'T>

        [<JsonConstructor>]
        new items = { Items = items }
    end

type internal Doc<'T> =
    struct
        val Document: 'T

        [<JsonConstructor>]
        new doc = { Document = doc }
    end