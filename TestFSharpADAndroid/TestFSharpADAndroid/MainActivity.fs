namespace TestFSharpADAndroid

open System

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget
open System.Linq
open Microsoft.IdentityModel.Clients.ActiveDirectory

type Resources = TestFSharpADAndroid.Resource

[<Activity (Label = "TestFSharpADAndroid", MainLauncher = true)>]
type MainActivity () =
    inherit Activity ()

    let mutable count:int = 1

    override this.OnCreate (bundle) =

        base.OnCreate (bundle)

        // Set our view from the "main" layout resource
        this.SetContentView (Resources.Layout.Main)

        let statusTextView = this.FindViewById<TextView>(Resources.Id.statustextView)

        // Get our button from the layout resource, and attach an event to it
        let button = this.FindViewById<Button>(Resources.Id.connectionButton)

        let AuthenticateAsync(authority, resource, clientId, returnUri) =
            let firstAuthContext = new AuthenticationContext(authority)
            let uri = new Uri(returnUri)

            let authContext =
                match firstAuthContext.TokenCache.ReadItems().Any() with
                | true  -> new AuthenticationContext(firstAuthContext.TokenCache.ReadItems().First().Authority)
                | false -> firstAuthContext

            let activity = this :> Android.App.Activity
            authContext.AcquireTokenAsync(resource, clientId, uri, new PlatformParameters(activity))

        let GetTokenAsync() =
            async {
                try
                    let! result = AuthenticateAsync(GlobalConfiguration.Authority, GlobalConfiguration.AppId, GlobalConfiguration.ClientId, GlobalConfiguration.RedirectUri) |> Async.AwaitTask
                    let ret = match result with
                                | null -> None
                                | _    -> Some(result)
                    return ret
                with
                   | exn -> return None
            }

        let LoginAsync() =
            async {
                let! token = GetTokenAsync()
                let tokenStatus = match token with
                                  | Some t -> true
                                  | None   -> false
                return tokenStatus
            }

        button.Click.Add (fun args -> 
            async {
                let! isAuth = LoginAsync()
                match isAuth with
                | true -> ()
                | false -> ()
            } |> Async.Start
        )

