using System.Net;

namespace Sfc.Wms.Asrs.Api
{
    public class SwaggerWindowsAuthConfig
    {
        internal static void RegisterAuth()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:59351/swagger/");
            listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;
            listener.Start();
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            if (!request.IsAuthenticated)
            {
                const string responseString = "<HTML><BODY>Unauthorized access.</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            listener.Stop();
        }
    }
}