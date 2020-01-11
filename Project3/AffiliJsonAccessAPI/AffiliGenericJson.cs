using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace AffiliJsonAccessAPI
{
    /// <summary>
    /// Basic implementation to read/write jason syntax to .net objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AffiliGenericJson<T>:IAffiliJSon<T>
    {
        /// <summary>
        /// Gets the object from json string.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public T GetObjectFromJsonString(string json)
        {
            T responseObject = JsonConvert.DeserializeObject<T>(json);
            return responseObject;
        }

        /// <summary>
        /// Gets the object from json stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public T GetObjectFromJsonStream(String uri)
        {
            return GetObjectFromJsonStream(uri, null);
        }

        /// <summary>
        /// Gets the object from json stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="userAgaent">The user agaent.</param>
        /// <returns></returns>
        public T GetObjectFromJsonStream(String uri, String userAgaent)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    if (!String.IsNullOrEmpty(userAgaent)) client.Headers.Add("User-Agent", userAgaent);
                    String responseString = client.DownloadString(new Uri(uri));

                    return GetObjectFromJsonString(responseString);
                }
            }
            catch (WebException ex)
            {
                throw;//todo implement affili jason exception or throw base exception from service utility
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Writes the object to json string.
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <returns></returns>
        public String WriteObjectToJsonString(T sourceObject)
        {
            return  JsonConvert.SerializeObject(sourceObject);
        }

        /// <summary>
        /// Writes the object in json format to a text file.
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="fileUri">The file URI. e.g. d:\data\jsonObjects.txt</param>
        public void WriteObjectToJsonFileStream(T sourceObject, String fileUri)
        {
            JsonSerializer serializer = null;

            try
            {
                serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(fileUri))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, sourceObject);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                serializer = null;
            }
        }
    }
}