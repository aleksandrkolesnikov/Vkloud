namespace VkApi

open VkApi.Wrappers
open VkApi.Core
open FSharp.Control.Tasks.V2


module private Parser =

    open Newtonsoft.Json


    let TryParse<'T> =
        function
        | Error json ->
            let error = JsonConvert.DeserializeObject<Error> json
            match error.InnerError.Code with
            | 6 -> raise <| new TooManyRequestsPerSecond (error.InnerError.Message)
            | _ -> raise <| new System.Exception ("Unknown exception")
        | Ok json -> JsonConvert.DeserializeObject<'T> json


[<Sealed>]
type Client (login, password) =
    let apiVersion = "5.103"

    let authInfo = task {
            let clientId = 3697615
            let clientSecret = "AlVXZFMUqyrnABp8ncuU"
            let url = sprintf "https://oauth.vk.com/token?grant_type=password&client_id=%i&client_secret=%s&username=%s&password=%s" clientId clientSecret login password
            let! response = Requests.AsyncGet url

            return Parser.TryParse<AuthInfo> response
        }

    member self.GetDocuments () =
        task {
            let! info = authInfo
            let! response = sprintf "https://api.vk.com/method/docs.get?access_token=%s&v=%s" info.AccessToken apiVersion
                                |> Requests.AsyncGet

            let t = Parser.TryParse<Response<Collection<Document>>> response

            return t.Response.Items
        }

    member self.AddDocument filePath =
        let AsyncGetUploadServer (info: AuthInfo) =
            task {
                let! response = sprintf "https://api.vk.com/method/docs.getUploadServer?access_token=%s&v=%s" info.AccessToken apiVersion
                                    |> Requests.AsyncGet

                let t = Parser.TryParse<Response<UploadServer>> response

                return t.Response
            }

        let AsyncUploadDocument filePath (uploadServer: UploadServer) =
            task {
                let! response = Requests.AsyncPost uploadServer.Url filePath

                return Parser.TryParse<UplodedFileInfo> response
            }

        let AsyncSaveDocument (info: AuthInfo) (uploadedFile: UplodedFileInfo) =
            task {
                let! response = sprintf "https://api.vk.com/method/docs.save?access_token=%s&file=%s&title=%s&tags=%s&v=%s" info.AccessToken uploadedFile.Info uploadedFile.Title uploadedFile.Title apiVersion
                                    |> Requests.AsyncGet

                let t = Parser.TryParse<Response<Doc<Document>>> response

                return t.Response.Document
            }

        task {
            let! info = authInfo
            let! uploadServer = AsyncGetUploadServer info
            let! uploadedFile = AsyncUploadDocument filePath uploadServer

            return! AsyncSaveDocument info uploadedFile
        }

    member self.DeleteDocument (document: Document) =
        task {
            let! info = authInfo
            let! response = sprintf "https://api.vk.com/method/docs.delete?access_token=%s&owner_id=%i&doc_id=%i&v=%s" info.AccessToken document.OwnerId document.Id apiVersion
                            |> Requests.AsyncGet

            ignore <| Parser.TryParse<Response<int>> response
        }
