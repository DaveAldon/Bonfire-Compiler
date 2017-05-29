using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;

namespace firetest
{
    class MainClass
    {
		public static void Main(string[] args)
		{
            
			new MainClass().Run().Wait();
		}

		private async Task Run()
        {
			var firebaseUrl = "https://bonfire-b6633.firebaseio.com";

			// The following username and password connections are created 
			// in the firebase console via https://console.firebase.google.com
			// For more information see https://firebase.google.com/docs/auth/web/password-auth
			var firebaseUsername = "aaaa@aaa.com";
			var firebasePassword = "aaaaaaaaaa";

			// this can be found via the console
			var firebaseApiKey = "AIzaSyB284IXRfrPx3LpNaGGVr1a66JhD3NUxKI";

			var auth = new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey));
			// this grabs a one-off token using the username and password combination
			var token = await auth.SignInWithEmailAndPasswordAsync(firebaseUsername, firebasePassword);

			// finally log in
			var firebaseClient = new FirebaseClient(
									firebaseUrl,
									new FirebaseOptions
									{
										AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken)
									});

            // now you can make your query
            var results = firebaseClient
                .Child("editor_values/Public").OnceAsync<string>();

			foreach (var editor in results.Result)
			{
                Console.WriteLine($"{editor.Key} is {editor.Object}");
			}
		}
	}
}