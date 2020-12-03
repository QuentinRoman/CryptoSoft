CryptoSoft
==========
Simple XOR encrypt/decrypt software in .NET Core 3.1

Encryption key
--------------
The software requires an encryption key form of a text file. The key mus be 8 characters long (64 bits)

Arguments
---------

##### 1. `--source` or `-s`
The fullpath of the file to encrypt/decrypt

##### 2. `--destination` or `-d`
The fullpath of the file to write out the output

##### 3. `--key` or `-k`
A file containg the encryption key

Return code
-----------
* Any number above `0` : The time in milliseconds ellepsed to encrypt the file
* `-1` : An error as occured during the file encryption
* `-2` : The source file does not exist
* `-3` : The destination directory does not exist
* `-4` : The key file does not exist or the key is wrong
