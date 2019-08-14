# FileHasher
A bare-bones WPF app that calculates hashes of a file so that the user can check its integrity. 

Many sites show hashes of files they publish so that the user can check that they have downloaded what was uploaded and 
that it hasn't been tampered with. As a Windows user, I can't be bothered to work out how to check these values, so I never bother. 
Yeah, I could fire up a git bash prompt and use `md5sum` or `sha256sum` or whatever, but that doesn't work for everyone.  

I knocked-up this little app to scratch this itch. It's a bare-minimum WPF app - no fancy frameworks or even any styling. 
If it's invoked with one argument, it expects that to be the filename of the file to hash - and it displays the md5, sha1 and sha256 values of it. 
If it's invoked with no arguments, it will offer to register itself as a shell extension so that you can easily launch it from Explorer by 
right-clicking on a file and choosing "Calculate hashes". If it's already registered, it will offer to unregister itself. 

Currently it only works on one file at a time - if called with more than one argument it will just do nothing. 
