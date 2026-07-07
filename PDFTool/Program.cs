
using System;
using System.IO;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("============PDF Tool=========");
		if (args.Length == 0)
		{
			PrintUsage();
			return;
		}

		string cmd = args[0].ToLowerInvariant();

		try
		{
			if (cmd == "compress")
			{
				// Usage: compress <input.pdf> <output.pdf>
				if (args.Length < 3)
				{
					Console.WriteLine("Error: compress requires input and output file paths.");
					PrintUsage();
					return;
				}

				string input = args[1];
				string output = args[2];
				CompressPdf(input, output);
				Console.WriteLine($"Success! Compressed: {Path.GetFullPath(output)}");
			}
			else if (cmd == "combine")
			{
				// Usage: combine <output.pdf> <input1.pdf> <input2.pdf> [...]
				if (args.Length < 3)
				{
					Console.WriteLine("Error: combine requires an output file and at least one input file.");
					PrintUsage();
					return;
				}

				string output = args[1];
				string[] inputs = args.Skip(2).ToArray();
				CombinePdfs(inputs, output);
				Console.WriteLine($"Success! Created: {Path.GetFullPath(output)}");
			}
			else
			{
				// Backwards-compatible: if first arg isn't a command, treat all args as files to combine into combined.pdf
				if (args.Length < 2)
				{
					Console.WriteLine("Error: Please provide at least two PDF files to combine.");
					PrintUsage();
					return;
				}

				string outputFile = "combined.pdf";
				CombinePdfs(args, outputFile);
				Console.WriteLine($"Success! Created: {Path.GetFullPath(outputFile)}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}

	static void PrintUsage()
	{
		Console.WriteLine("Usage:");
		Console.WriteLine("  Program.exe compress <input.pdf> <output.pdf>   - Compress a PDF file");
		Console.WriteLine("  Program.exe combine <output.pdf> <in1.pdf> <in2.pdf> [...] - Combine PDFs");
		Console.WriteLine("  Program.exe <file1.pdf> <file2.pdf> [...] - Combine into combined.pdf (legacy)");
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

	// Compress a single PDF file. Provide an input PDF path and an output path for the compressed file.
	static void CompressPdf(string inputFile, string outputFile)
	{
		if (!File.Exists(inputFile))
		{
			throw new FileNotFoundException($"The file '{inputFile}' could not be found.");
		}

		// Open the document in Modify mode so we can change save options
		using (PdfDocument document = PdfReader.Open(inputFile, PdfDocumentOpenMode.Modify))
		{
			// Enable content stream compression
			document.Options.CompressContentStreams = true;

			// Save to the requested output file
			document.Save(outputFile);
		}
	}
}
