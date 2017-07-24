# clear out dist folder
$strFolderName="dist"
If (Test-Path $strFolderName){
	Remove-Item $strFolderName  -Recurse -Force
}
New-Item dist -type directory

# restore and build
dotnet clean
dotnet restore
dotnet build

# publish
dotnet publish -o dist -c Release --runtime win10-x64