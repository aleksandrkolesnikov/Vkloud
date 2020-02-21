namespace VkApi

open Newtonsoft.Json


type UplodedFileInfo =
    {
        [<JsonProperty "file">]
        Info: string
    }
    with
        member self.Title = self.Info.Split("|").[7]
        member self.Hash = self.Info.Split("|").[8]

