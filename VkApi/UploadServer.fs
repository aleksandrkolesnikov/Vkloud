namespace VkApi

open FSharp.Json


type UploadServer = {

    [<JsonField("upload_url")>]
    Url: string
}