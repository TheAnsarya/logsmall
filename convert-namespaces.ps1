# Recursively process all .cs files in the current directory
Get-ChildItem -Path . -Filter *.cs -Recurse | ForEach-Object {
    $file = $_.FullName
    $content = Get-Content $file -Raw

    # Regex to match block-scoped namespace
    if ($content -match 'namespace\s+([A-Za-z0-9_.]+)\s*\{') {
        # Extract namespace name
        $namespace = $matches[1]

        # Replace the block-scoped namespace with file-scoped
        $content = $content -replace "namespace\s+$namespace\s*\{", "namespace $namespace;`n"

        # Remove the last closing brace in the file (assumed to be the namespace's)
        $lines = $content -split "`n"
        $braceIndex = ($lines | Select-String -Pattern '^\s*\}\s*$' | Select-Object -Last 1).LineNumber
        if ($braceIndex) {
            $lines = $lines[0..($braceIndex-2)] + $lines[$braceIndex..($lines.Length-1)]
        }

        # Dedent all lines (remove one tab or 4 spaces from the start of each line)
        $lines = $lines | ForEach-Object { $_ -replace '^(    |\t)', '' }

        # Write back to file
        Set-Content $file -Value ($lines -join "`n")
        Write-Host "Converted: $file"
    }
}