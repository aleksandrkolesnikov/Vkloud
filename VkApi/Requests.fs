namespace VkApi.Core

module internal Requests =

    open System
    open System.Text
    open System.IO
    open System.Net
    open FSharp.Control.Tasks.V2
    open Newtonsoft.Json
    open VkApi
    open VkApi.Exceptions


    let private tryConvert<'T> content =
        let (?) (content: string) =
            if content.Contains "error" = true then
                let error = content |> JsonConvert.DeserializeObject<Error>
                Error error.InnerError
            else
                Ok content
           
        match (?) content with
        | Error error -> raise <| new VkException (error)
        | Ok response -> response |> JsonConvert.DeserializeObject<'T>

    let makeGetRequest<'T> (url: string) =
        task {
            let httpRequest = url |> WebRequest.CreateHttp
            use! response = httpRequest.GetResponseAsync ()
            use stream = response.GetResponseStream ()
            use reader = new StreamReader (stream)
            let! content = reader.ReadToEndAsync ()

            return content |> tryConvert<'T>
        }

    let makePostRequest<'T> (url: string) filePath =
        let boundary = Guid.NewGuid () |> string

        let requestBody filePath =
            let contentType =
                let (|FileExtension|) (filePath: string) = Path.GetExtension filePath

                function
                | FileExtension ".txt" -> "text/plain"
                | FileExtension ".gif" -> "image/gif"
                | FileExtension ".jpeg" | FileExtension ".jpg" -> "image/jpeg"
                | FileExtension ".png" -> "image/png"
                | FileExtension ".pdf" -> "image/pdf"
                | _ -> "application/octet-stream"

            let content =
                task {
                    use stream = filePath |> File.OpenRead
                    let buffer = stream.Length |> int |> Array.zeroCreate<byte>
                    let! _ = stream.ReadAsync (buffer, 0, buffer.Length)

                    return buffer
                }

            let header = sprintf "--%s\r\nContent-Disposition: form-data; name=file; filename=\"%s\"\r\nContent-Type:%s\r\n\r\n" boundary (Path.GetFileName filePath) (contentType filePath)
                            |> Encoding.UTF8.GetBytes

            let footer = sprintf "\r\n--%s--\r\n" boundary
                            |> Encoding.UTF8.GetBytes

            task {
                let! fileContent = content
                let buffer = Array.zeroCreate<byte> (header.Length + footer.Length + fileContent.Length)
                use stream = new MemoryStream (buffer)
                do! stream.WriteAsync (header, 0, header.Length)
                do! stream.WriteAsync (fileContent, 0, fileContent.Length)
                do! stream.WriteAsync (footer, 0, footer.Length)

                return buffer
            }

        task {
            let httpRequest = url |> WebRequest.CreateHttp
            httpRequest.Method <- "POST"
            httpRequest.ContentType <- sprintf "multipart/form-data; boundary=%s" boundary
            use stream = httpRequest.GetRequestStream ()
            let! body = filePath |> requestBody
            do! stream.WriteAsync (body, 0, body.Length)
            let! response = httpRequest.GetResponseAsync ()
            use stream = response.GetResponseStream ()
            use reader = new StreamReader (stream)
            let! content = reader.ReadToEndAsync ()
            
            return content |> tryConvert<'T>
        }
