## cloudinary-dotnet
cloudinary-dotnet is a library to easily integrate [Cloudinary](http://cloudinary.com) into your ASP.net MVC projects. The library allows you to easily upload files to your Cloudinary cloud, and at the same time provides some Url and Html helpers to access your images.

### Installation
If you're not interested in the source and simply want to add Cloudinary to your project. Then there is a [nuget package](https://nuget.org/packages/Cloudinary) available. From the Package Manager Console, do:

        Install-Package Cloudinary

### Uploading images
For uploading images you can use the `Uploader` class. It takes an `AccountConfiguration` instance (containing your cloud's name, api key and api secret) in the constructor. You can then use the  the `Upload` to upload Images.

Example:
```csharp
var configuration = new AccountConfiguration("your cloud name",
                                             "your api key",
                                             "your secret");

// Setup the uploader
var uploader = new Uploader(configuration);

string publicId = Path.GetFileNameWithoutExtension(filename);

using(var stream =new FileStream(filename, FileMode.Open))
{
 	// Upload the file
    var uploadResult = uploader.Upload(new UploadInformation(filename, stream)
                        {
						 	// explicitly specify a public id (optional)
                            PublicId = publicId,
							// set the format, (default is jpg)
                            Format = "png",
							// Specify some eager transformations														 
                            Eager = new[]
                                        {
                                            new Transformation(240, 240),
                                            new Transformation(120, 360) { Crop = CropMode.Limit },
                                        }
                        });

	// Write some output info	
    Console.WriteLine("Version: {0}, PublicId {1}", uploadResult.Version, uploadResult.PublicId);
    Console.WriteLine("Url: {0}", uploadResult.Url);
}
Console.WriteLine("Successfully uploaded file");
```

### Creating Cloudinary Urls
In order to easily access your Cloudinary images, there are some UrlHelper extension methods. The easiest way to work
with them, is when you first initialize the AccountConfiguration statically. That way, you can use the helper methods
without needing the specify the `AccountConfiguration` object over and over. 

For example, in your application startup routine, put the following method:
```csharp
var configuration = new AccountConfiguration("your cloud name",
                                             "your api key",
                                             "your secret");

AccountConfiguration.Initialize(configuration);
```

Once that is done, you can call the `CloudinaryImage` extension methods without providing 
```csharp
<h3>Original</h3>
<img src="@Url.CloudinaryImage("sample")" alt="sample"/> 

<h3>Cropped 140x140</h3>
<img src="@Url.CloudinaryImage("sample", new Transformation(140, 140) { Crop = CropMode.Crop })" alt="cropped"/>
```

### TODO's
This library is far from done, here are some things that are still on the TODO list

* Correct creation of https urls
* More tests
* Uploading without reading the entire stream into memory
