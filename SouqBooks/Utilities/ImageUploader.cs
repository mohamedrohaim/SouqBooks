namespace SouqBooks.Utilities
{
	public class ImageUploader
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private List<string> _allowedExtensions = new List<string> { ".png", ".jpg", ".jpeg" };

		public ImageUploader(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public bool IsImageFile(string fileName)
		{
			var extension = Path.GetExtension(fileName).ToLowerInvariant();
			return _allowedExtensions.Contains(extension);
		}


		public string UploadImage(IFormFile file,string folder) {

			//Guid create unique file name
			var uniqueFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
			var webRootPath = _webHostEnvironment.WebRootPath;
			var imagePath = Path.Combine(webRootPath, "Images", folder);
			var filePath = Path.Combine(imagePath, uniqueFileName);
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(fileStream);
			}

			return uniqueFileName;

		}

		public bool DeleteFile(string filePath)
		{
			try
			{
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
					return true;
				}
				else
				{
					// File does not exist
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}








	}
}
