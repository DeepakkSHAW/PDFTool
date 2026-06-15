
using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("============PDF Tool=========");
		// 1. Validate command-line arguments
		if (args.Length < 2)
		{
			Console.WriteLine("Error: Please provide at least two PDF files to combine.");
			Console.WriteLine("Usage: Program.exe <file1.pdf> <file2.pdf> [file3.pdf ...]");
			return;
		}

		string outputFile = "combined.pdf";

		// 2. Execute the combination
		try
		{
			CombinePdfs(args, outputFile);
			Console.WriteLine($"Success! Created: {Path.GetFullPath(outputFile)}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}

	static void CombinePdfs(string[] sourceFiles, string outputFile)
	{
		// Target document using the free PdfSharp library
		using (PdfDocument outputDocument = new PdfDocument())
		{
			foreach (string file in sourceFiles)
			{
				if (!File.Exists(file))
				{
					throw new FileNotFoundException($"The file '{file}' could not be found.");
				}

				// Open the source document in Import mode
				using (PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import))
				{
					int pageCount = inputDocument.PageCount;
					for (int idx = 0; idx < pageCount; idx++)
					{
						PdfPage page = inputDocument.Pages[idx];
						outputDocument.AddPage(page);
					}
				}
			}

			// Save the final file
			outputDocument.Save(outputFile);
		}
	}
}
