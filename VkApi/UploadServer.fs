namespace VkApi

open Newtonsoft.Json


type UploadServer =
    {
        [<JsonProperty "upload_url">]
        Url: string
    }