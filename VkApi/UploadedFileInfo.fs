namespace VkApi

open FSharp.Json


type UplodedFileInfo = {

    [<JsonField("file")>]
    Info: string
} with

    member self.Title = self.Info.Split("|").[7]
    member self.Hash = self.Info.Split("|").[8]

