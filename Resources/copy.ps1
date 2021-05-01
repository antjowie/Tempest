$name = $args[0]
$bindir = $args[1]
$solutiondir = $args[2]
$file = "$solutiondir/Resources/ror2.txt"

#If the file does not exist, create it.
if (-not(Test-Path -Path $file -PathType Leaf)) {
    try {
        $null = New-Item -ItemType File -Path $file -Force -ErrorAction Stop
        Write-Host "$file has been created in Resources dir. Write the ror2 installation dir to auto copy the mod binaries" -ForegroundColor DarkGreen
    }
    catch {
        throw $_.Exception.Message
    }
}
# If the file already exists, show the message and do nothing.
else {
    $path = Get-Content $file

    if(!$path)
    {
        Write-Host "The ror2.txt file is empty. Auto copy is ignored." -ForegroundColor DarkGreen
        
    }
    elseif ($path.EndsWith("Risk of Rain 2")) {
        $moddir = "/BepInEx/plugins/$name"
        $path += $moddir
        Write-Host "Copying $name.dll to $path"
        
        robocopy "$bindir" "$path" "$name.dll" /mir /is /it
        # (robocopy "$bindir" "$path" "$name.dll" /mir /is /it) ^& IF %ERRORLEVEL% LSS 8 SET ERRORLEVEL = 0    
    }
    else {
        Write-Host "The ror2.txt file does not contain a correct path. Make sure it ends with 'Risk of Rain 2'" -ForegroundColor DarkGreen
    } 
}