@echo off
if exist Cube3Utils rd Cube3Utils /s/q
md Cube3Utils
copy D:\proj\bc-csharp-release-1.8.5\crypto\bin\Release\lib\net20\* Cube3Utils
copy D:\proj\dariusdamalakas-sourcegrid-2f7f00c20548\SourceGrid\bin\Release\* Cube3Utils
copy BitForByteSupport\bin\Release\netstandard2.0\*.dll Cube3Utils
copy Cube3Decoder\bin\Release\Cube3Decoder.exe Cube3Utils
copy Cube3Encoder\bin\Release\Cube3Encoder.exe Cube3Utils
copy Cube3Editor\bin\Release\Cube3Editor.exe Cube3Utils
copy Cube3ScriptGenerator\bin\Release\c3sg.exe Cube3Utils
copy FileHelper\bin\Release\netstandard2.0\*.dll Cube3Utils

if exist Cube3Utils.zip del Cube3Utils.zip
zip -r Cube3Utils Cube3Utils\*.*


