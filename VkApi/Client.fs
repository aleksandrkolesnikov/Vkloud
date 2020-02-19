namespace VkApi

open VkApi.Wrappers
open FSharp.Control.Tasks.V2


type Client (login, password) =
    let apiVersion = "5.103"
    let clientId = 3697615
    let clientSecret = "AlVXZFMUqyrnABp8ncuU"
    let url = sprintf "https://oauth.vk.com/token?grant_type=password&client_id=%i&client_secret=%s&username=%s&password=%s" clientId clientSecret login password
    let authInfo = task {
            return! url |> Requests.makeGetRequest<AuthInfo>
        }

    member self.GetDocuments () =
        task {
            let! info = authInfo
            let! response = sprintf "https://api.vk.com/method/docs.get?access_token=%s&v=%s" info.AccessToken apiVersion
                                |> Requests.makeGetRequest<Response<Collection<Document>>>

            return response.Response.Items
        }

    member self.AddDocument filePath =
        task {
            let! info = authInfo
            let! response = sprintf "https://api.vk.com/method/docs.getUploadServer?access_token=%s&v=%s" info.AccessToken apiVersion
                                |> Requests.makeGetRequest<Response<UploadServer>>

            let! uploadedFile = Requests.makePostRequest<UplodedFileInfo> response.Response.Url filePath
            let! response = sprintf "https://api.vk.com/method/docs.save?access_token=%s&file=%s&title=%s&tags=%s&v=%s" info.AccessToken uploadedFile.Info uploadedFile.Title uploadedFile.Title apiVersion
                                |> Requests.makeGetRequest<Response<Doc<Document>>>

            return response.Response.Document
        }

    member self.DeleteDocument (document: Document) =
        task {
            let! info = authInfo
            let! _ = sprintf "https://api.vk.com/method/docs.delete?access_token=%s&owner_id=%i&doc_id=%i&v=%s" info.AccessToken document.OwnerId document.Id apiVersion
                            |> Requests.makeGetRequest<Response<int>>

            ()
        }
