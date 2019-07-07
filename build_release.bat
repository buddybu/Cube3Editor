@echo off
if exist Cube3Utils rd Cube3Utils /s/q
md Cube3Utils
xcopy D:\proj\bc-csharp-release-1.8.5\crypto\bin\Release\lib\net20\* Cube3Utils
xcopy D:\proj\dariusdamalakas-sourcegrid-2f7f00c20548\SourceGrid\bin\Release\* Cube3Utils
xcopy BitForByteSupport\bin\Release\netstandard2.0\*.dll Cube3Utils
xcopy Cube3Decoder\bin\Release\Cube3Decoder.exe* Cube3Utils
xcopy Cube3Encoder\bin\Release\Cube3Encoder.exe* Cube3Utils
xcopy Cube3Editor\bin\Release\Cube3Editor.exe* Cube3Utils
xcopy Cube3ScriptGenerator\bin\Release\c3sg.exe* Cube3Utils
xcopy FileHelper\bin\Release\netstandard2.0\*.dll Cube3Utils
xcopy CubeExtractor\bin\Release\*.exe* Cube3Utils
xcopy CubeBuilder\bin\Release\*.exe* Cube3Utils

signtool sign /fd SHA256 /a /f Cube3Editor.pfx /p Cow!004-CUBE Cube3Utils\*.dll
rem signtool sign /fd SHA256 /a /f Cube3Editor.pfx /p Cow!004-CUBE Cube3Utils\BitForByteSupport.dll
signtool sign /fd SHA256 /a /f Cube3Editor.pfx /p Cow!004-CUBE Cube3Utils\*.exe


if exist Cube3Utils.zip del Cube3Utils.zip
zip -r Cube3Utils Cube3Utils\*.*


