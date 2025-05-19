[Setup]
AppName=PII Redactor Clipboard Manager
AppVersion=1.0
DefaultDirName={pf}\PIIRedactor
OutputBaseFilename=PIIRedactorSetup

[Files]
Source: "..\src\PIIRedactorApp\bin\Release\net6.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\PII Redactor"; Filename: "{app}\PIIRedactorApp.exe"
