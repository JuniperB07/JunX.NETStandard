using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JunX.NETStandard.EncryptionService
{
    /// <summary>
    /// Provides symmetric encryption and decryption services using AES, with keys derived from a user-supplied string.
    /// </summary>
    /// <remarks>
    /// This class generates a SHA256-based encryption key and an MD5-based initialization vector (IV) from the provided input string.
    /// It supports secure transformation of plaintext to encrypted Base64 strings and vice versa.
    /// Designed for scenarios requiring lightweight, consistent encryption logic across applications.
    /// </remarks>
    public class EncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionService"/> class using the specified key string to derive encryption parameters.
        /// </summary>
        /// <param name="Key">
        /// A string used to generate the AES encryption key and initialization vector (IV).
        /// </param>
        /// <remarks>
        /// The provided key string is hashed using SHA256 to produce the encryption key and MD5 to produce the IV.
        /// This ensures consistent and deterministic cryptographic parameters for symmetric encryption and decryption.
        /// </remarks>
        public EncryptionService(string Key)
        {
            SHA256 sha = SHA256.Create();
            MD5 md5 = MD5.Create();

            _key = sha.ComputeHash(Encoding.UTF8.GetBytes(Key));
            _iv = md5.ComputeHash(Encoding.UTF8.GetBytes(Key));
        }

        /// <summary>
        /// Encrypts the specified plaintext string using AES and returns the result as a Base64-encoded string.
        /// </summary>
        /// <param name="Raw">
        /// The plaintext string to encrypt.
        /// </param>
        /// <returns>
        /// A Base64-encoded string representing the encrypted form of the input.
        /// </returns>
        /// <exception cref="InvalidTextParameterException">
        /// Thrown when encryption fails due to invalid input or a cryptographic error.
        /// </exception>
        /// <remarks>
        /// This method uses AES encryption with a key and IV derived from the constructor input.
        /// The encrypted output is written to a memory stream and encoded as Base64 for safe transport or storage.
        /// </remarks>
        public string Encrypt(string Raw)
        {
            try
            {
                Aes aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);

                sw.Write(Raw);
                sw.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw new InvalidTextParameterException("Encryption failed due to invalid input or cryptographic error.", ex);
            }
        }

        /// <summary>
        /// Decrypts the specified Base64-encoded string using AES and returns the original plaintext.
        /// </summary>
        /// <param name="Encrypted">
        /// A Base64-encoded string representing the encrypted data.
        /// </param>
        /// <returns>
        /// The decrypted plaintext string.
        /// </returns>
        /// <exception cref="InvalidTextParameterException">
        /// Thrown when the input is not a valid Base64 string or when decryption fails due to invalid cryptographic parameters or corrupted data.
        /// </exception>
        /// <remarks>
        /// This method uses AES decryption with a key and IV derived from the constructor input.
        /// It expects the input to be a valid Base64 string produced by the <c>Encrypt</c> method.
        /// </remarks>
        public string Decrypt(string Encrypted)
        {
            try
            {
                Aes aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                byte[] buffer;
                try
                {
                    buffer = Convert.FromBase64String(Encrypted);
                }
                catch (FormatException ex)
                {
                    throw new InvalidTextParameterException("The provided text is not a valid Base64 string.", ex);
                }

                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch (CryptographicException ex)
            {
                throw new InvalidTextParameterException("Decryption failed due to invalid key, IV, or corrupted data.", ex);
            }
        }

        /// <summary>
        /// Attempts to decrypt the specified value and returns a success flag.
        /// </summary>
        /// <param name="Value">
        /// The encrypted string to be decrypted.
        /// </param>
        /// <returns>
        /// <c>true</c> if decryption succeeds without throwing an exception; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Wraps the <c>Decrypt</c> method in a try-catch block to suppress exceptions and indicate success.
        /// </remarks>
        public bool TryDecrypt(string Value)
        {
            try
            {
                Decrypt(Value);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Represents errors that occur when text input is invalid or incompatible with encryption or decryption operations.
    /// </summary>
    /// <remarks>
    /// This custom exception is used to wrap cryptographic and formatting errors with domain-specific context.
    /// It supports default, message-only, and inner-exception constructors for flexible error handling.
    /// </remarks>
    public class InvalidTextParameterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTextParameterException"/> class with no message or inner exception.
        /// </summary>
        /// <remarks>
        /// This constructor is useful when throwing a generic exception without additional context.
        /// </remarks>
        public InvalidTextParameterException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTextParameterException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// A descriptive message that explains the reason for the exception.
        /// </param>
        /// <remarks>
        /// Use this constructor to provide context-specific error details when throwing the exception.
        /// </remarks>
        public InvalidTextParameterException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTextParameterException"/> class with a specified error message and a reference to the inner exception that caused this exception.
        /// </summary>
        /// <param name="message">
        /// A descriptive message that explains the reason for the exception.
        /// </param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        /// <remarks>
        /// Use this constructor to wrap lower-level exceptions with domain-specific context for encryption and decryption failures.
        /// </remarks>
        public InvalidTextParameterException(string message, Exception inner)
            : base(message, inner) { }

        /// <summary>
        /// Returns a string representation of the current <see cref="InvalidTextParameterException"/> instance.
        /// </summary>
        /// <returns>
        /// A string that describes the exception, including the message and stack trace if available.
        /// </returns>
        /// <remarks>
        /// This override delegates to the base implementation and is useful for logging or debugging exception details.
        /// </remarks>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
