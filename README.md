# dotnetcore-ecdh-openssl

## What? Why?

Example of how to use [ECDiffieHellmanOpenSsl](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellmanopenssl?view=netcore-2.2), ported from the example for [ECDiffieHellmanCng](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellmancng?view=netcore-2.2), in .Net Core 2.2

Looking for the docs for implementing a Diffie Hellman key exchange in .NET Core 2 will lead you to the [docs for ECDiffieHellmanCng](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellmancng?view=netcore-2.2). This is fine in a Windows environment, but on Mac OS or Linux you'll encounter a PlatformNotSupportedException when running any of the CNG classes. This is because Cryptography Next Generation (CNG) is only supported on Windows.

On Mac/Linux you can use [ECDiffieHellmanOpenSsl](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellmanopenssl?view=netcore-2.2), but there aren't currently .NET Core 2.2 docs or examples for this implementation (at the date of writing). 

This repo contains source for a working .NET Core 2.2 console application, replicating the example code from the CNG class, but using the OpenSSL classes.

## What's different from ECDiffieHellmanCng?

### Required packages

In Windows you'd be adding the CNG package using:

`dotnet add package System.Security.Cryptography.Cng` 

But on Mac/Linux you'll be adding the OpenSSL package using: 

`dotnet add package System.Security.Cryptography.OpenSSL`

### Types

The type for public keys for Alice and Bob change from:

`byte[]` 

to: 

`ECDiffieHellmanPublicKey`

The Diffie Hellman class instantiation changes from:

```cs
using (ECDiffieHellmanCng alice = new ECDiffieHellmanCng())
```

to:

```cs
using (ECDiffieHellmanOpenSsl alice = new ECDiffieHellmanOpenSsl())
```

### Creating Keys

Creating keys in Windows using CNG was:

```cs
alice.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
alice.HashAlgorithm = CngAlgorithm.Sha256;
alicePublicKey = alice.PublicKey.ToByteArray();
Bob bob = new Bob();
CngKey k = CngKey.Import(bob.bobPublicKey, CngKeyBlobFormat.EccPublicBlob);
byte[] alicePrivateKey = alice.DeriveKeyMaterial(CngKey.Import(bob.bobPublicKey, CngKeyBlobFormat.EccPublicBlob));
```

Whereas using OpenSSL:

```cs
AlicePublicKey = alice.PublicKey;
Bob bob = new Bob();
byte[] alicePrivateKey = alice.DeriveKeyMaterial(bob.BobPublicKey);
```
