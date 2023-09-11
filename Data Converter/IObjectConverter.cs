namespace Messanger.Data.ObjectConverterService
{
    internal interface IObjectConverter
    {
        /// <summary>
        /// Convert an object to byte array.
        /// </summary>
        /// <param name="sender">Object that needs to be converted.</param>
        /// <returns>Object as byte array.</returns>
        byte[] ObjectToBytes(object sender);
        /// <summary>
        /// Convert bytes to an object
        /// </summary>
        /// <param name="bytes">Bytes that needs to be converted.</param>
        /// <returns>Object any type.</returns>
        object BytesToObject(byte[] bytes);
    }
}
