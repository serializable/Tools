// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateStory.cs" company="RBS GBM">
//   Copyright © RBS GBM 2010
// </copyright>
// <summary>
//   Defines the CreateStory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VersionOneAdapter
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public class CreateStory
    {
        private const string HostName = "lonmw65494";

        public void Execute(Story story)
        {
            string createStoryRequest = string.Format(@"
<Asset>
	<Attribute name=""Name"" act=""set"">{0}</Attribute>
    <Attribute name=""Estimate"" act=""set"">{1}</Attribute>
    <Attribute name=""Description"" act=""set"">{2}</Attribute>
	<Relation name=""Scope"" act=""set"">
		<Asset idref=""Scope:2636"" />
	</Relation>
</Asset>", story.Title, story.Estimate, story.Description);

            WebRequest request =
                WebRequest.Create(string.Format("http://{0}/VersionOne/rest-1.v1/Data/Story",
                                                HostName));
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            SetBasicAuthHeader(request, "Tom Peplow", "password1");
            byte[] content = Encoding.UTF8.GetBytes(createStoryRequest);
            request.ContentLength = content.Length;


            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(content, 0, content.Length);
                requestStream.Close();
            }
            using (var response = request.GetResponse())
            {
                Console.WriteLine(((HttpWebResponse) response).StatusDescription);
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                        reader.Close();
                    }
                    responseStream.Close();
                }
                response.Close();
            }
        }

        public void SetBasicAuthHeader(WebRequest req, String userName, String userPassword)
        {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            req.Headers["Authorization"] = "Basic " + authInfo;
        }
    }
}