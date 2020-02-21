namespace VkApi

open VkApi.Wrappers
open VkApi.Core
open FSharp.Control.Tasks.V2


type Client (login, password) =
    let apiVersion = "5.103"
    let clientId = 3697615
    let clientSecret = "AlVXZFMUqyrnABp8ncuU"
    let url = sprintf "https://oauth.vk.com/token?grant_type=password&client_id=%i&client_secret=%s&username=%s&password=%s" clientId clientSecret login password
    let authInfo = task {
            return! Requests.AsyncGet<AuthInfo> url
        }

    member self.GetDocuments () =
        task {
            let! info = authInfo
            let! response = sprintf "https://api.vk.com/method/docs.get?access_token=%s&v=%s" info.AccessToken apiVersion
                                |> Requests.AsyncGet<Response<Collection<Document>>>

            return response.Response.Items
        }

    member self.AddDocument filePath =
        let AsyncGetUploadServer info =
            task {
                let! response = sprintf "https://api.vk.com/method/docs.getUploadServer?access_token=%s&v=%s" info.AccessToken apiVersion
                                    |> Requests.AsyncGet<Response<UploadServer>>

                return response.Response
            }

        let AsyncUploadDocument filePath uploadServer =
            Requests.AsyncPost<UplodedFileInfo> uploadServer.Url filePath

        let AsyncSaveDocument info uploadedFile =
            task {
                let! response = sprintf "https://api.vk.com/method/docs.save?access_token=%s&file=%s&title=%s&tags=%s&v=%s" info.AccessToken uploadedFile.Info uploadedFile.Title uploadedFile.Title apiVersion
                                    |> Requests.AsyncGet<Response<Doc<Document>>>

                return response.Response.Document
            }

        task {
            let! info = authInfo
            let! uploadServer = AsyncGetUploadServer info
            let! uploadedFile = AsyncUploadDocument filePath uploadServer
            return! AsyncSaveDocument info uploadedFile
        }

    member self.DeleteDocument document =
        task {
            let! info = authInfo
            let! _ = sprintf "https://api.vk.com/method/docs.delete?access_token=%s&owner_id=%i&doc_id=%i&v=%s" info.AccessToken document.OwnerId document.Id apiVersion
                            |> Requests.AsyncGet<Response<int>>

            ()
        }
