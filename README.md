# dotnetcore-ecdh-openssl
Example of how to use [ECDiffieHellmanOpenSsl](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellmanopenssl?view=netcore-2.2), ported from the example for [ECDiffieHellmanCng](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellmancng?view=netcore-2.2), in .Net Core 2.2

Looking for the docs for implementing a Diffie Hellman key exchange in .NET Core 2 will lead you to the [docs for ECDiffieHellmanCng](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellmancng?view=netcore-2.2). This is fine in a Windows environment, but on Mac OS or Linux you'll encounter a PlatformNotSupportedException when running any of the CNG classes. This is because Cryptography Next Generation (CNG) is only supported on Windows.

On Mac/Linux you can use [ECDiffieHellmanOpenSsl](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellmanopenssl?view=netcore-2.2), but there aren't currently .NET Core 2.2 docs or examples for this implementation (at the date of writing). 

This repo contains source for a working .NET Core 2.2 console application, replicating the example code from the CNG class, but using the OpenSSL classes.
