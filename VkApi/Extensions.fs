namespace VkApi.Core


module internal Extensions =

    open FSharp.Control.Tasks.V2
    open System.Net
    open System.IO


    type HttpWebRequest with
        member self.AsyncGetResponse () =
            task {
                use! response = self.GetResponseAsync ()
                use stream = response.GetResponseStream ()
                use reader = new StreamReader (stream)
                let! content = reader.ReadToEndAsync ()

                return if content.Contains "error" then
                            Error content
                       else
                            Ok content
            }

