PDFTool
=======

Small CLI utility to work with PDF files: combine, compress (content streams) and split PDFs.

Features
- Combine multiple PDFs into a single PDF (`combine`).
- Compress a PDF's content streams (`compress`) — note this does not re-encode or downsample embedded images.
- Split a PDF into single-page PDF files (`split`).

Requirements
- .NET 10 SDK
- C# 14 source code
- The project uses PdfSharp. Install via NuGet before building:

  `dotnet add package PdfSharp`

Build

From the repository root:

  `dotnet build`

Run / Usage

You can run the tool with one of the supported commands. When running from the source with `dotnet run`, pass `--` before the tool arguments. Examples below show both forms.

1) Compress a PDF

- CLI (built executable):
  `Program.exe compress <input.pdf> <output.pdf>`

- `dotnet run` from project folder:
  `dotnet run -- compress "in.pdf" "out.pdf"`

Note: This uses PdfSharp's `CompressContentStreams` option. Large embedded or already-compressed images will not be reduced significantly.

2) Combine PDFs

- CLI:
  `Program.exe combine <output.pdf> <in1.pdf> <in2.pdf> [...]

- `dotnet run`:
  `dotnet run -- combine combined.pdf a.pdf b.pdf`

Legacy behavior: calling the program without a command and passing multiple files will combine them into `combined.pdf`.

3) Split a PDF into pages

- CLI:
  `Program.exe split <input.pdf> <outputFolder>`

- `dotnet run`:
  `dotnet run -- split "big.pdf" "out-folder"`

This writes one file per page named `<original>_page_<n>.pdf`.

Limitations and notes
- `CompressPdf` only sets `CompressContentStreams` in PdfSharp. To achieve stronger compression (image downsampling / re-encoding) use tools like Ghostscript or a library that can re-encode images (e.g. iText).

- The tool assumes input files are valid PDFs. It will throw `FileNotFoundException` when inputs are missing.

Contributing
- Open a pull request with fixes or improvements.

License
- Project contains no explicit license file in this repository; add one if you intend to publish.
