using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AffiliJsonAccessAPI
{
    /// <summary>
    /// Basic interface to read/write jason syntax to .net objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAffiliJSon<T>
    {
        /// <summary>
        /// Gets the object from json string.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        T GetObjectFromJsonString(String json);
        /// <summary>
        /// Gets the object from json stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        T GetObjectFromJsonStream(String uri);
        /// <summary>
        /// Gets the object from json stream.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="userAgaent">The user agaent.</param>
        /// <returns></returns>
        T GetObjectFromJsonStream(String uri, String userAgaent);
        /// <summary>
        /// Writes the object to json string.
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <returns></returns>
        String WriteObjectToJsonString(T sourceObject);
        /// <summary>
        /// Writes the object in json format to a text file.
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="fileUri">The file URI. e.g. d:\data\jsonObjects.txt</param>
        void WriteObjectToJsonFileStream(T sourceObject, String fileUri);
    }
}
